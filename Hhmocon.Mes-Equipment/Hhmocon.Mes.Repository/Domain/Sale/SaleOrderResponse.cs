namespace Hhmocon.Mes.Repository.Domain
{
    public class SaleOrderResponse : sale_order
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string supplier_name { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string dept_name { get; set; }
    }
}
