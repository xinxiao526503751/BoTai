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
    /// 事件处理页面，点击处理事件按钮以后，处理事件按钮小页面的数据显示
    /// 请求参数
    /// </summary>
    public class DealRecordRequest
    {
        /// <summary>
        /// 事件记录id
        /// </summary>
        public string fault_record_id;

        /// <summary>
        /// 事件名称
        /// </summary>
        public string fault_name;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string equipment_name;

        /// <summary>
        /// 当前流程
        /// </summary>
        public string current_flow;

        /// <summary>
        /// 异常类型
        /// </summary>
        public string fault_class_name;

        /// <summary>
        /// 流程状态
        /// </summary>
        public string flow_status;

        /// <summary>
        /// 异常持续时间
        /// </summary>
        public string fault_duration;

        /// <summary>
        /// 流程持续时长
        /// </summary>
        public string flow_duration;

        /// <summary>
        /// 上报人
        /// </summary>
        public string report_people;


    }
}
