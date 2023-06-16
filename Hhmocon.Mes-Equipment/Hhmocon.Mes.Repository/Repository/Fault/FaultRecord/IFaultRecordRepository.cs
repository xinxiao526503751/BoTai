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
using System;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件记录仓储接口
    /// </summary>
    public interface IFaultRecordRepository
    {
        /// <summary>
        /// 根据设备id获取事件记录
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<fault_record> GetFaultRecordByEquipmentId(string equipment_id, IDbConnection dbConnection = null, IDbTransaction transaction = null);


        /// <summary>
        /// 插入事件记录
        /// </summary>
        /// <param name="fault_Record">被写入的事件记录对象</param>
        /// <param name="conn">IDbConnection</param>
        /// <param name="transaction">IDbTransaction</param>
        /// <returns></returns>
        public bool Insert(fault_record fault_Record, IDbConnection conn, IDbTransaction transaction);

        /// <summary>
        /// 通过时间范围和异常类型找事件
        /// </summary>
        public List<fault_record> GetByTimeScopeAndFaultClass(DateTime StartTime, DateTime EndTime, base_fault_class fault_Class);

        /// <summary>
        /// 找到所有parent_id = 参数.动态解析id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<base_fault_class> GetAllByParent(base_fault_class obj);
    }
}
