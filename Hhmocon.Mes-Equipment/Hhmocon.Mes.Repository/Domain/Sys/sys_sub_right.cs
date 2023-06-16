using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 权限子表
    /// </summary>
    [Table(TableName = "sys_sub_right", KeyName = "sub_right_id", IsIdentity = false)]
    public class sys_sub_right
    {

        /// <summary>
        /// 上级权限Id
        /// </summary>
        public string right_id { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 权限ID（指向该子权限的归属ID）
        /// </summary>
        public string sub_right_id { get; set; }

        /// <summary>
        /// 子权限名称
        /// </summary>
        public string sub_right_name { get; set; }

        /// <summary>
        /// 子权限编码
        /// </summary>
        public string sub_right_code { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

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
