using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 事件定义表
    /// </summary>
    [Table(TableName = "base_fault", KeyName = "fault_id", Code = "fault_code", IsIdentity = false)]
    public class base_fault
    {

        /// <summary>
        /// id
        /// </summary>
        public string fault_id { get; set; }

        /// <summary>
        /// 事件编码
        /// </summary>
        public string fault_code { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string fault_name { get; set; }

        /// <summary>
        /// 所属事件类型ID
        /// </summary>
        public string fault_class_id { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
