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
    /// 设备分类页面搜索框请求
    /// </summary>
    public class EquipmentClassSearchBarRequest
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
        /// 设备分类name
        /// </summary>
        public string Class_Name { get; set; }

        /// <summary>
        /// 设备分类编码
        /// </summary>
        public string Class_Code { get; set; }

        /// <summary>
        /// 设备分类Id
        /// </summary>
        public string EquipmentClass_Id { get; set; }
    }
}
