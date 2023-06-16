/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Application;
using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    public class ExamPlanMethodItemRepository : IExamPlanMethodItemRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public ExamPlanMethodItemRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public List<exam_plan_method_item> GetItemByPlanId(string exam_plan_method_id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<exam_plan_method_item> data = conn.GetByOneFeildsSql<exam_plan_method_item>("exam_plan_method_id", exam_plan_method_id).ToList(); ;
                return data;
            }
        }

        /// <summary>
        /// 根据点检计划和设备id在点检计划-点检项表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_item> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id)
        {

            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<exam_plan_method_item> data = conn.GetByTwoFeildsSql<exam_plan_method_item>("exam_plan_method_id", exam_plan_method_id, "equipment_id", equipment_id).OrderBy(c => c.examitem_code).ToList(); ;
                return data;
            }

        }




        /// <summary>
        /// 添加点检计划-项目
        /// </summary>
        /// <param name="data">数组参数data的equipment_id 和 exam_plan_method_id是必传字段，其他内容能填尽量填</param>
        /// <param name="equipment_id"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodItem(List<exam_plan_method_item> data, string equipment_id, string exam_plan_method_id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    foreach (exam_plan_method_item temp in data)
                    {
                        if (string.IsNullOrEmpty(temp.equipment_id))
                        {
                            throw new Exception("未选中设备");
                        }
                        if (string.IsNullOrEmpty(temp.examitem_id))
                        {
                            throw new Exception("未选中点检项");
                        }

                        List<exam_plan_method_item> _base = _pikachuRepository.GetAll<exam_plan_method_item>(dbConnection: conn, tran: transaction)
                            .Where(a => a.examitem_id == temp.examitem_id && a.exam_plan_method_id == temp.exam_plan_method_id && a.equipment_id == temp.equipment_id).ToList();
                        if (_base.Count != 0)
                        {
                            throw new Exception("该点检项目重复添加");
                        }
                        //取ID
                        temp.exam_plan_method_item_id = CommonHelper.GetNextGUID();
                        temp.modified_time = Time.Now;
                        temp.create_time = DateTime.Now;

                        temp.examitem_code = _pikachuRepository.GetById<base_examitem>(temp.examitem_id, dbConnection: conn, tran: transaction).examitem_code;
                        temp.examitem_name = _pikachuRepository.GetById<base_examitem>(temp.examitem_id, dbConnection: conn, tran: transaction).examitem_name;
                        temp.create_by = _auth.GetUserAccount(null);
                        temp.create_by_name = _auth.GetUserName(null);
                        temp.modified_by = _auth.GetUserAccount(null);
                        temp.modified_by_name = _auth.GetUserName(null);

                        temp.equipment_id = equipment_id;
                        temp.examitem_name = _pikachuRepository.GetById<base_examitem>(temp.examitem_id, dbConnection: conn, tran: transaction).examitem_name;
                        temp.examitem_std = _pikachuRepository.GetById<base_examitem>(temp.examitem_id, dbConnection: conn, tran: transaction).examitem_std;
                        //temp.examschema_id = //点检项目定义表中没有 维保方案编号字段
                        temp.exam_plan_method_id = exam_plan_method_id;
                        _pikachuRepository.Insert(temp, dbConnection: conn, tran: transaction);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception(exception.Message);
                }
            }
        }
    }
}
