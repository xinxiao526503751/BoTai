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
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件记录流程仓储
    /// </summary>
    public class FaultRecordFlowRepository : IFaultRecordFlowRepository, IDependency
    {
        /// <summary>
        /// 插入事件记录流程
        /// </summary>
        /// <param name="fault_Record_Flow">等待被写入的事件流程对象</param>
        /// <param name="conn">IDbConnection</param>
        /// <param name="transaction">IDbTransaction</param>
        /// <returns>bool</returns>
        public bool Insert(fault_record_flow fault_Record_Flow, IDbConnection conn, IDbTransaction transaction)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            
            if (conn.Insert(fault_Record_Flow, transaction) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到流程
        /// </summary>
        /// <returns></returns>
        public List<fault_record_flow> GetFlowsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            string sql = "select * from fault_record_flow where flow_seq = 1 and is_finish = 0 and notice_flag = 0";
            List<fault_record_flow> fault_Record_Flows = conn.Query<fault_record_flow>(sql, transaction: transaction).ToList();

            if (dbConnection == null)
            {
                conn.Close();
            }

            return fault_Record_Flows;
        }

        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到事件记录id
        /// </summary>
        /// <returns></returns>
        public List<string> GetRecordsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            string sql = "select * from fault_record_flow where flow_seq = 1 and is_finish = 0 and notice_flag = 0";
            List<fault_record_flow> fault_Record_Flows = conn.Query<fault_record_flow>(sql: sql, transaction: transaction).ToList();
            List<string> fault_ids = new();
            foreach (fault_record_flow fault_Record_Flow in fault_Record_Flows)
            {
                if (string.IsNullOrEmpty(fault_Record_Flow.fault_id))
                {
                    fault_ids.Add(fault_Record_Flow.fault_id);
                }
            }

            if (dbConnection == null)
            {
                conn.Close();
            }
            return fault_ids;
        }


    }
}
