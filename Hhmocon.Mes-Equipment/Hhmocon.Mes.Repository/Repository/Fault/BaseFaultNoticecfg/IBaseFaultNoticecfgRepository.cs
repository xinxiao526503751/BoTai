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

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 事件通知配置仓储接口
    /// </summary>
    public interface IBaseFaultNoticecfgRepository
    {

        /// <summary>
        /// 根据事件ID和通知等级获取已经保存的通知配置
        /// </summary>
        /// <param name="fault_id">事件id</param>
        /// <param name="notice_level">通知等级</param>
        /// <returns></returns>
        public List<base_fault_noticecfg> GetByFaultIdAndNoticeLevel(string fault_id, string notice_level);

        /// <summary>
        /// 插入事件配置
        /// </summary>
        /// <param name="t">事件</param>
        /// <param name="user_ids">用户Id</param>
        /// <returns></returns>
        public bool InsertBaseFaultNoticeCfg(base_fault_noticecfg t, List<string> user_ids);


    }
}
