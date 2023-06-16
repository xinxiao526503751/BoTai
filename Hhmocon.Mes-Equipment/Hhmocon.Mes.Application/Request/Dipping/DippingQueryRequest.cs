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

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// DippingQuery的接口请求类
    /// </summary>
    public class DippingQueryRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName;

        /// <summary>
        /// 变量名称
        /// </summary>
        public string VariableCode;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime;

        /// <summary>
        /// 页数
        /// </summary>
        public int iPage;

        /// <summary>
        /// 行数
        /// </summary>
        public int iRows;
    }
}
