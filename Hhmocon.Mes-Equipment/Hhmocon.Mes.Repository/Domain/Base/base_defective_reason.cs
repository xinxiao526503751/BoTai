using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 不合格品原因
    /// </summary>
    [Table(TableName = "base_defective_reason", KeyName = "defective_reason_id", Code = "defective_reason_code", IsIdentity = false)]
    public class base_defective_reason
    {

        /// <summary>
        /// 不合格品原因ID
        /// </summary>
        public string defective_reason_id { get; set; }

        /// <summary>
        /// 不合格品原因编码
        /// </summary>
        public string defective_reason_code { get; set; }

        /// <summary>
        /// 不合格品原因名称
        /// </summary>
        public string defective_reason_name { get; set; }

        /// <summary>
        /// 不合格品类型ID
        /// </summary>
        public string defective_type_id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
