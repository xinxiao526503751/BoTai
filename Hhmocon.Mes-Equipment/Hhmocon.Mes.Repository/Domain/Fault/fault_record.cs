using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 事件记录表
    /// </summary>
    [Table(TableName = "fault_record", KeyName = "fault_record_id", IsIdentity = false)]
    public class fault_record
    {
        /// <summary>
        /// 事件记录id
        /// </summary>
        public string fault_record_id { get; set; }

        /// <summary>
        /// 事件id
        /// </summary>
        public string fault_id { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 报告内容
        /// </summary>
        public string record_info { get; set; }

        /// <summary>
        /// 持续时长(秒)
        /// </summary>
        public int fault_duration { get; set; }

        /// <summary>
        /// 是否已完成(0未完成)
        /// </summary>
        public int is_finish { get; set; }

        /// <summary>
        /// 事件状态(0:)
        /// </summary>
        public int notice_status { get; set; }

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
