using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.AuthStrategies;
using Hhmocon.Mes.Repository.LoginRelated;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace Hhmocon.Mes.WebApi.Controllers.CheckIn
{
    /// <summary>
    /// 登录接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Login", IgnoreApi = false)]
    [ApiController]

    public class CheckController : ControllerBase
    {
        private readonly IAuth _authUtil;

        private readonly ILogger<CheckController> _logger;

        private readonly AuthStrategyContext _authStrategyContext;

        public CheckController(IAuth authUtil, ILogger<CheckController> logger)//依赖注入
        {
            _authUtil = authUtil;
            _logger = logger;
            _authStrategyContext = _authUtil.GetCurrentUser();
        }

        /// <summary>
        /// 检查授权策略
        /// </summary>
        private void CheckContext()
        {
          
            if (_authStrategyContext == null) //发现问题 刷新为空 
            {
                throw new Exception("登录已过期");
            }
        }
        /// <summary>
        /// 检验token是否有效
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<bool> GetStatus()
        {
            Response<bool> result = new Response<bool>();
            try
            {
                result.Result = _authUtil.CheckLogin();//检查登录
            }
            catch (Exception ex)
            {
                result.Code = 50014;//TOKEN无效就返回状态码50014
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="request">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public LoginResult Login([FromBody] PassportLoginRequest request)//登录api  从页面post进PassportLoginRequest
        {
            _logger.LogInformation("Login enter");//往日志里写入记录
            LoginResult result = new LoginResult();
            try
            {
                result = _authUtil.Login(request.AppKey, request.Account, request.Password);    //检验appkey   和账号密码
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Logout()
        {
            Response<bool> resp = new Response<bool>();
            try
            {
                resp.Result = _authUtil.Logout();
            }
            catch (Exception e)
            {
                resp.Result = false;
                resp.Message = e.Message;
            }
            return resp;
        }

        /// <summary>
        /// 根据Token获取用户名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<string> GetUserName()
        {
            Response<string> result = new Response<string>();
            try
            {
                CheckContext();
                result.Result = _authStrategyContext.User.user_name;
            }
            catch (CommonException ex)
            {

                result.Code = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.InnerException != null
                    ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取权限菜单栏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<List<MenuTree>> GetRight()
        {
            Response<List<MenuTree>> result = new Response<List<MenuTree>>();
            try
            {
                CheckContext();

                result.Result = _authStrategyContext.Rights;
            }
            catch (CommonException ex)
            {

                result.Code = ex.Code;
                result.Message = ex.Message;

            }
            return result;
        }
    }


}
