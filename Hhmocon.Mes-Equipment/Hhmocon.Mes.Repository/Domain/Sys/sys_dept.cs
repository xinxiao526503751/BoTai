using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 部门表
    /// </summary>
    [Table(TableName = "sys_dept", KeyName = "dept_id", Code = "dept_code", IsIdentity = false)]
    public class sys_dept
    {

        /// <summary>
        /// 部门ID
        /// </summary>
        public string dept_id { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string dept_code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string dept_name { get; set; }

        /// <summary>
        /// 上级部门编号
        /// </summary>
        public string parent_dept_id { get; set; }

        /// <summary>
        /// 地点编号
        /// </summary>
        public string location_id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string role_id { get; set; }

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
