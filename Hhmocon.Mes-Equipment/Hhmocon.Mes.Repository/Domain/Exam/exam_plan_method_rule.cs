using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 点检计划时间方案
    /// </summary>
    [Table(TableName = "exam_plan_method_rule", KeyName = "exam_plan_method_rule_id", IsIdentity = false)]
    public class exam_plan_method_rule
    {

        /// <summary>
        /// 规则id
        /// </summary>
        public string exam_plan_method_rule_id { get; set; }

        /// <summary>
        /// 对应的计划ID
        /// </summary>
        public string exam_plan_method_id { get; set; }

        /// <summary>
        /// 规则类型（maint：保养；check：点检）
        /// </summary>
        public string rule_type { get; set; }

        /// <summary>
        /// 触发条件
        /// </summary>
        public string occur_condition { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 触发类型（day 日，week 周，month月，temporary指定日期）
        /// </summary>
        public string unit_mode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime trigger_time { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
