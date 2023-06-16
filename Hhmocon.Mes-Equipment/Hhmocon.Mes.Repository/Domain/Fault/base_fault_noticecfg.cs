using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 事件通知配置表
    /// </summary>
    [Table(TableName = "base_fault_noticecfg", KeyName = "fault_noticecfg_id", IsIdentity = false)]
    public class base_fault_noticecfg
    {

        /// <summary>
        /// id
        /// </summary>
        public string fault_noticecfg_id { get; set; }

        /// <summary>
        /// 事件id
        /// </summary>
        public string fault_id { get; set; }

        /// <summary>
        /// 通知类型（微信：webchat；短信： sms ；邮件：email）
        /// </summary>
        public string notice_type { get; set; }

        /// <summary>
        /// 通知等级(0: 立即通知  1：一级 2：二级  3：三级)
        /// </summary>
        public int notice_level { get; set; }



        /// <summary>
        /// 被通知用户ID
        /// </summary>
        public string user_id { get; set; }

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
