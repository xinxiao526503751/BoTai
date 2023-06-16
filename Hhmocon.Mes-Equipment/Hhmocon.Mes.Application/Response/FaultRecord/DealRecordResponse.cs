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
    /// 事件处理页面，点击处理事件按钮以后，处理事件按钮小页面的数据显示
    /// 应答数据
    /// </summary>
    public class DealRecordResponse
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public string fault_class_name;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string equipment_name;

        /// <summary>
        /// 异常名称
        /// </summary>
        public string fault_name;

        /// <summary>
        /// 异常创建时间
        /// </summary>
        public DateTime create_time;

        /// <summary>
        /// 异常上报人员
        /// </summary>
        public string report_people;

        /// <summary>
        /// 前端组件判断流程用的参数 0未确认 1确认 2处理 3关闭
        /// </summary>
        public int active;
    }
}
