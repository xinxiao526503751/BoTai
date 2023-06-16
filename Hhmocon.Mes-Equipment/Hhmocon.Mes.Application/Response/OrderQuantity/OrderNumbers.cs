
namespace Hhmocon.Mes.Application.Response
{

    /// <summary>
    /// 各个订单加工状态的数量
    /// </summary>
    public class OrderNumbers
    {
        /// <summary>
        /// 订单总数
        /// </summary>
        public int TotalOrderNumber { get; set; }

        /// <summary>
        /// 已完成订单数量
        /// </summary>
        public int CompleteOrderNumber { get; set; }

        /// <summary>
        /// 未完成订单数量
        /// </summary>
        public int IncompleteOrderNumber { get; set; }
    }
}
