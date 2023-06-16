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

using System.ComponentModel;

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 设备定义页面的搜索栏请求
    /// </summary>
    public class EquipmentSearchBarRequest
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        [DefaultValue(1)]
        public int Page { get; set; }
        /// <summary>
        /// 每页数据条数
        /// </summary>
        [DefaultValue(10)]
        public int Rows { get; set; }

        /// <summary>
        /// 地点Id
        /// </summary>
        public string Location_Id { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string Name { get; set; }
    }
}
