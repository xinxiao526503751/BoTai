using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 报工记录表
    /// </summary>
    [Table(TableName = "plan_work_rpt", KeyName = "plan_work_rpt_id", Code = "plan_work_rpt_code", IsIdentity = false)]
    public class plan_work_rpt
    {

        /// <summary>
        /// 报工记录ID
        /// </summary>
        public string plan_work_rpt_id { get; set; }

        /// <summary>
        /// 报工记录编码
        /// </summary>
        public string plan_work_rpt_code { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string plan_work_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }
        /// <summary>
        /// 加工数
        /// </summary>
        public int da_num { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public int reject_num { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 返工数
        /// </summary>
        public int return_num { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public int plan_num { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime report_date { get; set; }

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
