using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 工序表
    /// </summary>
    [Table(TableName = "base_process", Code = "process_code", KeyName = "process_id", IsIdentity = false)]
    public class base_process
    {

        /// <summary>
        /// 工序ID
        /// </summary>
        [DefaultValue("")]
        public string process_id { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [DefaultValue("")]
        public string process_code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [DefaultValue("")]
        public string process_name { get; set; }

        /// <summary>
        /// 工序全名
        /// </summary>
        [DefaultValue("")]
        public string process_fullname { get; set; }

        /// <summary>
        /// 工序类型ID
        /// </summary>
        [DefaultValue("")]
        public string process_type_id { get; set; }

        /// <summary>
        /// 工序类型名称
        /// </summary>
        [DefaultValue("")]
        public string process_type_name { get; set; }

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
    }
}
