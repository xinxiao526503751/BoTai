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

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 事件上报请求
    /// </summary>
    public class FaultReportRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string equipment_name;

        /// <summary>
        /// 事件名称
        /// </summary>
        public string fault_name;
    }
}
