using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 订单表
    /// </summary>
    [Table(TableName = "sale_order", KeyName = "sale_order_id", Code = "sale_order_code", IsIdentity = false)]
    public class sale_order
    {

        /// <summary>
        /// 订单id
        /// </summary>
        public string sale_order_id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string sale_order_code { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string supplier_id { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string dept_id { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        public string sale_man { get; set; }

        /// <summary>
        /// 销售订单日期
        /// </summary>
        public DateTime sale_order_date { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string check_by { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime check_time { get; set; }

        /// <summary>
        /// 是否已审核(0代表未审,1代表已审)
        /// </summary>
        public int is_checked { get; set; }

        /// <summary>
        /// 是否已安排
        /// </summary>
        public int is_planed { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public int is_finish { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

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
