using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 事件类型表
    /// </summary>
    [Table(TableName = "base_fault_class", KeyName = "fault_class_id", Code = "fault_class_code", IsIdentity = false)]
    public class base_fault_class
    {

        /// <summary>
        /// id
        /// </summary>
        public string fault_class_id { get; set; }

        /// <summary>
        /// 事件类型编码
        /// </summary>
        public string fault_class_code { get; set; }

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string fault_class_name { get; set; }

        /// <summary>
        /// 事件类型全称
        /// </summary>
        public string fault_class_fullname { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string fault_class_memo { get; set; }

        /// <summary>
        /// 默认系统保留事件，界面上不允许删除(为1，不允许删除，其余可删除)
        /// </summary>
        public int is_sys { get; set; }

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

        /// <summary>
        /// 异常类型父类id
        /// </summary>
        public string fault_class_parentid { get; set; }

        /// <summary>
        /// 异常类型父类
        /// </summary>
        public string fault_class_parentname { get; set; }
    }
}
