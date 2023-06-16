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

using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件记录流程仓储接口
    /// </summary>
    public interface IFaultRecordFlowRepository
    {
        /// <summary>
        /// 插入事件记录流程
        /// </summary>
        /// <param name="fault_Record_Flow">等待被写入的事件流程对象</param>
        /// <param name="conn">IDbConnection</param>
        /// <param name="transaction">IDbTransaction</param>
        /// <returns>bool</returns>
        public bool Insert(fault_record_flow fault_Record_Flow, IDbConnection conn, IDbTransaction transaction);


        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到事件记录id
        /// </summary>
        /// <returns></returns>
        public List<string> GetRecordsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction);

        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到流程
        /// </summary>
        /// <returns></returns>
        public List<fault_record_flow> GetFlowsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction);

    }
}
