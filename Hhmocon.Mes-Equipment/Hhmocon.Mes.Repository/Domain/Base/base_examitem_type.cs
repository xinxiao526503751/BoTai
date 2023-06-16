using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 点检项目类型
    /// </summary>
    [Table(TableName = "base_examitem_type", Code = "examitem_type_code", KeyName = "examitem_type_id", IsIdentity = false)]
    public class base_examitem_type
    {

        /// <summary>
        /// 点检类型ID
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_id { get; set; }

        /// <summary>
        /// 点检类型编码
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_code { get; set; }

        /// <summary>
        /// 点检类型名称
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_name { get; set; }

        /// <summary>
        /// 上级点检项目类型ID
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_parentid { get; set; }


        /// <summary>
        /// 上级点检项目类型ID
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_parentname { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DefaultValue("")]
        public string description { get; set; }

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
        /// 修改人时间
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

        /// <summary>
        /// 1点检  2保养  3维修
        /// </summary>
        public string method_type { get; set; }
    }
}
