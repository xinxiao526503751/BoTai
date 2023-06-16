using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 订单明细表(物料订单表)
    /// </summary>
    [Table(TableName = "sale_order_detail", KeyName = "sale_order_detail_id", Code = "sale_order_detail_code", IsIdentity = false)]
    public class sale_order_detail
    {
        /// <summary>
        /// 订单明细id
        /// </summary>
        public string sale_order_detail_id { get; set; }

        /// <summary>
        /// 物料订单编号
        /// </summary>
        public string sale_order_detail_code { get; set; }

        /// <summary>
        /// 订单id(关联订单表)
        /// </summary>
        public string sale_order_id { get; set; }

        /// <summary>
        /// 交付日期
        /// </summary>
        public DateTime delivery_date { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 工价
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public int totalmny { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public int out_qty { get; set; }

        /// <summary>
        /// 小包装数
        /// </summary>
        public int spack_qty { get; set; }

        /// <summary>
        /// 大包装数
        /// </summary>
        public int bpack_qty { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string barcode { get; set; }

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
    public class sale_order_detail_rep : sale_order_detail
    {
        /// <summary>
        /// 物料name
        /// </summary>
        public string material_name { get; set; }
    }
}
