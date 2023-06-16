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

using Dapper;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件记录仓储
    /// </summary>
    public class FaultRecordRepository : IFaultRecordRepository, IDependency
    {
        private readonly PikachuRepository _PikachuRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pikachuRepository"></param>
        public FaultRecordRepository(PikachuRepository pikachuRepository)
        {
            _PikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 根据设备id获取事件记录
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<fault_record> GetFaultRecordByEquipmentId(string equipment_id, IDbConnection dbConnection=null,IDbTransaction transaction=null)
        {
            IDbConnection conn = dbConnection??SqlServerDbHelper.GetConn();
            
            List<fault_record> data = conn.GetByOneFeildsSql<fault_record>("equipment_id", equipment_id,tran:transaction);

            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// 插入事件记录
        /// </summary>
        /// <param name="fault_Record">被写入的事件记录对象</param>
        /// <param name="conn">IDbConnection</param>
        /// <param name="transaction">IDbTransaction</param>
        /// <returns></returns>
        public bool Insert(fault_record fault_Record, IDbConnection conn, IDbTransaction transaction)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            if (conn.Insert(fault_Record, transaction) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过时间范围和异常类型找异常事件类型挂载的 异常时间 对应的 事件记录
        /// </summary>
        public List<fault_record> GetByTimeScopeAndFaultClass(DateTime StartTime, DateTime EndTime, base_fault_class fault_Class)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            //找到类型下的异常
            string sql = $"select * from base_fault where fault_class_id = '{fault_Class.fault_class_id}'";
            List<base_fault> base_Faults = conn.Query<base_fault>(sql: sql).ToList();

            List<fault_record> fault_Records = new();
            //找到对应异常下 时间范围内 的事件
            foreach (base_fault base_Fault in base_Faults)
            {
                sql = $"select * from fault_record where create_time between '{StartTime}' and '{EndTime}' and fault_id = '{base_Fault.fault_id}'";
                List<fault_record> data = conn.Query<fault_record>(sql: sql).ToList();
                if (data.Count != 0)
                {
                    fault_Records.AddRange(data);
                }
            }
            return fault_Records;
        }


        /// <summary>
        /// 找到所有parent_id = 参数.动态解析id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<base_fault_class> GetAllByParent(base_fault_class obj)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            string sql = $"select * from base_fault_class where fault_class_parentid = '{obj.fault_class_id}'";
            List<base_fault_class> data = conn.Query<base_fault_class>(sql: sql).ToList();
            return data;
        }


    }
}
