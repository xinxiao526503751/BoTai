using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// bom明细表
    /// </summary>
    [Table(TableName = "base_bom_detail", KeyName = "bom_detail_id", IsIdentity = false)]
    public class base_bom_detail
    {

        /// <summary>
        /// bom明细ID
        /// </summary>
        public string bom_detail_id { get; set; }

        /// <summary>
        /// bomID
        /// </summary>
        public string bom_id { get; set; }

        /// <summary>
        /// 子物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime start_date { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime end_date { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public int loss_rate { get; set; }

        /// <summary>
        /// 子件单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int bom_seq { get; set; }

        /// <summary>
        /// 分子用量
        /// </summary>
        public int quantity_nume { get; set; }

        /// <summary>
        /// 分母用量
        /// </summary>
        public int quantity_deno { get; set; }

        /// <summary>
        /// 偏置期
        /// </summary>
        public int offset_time { get; set; }

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
