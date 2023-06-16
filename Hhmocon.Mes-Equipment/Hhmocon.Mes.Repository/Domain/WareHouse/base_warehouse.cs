using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 仓库表
    /// </summary>
    [Table(TableName = "base_warehouse", KeyName = "warehouse_id", Code = "warehouse_code", IsIdentity = false)]
    public class base_warehouse
    {

        /// <summary>
        /// 仓库id
        /// </summary>
        public string warehouse_id { get; set; }

        /// <summary>
        /// 仓库类型id
        /// </summary>
        public string warehouse_type_id { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouse_code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string warehouse_name { get; set; }

        /// <summary>
        /// 地点ID
        /// </summary>
        public string location_id { get; set; }

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
