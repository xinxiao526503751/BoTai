using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hhmocon.Mes.WebApi.Controllers.SaleOrder
{
    /// <summary>
    /// 订单管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "SaleOrder", IgnoreApi = false)]
    [ApiController]
    public class SaleOrderManagerController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly SaleOrderApp _saleOrderApp;
        private readonly SaleOrderDetailApp _saleOrderDetailApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="picachuApp"></param>
        /// <param name="saleOrderApp"></param>
        public SaleOrderManagerController(PikachuApp picachuApp, SaleOrderApp saleOrderApp, SaleOrderDetailApp saleOrderDetailApp)
        {
            _pikachuApp = picachuApp;
            _saleOrderApp = saleOrderApp;
            _saleOrderDetailApp = saleOrderDetailApp;
        }


    }
}
