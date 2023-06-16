using Hhmocon.Mes.Repository.AuthStrategies;
using Hhmocon.Mes.Util;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// SSO接口
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// 检验Token是否有效
        /// </summary>
        /// <param name="token">token值</param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        bool CheckLogin(string token = "", string otherInfo = "");

        /// <summary>
        /// 获取登录用户的授权策略
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        AuthStrategyContext GetCurrentUser(string otherInfo = "");

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        string GetUserName(string otherInfo = "");

        string GetUserAccount(string otherInfo = "");
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appkey">登录的应用appkey</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        LoginResult Login(string appkey, string username, string pwd);

        /// <summary>
        /// 手机登录接口
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        AndroidLoginResult AndroidLogin(string appkey, string username, string pwd);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        bool Logout();
    }
}
