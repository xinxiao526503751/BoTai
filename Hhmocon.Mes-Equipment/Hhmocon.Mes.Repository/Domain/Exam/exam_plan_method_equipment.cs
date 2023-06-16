using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 点检计划关联设备表
    /// </summary>
    [Table(TableName = "exam_plan_method_equipment", KeyName = "exam_plan_method_equipment_id", IsIdentity = false)]
    public class exam_plan_method_equipment
    {

        /// <summary>
        /// 规则id
        /// </summary>
        public string exam_plan_method_equipment_id { get; set; }

        /// <summary>
        /// 对应的计划ID
        /// </summary>
        public string exam_plan_method_id { get; set; }

        /// <summary>
        /// 对应的设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 对应的设备CODE
        /// </summary>
        public string equipment_code { get; set; }

        /// <summary>
        /// 对应的设备Name
        /// </summary>
        public string equipment_name { get; set; }

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
        /// "1"点检 "2"保养 "3"维修
        /// </summary>
        public string method_type { get; set; }
    }
}
