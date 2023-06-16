using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 工艺路线规则物料需求
    /// </summary>
    [Table(TableName = "base_process_route_material", KeyName = "process_route_material_id", IsIdentity = false)]
    public class base_process_route_material
    {

        /// <summary>
        /// 工艺路线所需物料ID
        /// </summary>
        public string process_route_material_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string process_route_id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// bom编号
        /// </summary>
        public string bom_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 是否是主
        /// </summary>
        public int is_main { get; set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public string arg_1 { get; set; }

        /// <summary>
        /// 参数2
        /// </summary>
        public string arg_2 { get; set; }

        /// <summary>
        /// 参数3
        /// </summary>
        public string arg_3 { get; set; }

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
