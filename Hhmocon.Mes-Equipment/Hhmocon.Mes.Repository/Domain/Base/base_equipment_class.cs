using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 设备分类表
    /// </summary>
    [Table(TableName = "base_equipment_class", KeyName = "equipment_class_id", Code = "equipment_class_code", IsIdentity = false)]
    public class base_equipment_class
    {

        /// <summary>
        /// 设备分类父id
        /// </summary>
        public string equipment_class_parentid { get; set; }

        /// <summary>
        /// 设备分类父code
        /// </summary>
        public string equipment_class_parentcode { get; set; }

        /// <summary>
        /// 设备分类父name
        /// </summary>
        public string equipment_class_parentname { get; set; }

        /// <summary>
        /// 设备分类ID
        /// </summary>
        public string equipment_class_id { get; set; }

        /// <summary>
        /// 设备分类编码
        /// </summary>
        public string equipment_class_code { get; set; }

        /// <summary>
        /// 设备分类名称
        /// </summary>
        public string equipment_class_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string process_equipment_class_id { get; set; }

        /// <summary>
        /// 设备分类类型
        /// </summary>
        public string equipment_class_type { get; set; }

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
    }
}
