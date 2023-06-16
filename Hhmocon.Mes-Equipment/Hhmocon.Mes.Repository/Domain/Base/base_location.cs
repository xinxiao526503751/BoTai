using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 地点表
    /// </summary>
    [Table(TableName = "base_location", Code = "location_code", KeyName = "location_id", IsIdentity = false)]
    public class base_location
    {
        /// <summary>
        /// 地点ID
        /// </summary>
        [DefaultValue("")]
        public string location_id { get; set; }

        /// <summary>
        /// 地点类型ID
        /// </summary>
        [DefaultValue("")]
        public string location_type_id { get; set; }

        /// <summary>
        /// 地点类型名称
        /// </summary>
        [DefaultValue("")]
        public string location_type_name { get; set; }

        /// <summary>
        /// 地点编码
        /// </summary>
        [DefaultValue("")]
        public string location_code { get; set; }

        /// <summary>
        /// 地点名称
        /// </summary>
        [DefaultValue("")]
        public string location_name { get; set; }

        /// <summary>
        /// 地点全名
        /// </summary>
        [DefaultValue("")]
        public string location_fullname { get; set; }

        /// <summary>
        /// 父级地点id
        /// </summary>
        [DefaultValue("")]
        public string location_parentid { get; set; }

        /// <summary>
        /// 父级地点name
        /// </summary>
        public string location_parentname { get; set; }


        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 停用时间
        /// </summary>
        public DateTime end_time { get; set; }

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
