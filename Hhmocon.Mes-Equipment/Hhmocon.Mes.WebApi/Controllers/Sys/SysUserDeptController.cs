using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 用户部门接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysUserDeptController
    {
        private readonly SysUserDeptApp _app;
        private readonly PikachuApp _picachuApp;

        /// <summary>
        /// 用户部门控制器构造函数
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public SysUserDeptController(SysUserDeptApp app, PikachuApp picachuApp)
        {
            _app = app;
            _picachuApp = picachuApp;
        }
    }
}
