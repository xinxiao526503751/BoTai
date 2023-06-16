using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// </summary>
    /// <summary>
    /// 系统参数
    /// </summary>
    [Table(TableName = "sys_parm", KeyName = "parm_id", Code = "parm_code", IsIdentity = false)]
    public class sys_parm
    {

        /// <summary>
        /// 参数ID
        /// </summary>
        public string parm_id { get; set; }

        /// <summary>
        /// 参数类型id
        /// </summary>
        public string parm_type_id { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string parm_code { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string parm_name { get; set; }

        /// <summary>
        /// 参数英文名
        /// </summary>
        public string parm_enname { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string parm_value { get; set; }

        /// <summary>
        /// 上级参数ID
        /// </summary>
        public string parent_parm_id { get; set; }

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
