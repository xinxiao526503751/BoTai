using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 不合格品登记表
    /// </summary>
    [Table(TableName = "base_defective_product", KeyName = "defective_product_id", IsIdentity = false)]
    public class base_defective_product
    {

        /// <summary>
        /// 不合格品ID
        /// </summary>
        public string defective_product_id { get; set; }

        /// <summary>
        /// 不合格品类型ID
        /// </summary>
        public string defective_type_id { get; set; }

        /// <summary>
        /// 不合格品原因ID
        /// </summary>
        public string defective_reason_id { get; set; }

        /// <summary>
        /// 不合格品原因名称
        /// </summary>
        public string defective_reason_name { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// 工序NAME
        /// </summary>
        public string process_name { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int defective_num { get; set; }

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

        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime register_time { get; set; }
    }
}
