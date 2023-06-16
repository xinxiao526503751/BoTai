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
    /// <summary>
    /// 点检计划-设备
    /// </summary>
    public class ExamPlanMethodEquipmentRepository : IExamPlanMethodEquipmentRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public ExamPlanMethodEquipmentRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 传入点检计划id  查找点检计划-设备关联表 返回计划-设备关联数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetExamPlanMethodEquipmentByPlanId(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string[] id_ = new string[1];
                id_[0] = id;
                List<exam_plan_method_equipment> data = conn.GetByIdsWithField<exam_plan_method_equipment>("exam_plan_method_id", id_).ToList();
                return data;
            }
        }

        /// <summary>
        ///  传入设备-点检计划id 在点检计划-设备表中假删除对应的关联
        /// </summary>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //开启事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    foreach (string id_temp in ids)
                    {
                        exam_plan_method_equipment exam_Plan_Method_Equipment = _pikachuRepository.GetById<exam_plan_method_equipment>(id_temp);
                        string[] dele_str = new string[] { id_temp };
                        //删除计划-设备关联
                        _pikachuRepository.Delete_Mask<exam_plan_method_equipment>(dele_str, dbConnection: conn, tran: transaction);
                        List<string> equip_str = new();
                        equip_str.Add(exam_Plan_Method_Equipment.equipment_id);
                        //删除计划-项目关联
                        List<exam_plan_method_item> exam_Plan_Method_Items = _pikachuRepository.GetByTwoFeildsSql<exam_plan_method_item>("exam_plan_method_id", exam_Plan_Method_Equipment.exam_plan_method_id, "equipment_id", equip_str, dbConnection: conn, tran: transaction);
                        foreach (exam_plan_method_item exam_Plan_Method_Item in exam_Plan_Method_Items)
                        {
                            string[] dele_str_t = new string[] { exam_Plan_Method_Item.exam_plan_method_item_id };
                            _pikachuRepository.Delete_Mask<exam_plan_method_item>(dele_str_t, dbConnection: conn, tran: transaction);
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }


        /// <summary>
        /// 添加点检计划-设备关联
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodEquipment(List<exam_plan_method_equipment> obj, string exam_plan_method_id, string method_type)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //开启事务
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    foreach (exam_plan_method_equipment exam_Plan_Method_Equipment in obj)
                    {
                        //获取要关联的设备
                        base_equipment base_Equipment = _pikachuRepository.GetById<base_equipment>(exam_Plan_Method_Equipment.equipment_id, dbConnection: conn, tran: transaction);
                        List<exam_plan_method_equipment> _base = _pikachuRepository.GetAll<exam_plan_method_equipment>(dbConnection: conn, tran: transaction)
                         .Where(a => a.equipment_id == base_Equipment.equipment_id && a.exam_plan_method_id == exam_plan_method_id).ToList();
                        if (_base.Count != 0)
                        {
                            throw new Exception("该点检设备重复添加");
                        }

                        exam_plan_method_equipment data = exam_Plan_Method_Equipment;
                        data.equipment_name = base_Equipment.equipment_name;
                        data.equipment_code = base_Equipment.equipment_code;
                        //取ID
                        data.exam_plan_method_equipment_id = CommonHelper.GetNextGUID();
                        data.modified_time = Time.Now;
                        data.create_time = DateTime.Now;

                        string[] equipment_id = new string[1];
                        equipment_id[0] = data.equipment_id;
                        data.create_by = _auth.GetUserAccount(null);
                        data.create_by_name = _auth.GetUserName(null);
                        data.modified_by = _auth.GetUserAccount(null);
                        data.modified_by_name = _auth.GetUserName(null);

                        data.exam_plan_method_id = exam_plan_method_id;
                        data.method_type = method_type;
                        //在点检计划-设备关联表查询 是否已经有该点检计划 和 设备 对应的数据
                        List<exam_plan_method_equipment> exam_Plan_Method_Equipment1 = GetItemByPlanIdAndEquipmentId(data.exam_plan_method_id, equipment_id.ToList(), dbConnection: conn, tran: transaction);
                        if (exam_Plan_Method_Equipment1.Count != 0)
                        {
                            exam_plan_method exam_Plan_Method = _pikachuRepository.GetById<exam_plan_method>(exam_Plan_Method_Equipment.exam_plan_method_id, dbConnection: conn, tran: transaction);
                            throw new Exception($"设备{base_Equipment.equipment_name}和计划{exam_Plan_Method.exam_method_name}已经存在关联");
                        }
                        _pikachuRepository.Insert(data, dbConnection: conn, tran: transaction);
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

        /// <summary>
        /// 根据点检计划和设备id在点检计划-设备表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {

            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            List<exam_plan_method_equipment> data = conn.GetByTwoFeildsSql<exam_plan_method_equipment>("exam_plan_method_id", exam_plan_method_id, "equipment_id", equipment_id, tran: tran).ToList(); ;
            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }


    }
}
