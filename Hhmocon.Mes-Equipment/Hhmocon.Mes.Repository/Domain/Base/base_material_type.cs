using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    [Table(TableName = "base_material_type", Code = "material_type_code", KeyName = "material_type_id", IsIdentity = false)]
    public class base_material_type
    {
        /// <summary>
        /// 物料类型ID
        /// </summary>
        [DefaultValue("")]
        public string material_type_id { get; set; }

        /// <summary>
        /// 物料类型编码
        /// </summary>
        [DefaultValue("")]
        public string material_type_code { get; set; }

        /// <summary>
        /// 物料类型名称
        /// </summary>
        [DefaultValue("")]
        public string material_type_name { get; set; }

        /// <summary>
        /// 物料类型全名
        /// </summary>
        [DefaultValue("")]
        public string material_type_fullname { get; set; }

        /// <summary>
        /// 物料类型父名
        /// </summary>
        [DefaultValue("")]
        public string material_type_parentname { get; set; }

        /// <summary>
        /// 物料类型父id
        /// </summary>
        [DefaultValue("")]
        public string material_type_parentid { get; set; }

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
        [DefaultValue("")]
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DefaultValue("")]
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [DefaultValue("")]
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [DefaultValue("")]
        public string modified_by_name { get; set; }
    }
}
