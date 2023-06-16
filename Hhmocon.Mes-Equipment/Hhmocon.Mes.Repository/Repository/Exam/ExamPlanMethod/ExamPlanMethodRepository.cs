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

using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    public class ExamPlanMethodRepository : IExamPlanMethodRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger<exam_plan_method> _logger;

        public ExamPlanMethodRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository, ILogger<exam_plan_method> logger)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }
        /// <summary>
        /// 同时插入点检计划和点检规则
        /// </summary>
        /// <returns></returns>
        public bool InsertExamPlanMethodAndExamPlanMethodRule(exam_plan_method data, List<exam_plan_method_rule> rule_data)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //新建事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    //插入点检计划
                    conn.Insert(data, transaction);
                    foreach (exam_plan_method_rule item in rule_data)
                    {
                        //插入点检规则
                        conn.Insert(item, transaction);
                    }
                    //提交事务
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    //事务回滚
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据传入的点检计划ids在ExamPlanMethod表中删除相应的点检计划，并且删除ExamPlanMethodRule表中挂载了该计划的规则
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
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
                    //删除点检计划
                    List<exam_plan_method> data = conn.GetByIds<exam_plan_method>(ids, null, transaction).ToList();
                    foreach (exam_plan_method temp in data)
                    {
                        temp.delete_mark = 1;
                        conn.Update(temp, null, transaction);
                    }
                    //删除ExamPlanMethodRule表中挂载了计划的规则
                    List<exam_plan_method_rule> rules = conn.GetByOneFeildSqlIn<exam_plan_method_rule>("exam_plan_method_id", ids, "*", transaction).ToList();
                    foreach (exam_plan_method_rule temp in rules)
                    {
                        temp.delete_mark = 1;
                        conn.Update(temp, null, transaction);
                    }
                    //删除计划-设备关联表
                    List<exam_plan_method_equipment> exam_Plan_Method_Equipment = conn.GetByOneFeildSqlIn<exam_plan_method_equipment>("exam_plan_method_id", ids, tran: transaction);
                    foreach (exam_plan_method_equipment temp in exam_Plan_Method_Equipment)
                    {
                        string[] idss = new string[] { temp.exam_plan_method_equipment_id };
                        _pikachuRepository.Delete_Mask<exam_plan_method_equipment>(idss, tran: transaction, dbConnection: conn);
                    }
                    //删除计划-项目关联表
                    List<exam_plan_method_item> exam_Plan_Method_Items = conn.GetByOneFeildSqlIn<exam_plan_method_item>("exam_plan_method_id", ids, tran: transaction);
                    foreach (exam_plan_method_item temp in exam_Plan_Method_Items)
                    {
                        string[] idss = new string[] { temp.exam_plan_method_item_id };
                        _pikachuRepository.Delete_Mask<exam_plan_method_item>(idss, dbConnection: conn, tran: transaction);
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 根据equipment获取设备点检计划
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method> GetByEquipmentId(string equipment_id)
        {

            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByEquipmentId<exam_plan_method>(equipment_id);
                List<exam_plan_method> data = _pikachuRepository.GetbySql<exam_plan_method>(sql);
                return data;
            }

        }

        public List<exam_plan_method_equipment> GetepmeByEquipmentId(string equipment_id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByEquipmentId<exam_plan_method_equipment>(equipment_id);
                List<exam_plan_method_equipment> data = _pikachuRepository.GetbySql<exam_plan_method_equipment>(sql);
                return data;
            }

        }
    }
}
