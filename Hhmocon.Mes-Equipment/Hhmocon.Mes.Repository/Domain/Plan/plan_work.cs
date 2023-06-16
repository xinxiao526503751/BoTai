using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 计划工单表
    /// </summary>
    [Table(TableName = "plan_work", KeyName = "plan_work_id", Code = "plan_work_code", IsIdentity = false)]
    public class plan_work
    {

        /// <summary>
        /// 工单状态(0暂停,1加工,2调试,9完工,5换型调试)
        /// </summary>
        public int work_state { get; set; }

        /// <summary>
        /// 计划状态(0未下发， 1已下发)
        /// </summary>
        public int plan_state { get; set; }

        /// <summary>
        /// 紧急度(0普通，1较高，2最高)
        /// </summary>
        public int urgency { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int sort_num { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime state_start_time { get; set; }

        /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime state_end_time { get; set; }

        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime report_time { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public int plan_num { get; set; }

        /// <summary>
        /// 加工数
        /// </summary>
        public int da_num { get; set; }

        /// <summary>
        /// 返工数
        /// </summary>
        public int return_num { get; set; }

        /// <summary>
        /// 报工数
        /// </summary>
        public int report_num { get; set; }

        /// <summary>
        /// 调试数
        /// </summary>
        public int debug_num { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public int reject_num { get; set; }

        /// <summary>
        /// 最大允许加工数
        /// </summary>
        public int max_num { get; set; }

        /// <summary>
        /// 计划工单ID
        /// </summary>
        public string plan_work_id { get; set; }

        /// <summary>
        /// 计划工单编码
        /// </summary>
        public string plan_work_code { get; set; }

        /// <summary>
        /// 关联计划ID
        /// </summary>
        public string plan_process_id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 地点ID
        /// </summary>
        public string location_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string process_id { get; set; }

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
