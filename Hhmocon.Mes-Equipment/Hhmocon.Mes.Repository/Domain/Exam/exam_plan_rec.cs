using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 设备点检保养计划记录
    /// </summary>
    [Table(TableName="exam_plan_rec",KeyName= "exam_plan_rec_id", IsIdentity = false)]
    public class exam_plan_rec
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public string exam_plan_rec_id { get; set; }

        /// <summary>
        /// 时间方案id
        /// </summary>
        public string exam_plan_method_rule_id { get; set; }
        
        /// <summary>
        /// 对应计划编码
        /// </summary>
        public string exam_plan_code { get; set; }

        /// <summary>
        /// 对应计划名称
        /// </summary>
        public string exam_plan_name { get; set; }

        /// <summary>
        /// 点检方式编号
        /// </summary>
        public string exam_method_id { get; set; }

        /// <summary>
        /// 点检方案编号
        /// </summary>
        public string exam_schema_id { get; set; }

        /// <summary>
        /// 维保方式类型（1：点检；2：保养 3：维修）
        /// </summary>
        public string exam_method_type { get; set; }

        /// <summary>
        /// 是否停机
        /// </summary>
        public int is_stop_machine { get; set; }

        /// <summary>
        /// 日期类型（year：年度计划；month：月底计划；day：日计划）
        /// </summary>
        public string calendar_type { get; set; }

        /// <summary>
        /// 是否手动（默认0：否）
        /// </summary>
        public int is_manual { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string plan_level { get; set; }

        /// <summary>
        /// 点检方案
        /// </summary>
        public string exam_schema_name { get; set; }

        /// <summary>
        /// 点检方式
        /// </summary>
        public string exam_method_name { get; set; }

        /// <summary>
        /// 提前期
        /// </summary>
        public int lead_time { get; set; }

        /// <summary>
        /// 提前期单位
        /// </summary>
        public string lead_time_unit { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime effect_start_time { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime effect_end_time { get; set; }

        /// <summary>
        /// 状态（new：新建；done：完成）
        /// </summary>
        public string status { get; set; }

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

        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 设备NAME
        /// </summary>
        public string equipment_name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string trigger_time { get; set; }

        /// <summary>
        /// 对应点检计划ID
        /// </summary>
        public string exam_plan_method_id { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public int result { get; set; }
    }
}
