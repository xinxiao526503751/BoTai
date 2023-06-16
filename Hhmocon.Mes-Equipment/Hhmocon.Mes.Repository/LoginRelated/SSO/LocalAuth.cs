using Hhmocon.Mes.Cache;
using Hhmocon.Mes.Repository.AuthStrategies;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.AspNetCore.Http;
using System;

namespace Hhmocon.Mes.Repository.LoginRelated.SSO
{
    /// <summary>
    /// 使用本地登录，这个注入IAuth时，只需要OpenAuth.MVC一个项目即可，无需WebApi的支持
    /// </summary>
    public class LocalAuth : IAuth, IDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthContextFactory _app;

        private readonly LoginParse _loginParse;

        private readonly ICacheContext _cacheContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="app"></param>
        /// <param name="loginParse"></param>
        /// <param name="cacheContext"></param>
        public LocalAuth(IHttpContextAccessor httpContextAccessor
            , AuthContextFactory app
            , LoginParse loginParse
            , ICacheContext cacheContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _app = app;
            _loginParse = loginParse;
            _cacheContext = cacheContext;

        }

        /// <summary>
        /// 获取令牌中的账号
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            //获取当前query请求中key值为Authorization的Token
            string token = _httpContextAccessor.HttpContext.Request.Query["Authorization"];
            //请求中有token就返回
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            //获取当前Headers中key值为Authorization的Token
            token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //拿到token就返回
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            //如果上面两种情况都拿不到token，就拿cookie
            string cookie = _httpContextAccessor.HttpContext.Request.Cookies["Authorization"];
            return cookie ?? string.Empty;

        }

        /// <summary>
        /// 校验是否登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        public bool CheckLogin(string token = "", string otherInfo = "")
        {
            //如果没有token，就  从Query 、 header、 cookie里面找  token
            if (string.IsNullOrEmpty(token))
            {
                token = GetToken();
            }
            //query、header、cookie里面都找不到，登录校验失败
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            //找到token了，判断token的格式是不是jwt字符串
            if (!JwtHelper.CanReadToken(token))
            {
                return false;
            }
            bool result = _cacheContext.Get<UserAuthSession>(JwtHelper.SerializeJWT(token).Account)?.Token == token;
            return result;
        }

        /// <summary>
        /// 获取当前登录的用户信息
        /// <para>通过URL中的Token参数或Cookie中的Token</para>>
        /// </summary>
        /// <param name="otherInfo">The otherInfo</param>
        /// <returns>LoginUserVM.</returns>
        public AuthStrategyContext GetCurrentUser(string otherInfo = "")
        {
            AuthStrategyContext context = null;
            //如果没有token，不允许获取信息
            if (!CheckLogin())
            {
                return null;
            }

            string token = GetToken();
            //获取user
            UserAuthSession user = _cacheContext.Get<UserAuthSession>(JwtHelper.SerializeJWT(token).Account);
            if (user != null)
            {
                context = _app.GetAuthStrategyContext(user.Account);
            }
            return context;
        }

        /// <summary>
        /// 获取当前登录的用户名
        /// <para>通过URL中的Token参数或Cookie中的Token</para>
        /// </summary>
        /// <param name="otherInfo">The otherInfo</param>
        /// <returns>System.String</returns>
        public string GetUserAccount(string otherInfo = "")
        {
            UserAuthSession user = _cacheContext.Get<UserAuthSession>(JwtHelper.SerializeJWT(GetToken()).Account);
            return user != null ? user.Account : "";
        }
        public string GetUserName(string otherInfo = "")
        {
            UserAuthSession user = _cacheContext.Get<UserAuthSession>(JwtHelper.SerializeJWT(GetToken()).Account);
            return user != null ? user.Name : "";
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appkey">应用程序Key</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>system.String</returns>
        public LoginResult Login(string appkey, string username, string pwd)
        {
            return _loginParse.Do(new PassportLoginRequest//对登录的信息进行分析判断
            {
                AppKey = appkey,
                Account = username,
                Password = pwd
            });
        }



        /// <summary>
        /// 手机登录接口
        /// </summary>
        /// <param name="appkey">应用程序Key</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>system.String</returns>
        public AndroidLoginResult AndroidLogin(string appkey, string username, string pwd)
        {
            return _loginParse.AndroidLoginDo(new PassportLoginRequest//对登录的信息进行分析判断
            {
                AppKey = appkey,
                Account = username,
                Password = pwd
            });

        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            string tokenAccount = JwtHelper.SerializeJWT(GetToken()).Account;//解析token获取用户
            if (string.IsNullOrEmpty(tokenAccount))
            {
                return true;
            }

            try
            {
                _cacheContext.Remove(tokenAccount);//移除缓存
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
