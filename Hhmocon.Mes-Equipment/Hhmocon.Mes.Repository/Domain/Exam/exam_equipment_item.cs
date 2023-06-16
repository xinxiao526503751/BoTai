using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 设备点检项目表
    /// </summary>
    [Table(TableName = "exam_equipment_item", KeyName = "exam_equipment_item_id", IsIdentity = false)]
    public class exam_equipment_item
    {

        /// <summary>
        ///  项目说明
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 项目标准
        /// </summary>
        public string examitem_std { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string value_type { get; set; }

        /// <summary>
        /// "1"点检 "2"保养 "3"维修
        /// </summary>
        public string method_type { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string exam_equipment_item_id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 点检项目ID
        /// </summary>
        public string examitem_id { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string examitem_code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string examitem_name { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string default_value { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public string max_value { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public string min_value { get; set; }

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

    /// <summary>
    /// 设备加点检项
    /// </summary>
    public class eqp_exe
    {
        public string equipment_id { get; set; }

        public string[] examitem_ids { get; set; }

        /// <summary>
        /// 1点检 2保养 3维修
        /// </summary>
        public string method_type { get; set; }
    }
}
