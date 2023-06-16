using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;

namespace hmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// bom表
    /// </summary>
    [Table(TableName = "base_bom", KeyName = "bom_id", Code = "bom_code", IsIdentity = false)]
    public class base_bom
    {

        /// <summary>
        /// bomID
        /// </summary>
        public string bom_id { get; set; }

        /// <summary>
        /// bom编码
        /// </summary>
        public string bom_code { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string bom_name { get; set; }

        /// <summary>
        /// 母件物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 上一级BOM编号
        /// </summary>
        public string parent_bom_id { get; set; }

        /// <summary>
        /// 上一级BOM名称
        /// </summary>
        public string parent_bom_name { get; set; }

        /// <summary>
        /// BOM备注
        /// </summary>
        public string bom_comment { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 状态（new:新建/audit:审核/stop:停用）
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 母件单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 成品率
        /// </summary>
        public int yield { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public int loss_rate { get; set; }

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
    public class base_bom_plus
    {
        public base_bom base_Bom { get; set; }
        public List<base_bom_detail> base_Bom_Details { get; set; }
    }
}
