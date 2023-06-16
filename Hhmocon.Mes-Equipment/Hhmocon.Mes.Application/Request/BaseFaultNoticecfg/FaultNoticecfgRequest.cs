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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 事件通知配置页面保存按钮
    /// </summary>
    public class FaultNoticecfgRequest
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public string fault_id;

        /// <summary>
        /// 通知类型(微信：webchat;短信:sms;邮件:email)
        /// </summary>
        public List<string> notice_type;

        /// <summary>
        /// 通知等级 0立即通知 1一级通知 2二级通知 3三级通知
        /// </summary>
        [Range(0, 3)]
        public int notice_level;

        /// <summary>
        /// 被通知用户id
        /// </summary>
        public List<string> user_id;
    }
}
