using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 出入库类型表
    /// </summary>
    [Table(TableName = "base_iowarehouse_type", KeyName = "iowarehouse_type_id", Code = "iowarehouse_type_code", IsIdentity = false)]
    public class base_iowarehouse_type
    {

        /// <summary>
        /// 出入库类型id
        /// </summary>
        public string iowarehouse_type_id { get; set; }

        /// <summary>
        /// 出入库类型编码
        /// </summary>
        public string iowarehouse_type_code { get; set; }

        /// <summary>
        /// 出入库类型名称
        /// </summary>
        public string iowarehouse_type_name { get; set; }

        /// <summary>
        /// 出入库标志(1:出库  0：入库)
        /// </summary>
        public int io_type { get; set; }

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
