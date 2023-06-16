using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 库存表
    /// </summary>
    [Table(TableName = "base_stock", KeyName = "stock_id", IsIdentity = false)]
    public class base_stock
    {

        /// <summary>
        /// 库存id
        /// </summary>
        public string stock_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string material_code { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public string warehouse_id { get; set; }

        /// <summary>
        /// 仓库code
        /// </summary>
        public string warehouse_code { get; set; }

        /// <summary>
        /// 库位id
        /// </summary>
        public string warehouse_loc_id { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string warehouse_loc_code { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string warehouse_loc_name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }

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
