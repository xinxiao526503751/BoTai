using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 用户角色接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysUserRoleController
    {
        private readonly SysUserRoleApp _app;
        private readonly PikachuApp _picachuApp;

        /// <summary>
        /// 用户角色控制器构造函数
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public SysUserRoleController(SysUserRoleApp app, PikachuApp picachuApp)
        {
            _app = app;
            _picachuApp = picachuApp;
        }


        /// <summary>
        /// 根据用户Id在关联表获取用户的角色
        /// </summary>
        [HttpPost]
        public Response<List<sys_role>> GetRolesByUserId(string user_id)
        {
            Response<List<sys_role>> result = new Response<List<sys_role>>();
            try
            {
                result.Result = _app.GetRolesByUserId(user_id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
    }
}

