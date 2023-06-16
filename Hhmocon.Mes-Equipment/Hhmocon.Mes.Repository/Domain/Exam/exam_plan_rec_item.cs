using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 设备点检保养计划记录项目
    /// </summary>
    [Table(TableName = "exam_plan_rec_item", KeyName = "exam_plan_rec_item_id", IsIdentity = false)]
    public class exam_plan_rec_item
    {

        /// <summary>
        /// 项目ID
        /// </summary>
        public string exam_plan_rec_item_id { get; set; }

        /// <summary>
        /// 对应点检记录id
        /// </summary>
        public string exam_plan_rec_id { get; set; }

        /// <summary>
        /// 维保方案编号
        /// </summary>
        public string examschema_id { get; set; }

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
        /// 维保标准
        /// </summary>
        public string examitem_std { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string value_type { get; set; }

        /// <summary>
        /// 结果(1:合格 0：不合格)
        /// </summary>
        public int result { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int value { get; set; }

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
