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

namespace Hhmocon.Mes.Application.Response
{
    /// <summary>
    /// 事件处理查询 应答页面
    /// </summary>
    public class FaultRecordResponse
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
        /// 流程顺序 0确认 1处理 2关闭
        /// </summary>
        public int flow_seq;

        /// <summary>
        /// 异常类型
        /// </summary>
        public string fault_class;

        /// <summary>
        /// 流程状态
        /// </summary>
        public int flow_status;

        /// <summary>
        /// 异常持续时间
        /// </summary>
        public int fault_duration;

        /// <summary>
        /// 流程持续时长
        /// </summary>
        public int flow_duration;

        /// <summary>
        /// 上报人
        /// </summary>
        public string report_people;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time;
    }
}
