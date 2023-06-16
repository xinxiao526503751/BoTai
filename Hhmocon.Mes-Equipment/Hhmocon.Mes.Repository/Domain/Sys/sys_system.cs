using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 系统表
    /// </summary>
    [Table(TableName = "sys_system", KeyName = "sys_id", IsIdentity = false)]
    public class sys_system
    {

        /// <summary>
        /// 系统ID
        /// </summary>
        public string sys_id { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string sys_name { get; set; }

        /// <summary>
        /// 上级系统名称
        /// </summary>
        public string parent_sys_id { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public string sys_ishide { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string sys_sort { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string sys_url { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string sys_pic { get; set; }

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
