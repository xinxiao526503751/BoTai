using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 角色表
    /// </summary>
    [Table(TableName = "sys_role", KeyName = "role_id", IsIdentity = false)]
    public class sys_role
    {

        /// <summary>
        /// 角色ID
        /// </summary>
        public string role_id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string role_name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string role_desc { get; set; }

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
