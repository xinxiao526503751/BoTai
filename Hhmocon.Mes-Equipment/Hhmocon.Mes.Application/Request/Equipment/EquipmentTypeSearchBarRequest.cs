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
    public class EquipmentTypeSearchBarRequest
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
        /// equipmentType_Id
        /// </summary>
        public string EquipmentType_Id { get; set; }

        /// <summary>
        /// 设备类型name
        /// </summary>
        public string Type_Name { get; set; }

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string Type_Code { get; set; }
    }
}
