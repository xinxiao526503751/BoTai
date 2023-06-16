using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 事件记录流程表
    /// </summary>
    [Table(TableName = "fault_record_flow", KeyName = "fault_record_flow_id", IsIdentity = false)]
    public class fault_record_flow
    {

        /// <summary>
        /// 事件记录流程id
        /// </summary>
        public string fault_record_flow_id { get; set; }

        /// <summary>
        /// 事件记录id
        /// </summary>
        public string fault_record_id { get; set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        public string fault_id { get; set; }

        /// <summary>
        /// 流程顺序 1确认 2处理 3关闭
        /// </summary>
        public int flow_seq { get; set; }

        /// <summary>
        /// 流程状态:0产生，1结束
        /// </summary>
        public int flow_status { get; set; }

        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime flow_start_time { get; set; }

        /// <summary>
        /// 流程结束时间
        /// </summary>
        public string flow_end_time { get; set; }

        /// <summary>
        /// 报告内容
        /// </summary>
        public string flow_info { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public int is_finish { get; set; }

        /// <summary>
        /// 流程持续时长(秒)
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        /// 流程刷卡人员
        /// </summary>
        public string flow_user_id { get; set; }

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
        /// 通知是否完成的标志位
        /// </summary>
        public int notice_flag { get; set; }
    }
}
