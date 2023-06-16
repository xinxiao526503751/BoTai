using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 子设备定义
    /// </summary>
    [Table(TableName = "base_equipment_sub", KeyName = "id", IsIdentity = false)]
    public class base_equipment_sub
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 子设备ID
        /// </summary>
        public string equipment_sub_id { get; set; }

        /// <summary>
        /// 子设备编码
        /// </summary>
        public string equipment_code { get; set; }

        /// <summary>
        /// 子设备编名称
        /// </summary>
        public string equipment_name { get; set; }

        /// <summary>
        /// 子设备类型
        /// </summary>
        public string equipment_sub_type_id { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public string process_id { get; set; }

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


        /// <summary>
        /// 子设备名
        /// </summary>
        public string equipment_sub_name { get; set; }

        /// <summary>
        /// 子设备工序名
        /// </summary>
        public string equipment_sub_process { get; set; }

        /// <summary>
        /// 子设备工序Id
        /// </summary>
        public string equipment_sub_process_id { get; set; }
    }
}
