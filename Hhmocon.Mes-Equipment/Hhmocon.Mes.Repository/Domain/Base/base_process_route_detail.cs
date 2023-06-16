using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 工艺路线规则明细表
    /// </summary>
    [Table(TableName = "base_process_route_detail", KeyName = "process_route_detail_id", IsIdentity = false)]
    public class base_process_route_detail
    {
        /// <summary>
        /// 工艺路线工序明细ID
        /// </summary>
        public string process_route_detail_id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string process_route_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int process_seq { get; set; }

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
