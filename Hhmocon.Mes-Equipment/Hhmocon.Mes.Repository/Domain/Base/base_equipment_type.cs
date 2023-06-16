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

using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 设备类型表
    /// </summary>
    [Table(TableName = "base_equipment_type", KeyName = "equipment_type_id", Code = "equipment_type_code", IsIdentity = false)]
    public class base_equipment_type
    {
        /// <summary>
        /// 地点类型ID
        /// </summary>
        public string equipment_type_id { get; set; }

        /// <summary>
        /// 地点类型编码
        /// </summary>
        public string equipment_type_code { get; set; }

        /// <summary>
        /// 地点类型名称
        /// </summary>
        public string equipment_type_name { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modified_by_name { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public string equipment_type_parentid { get; set; }

        /// <summary>
        /// 父name
        /// </summary>
        public string equipment_type_parentname { get; set; }

        /// <summary>
        /// 父code
        /// </summary>
        public string equipment_type_parentcode { get; set; }
    }
}
