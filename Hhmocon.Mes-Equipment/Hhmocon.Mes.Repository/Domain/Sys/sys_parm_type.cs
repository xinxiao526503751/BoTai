using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 系统参数类型
    /// </summary>
    [Table(TableName = "sys_parm_type", KeyName = "parm_type_id", IsIdentity = false)]
    public class sys_parm_type
    {

        /// <summary>
        /// 类型ID
        /// </summary>
        public string parm_type_id { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string parm_type_name { get; set; }

        /// <summary>
        /// 类型英文名
        /// </summary>
        public string parm_type_enname { get; set; }

        /// <summary>
        /// 参数默认值,system
        /// </summary>
        public string parm_type_default { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 是否启用(1:启用  默认启用)
        /// </summary>
        public int is_enable { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int sort_no { get; set; }

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
