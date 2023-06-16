/// <summary>
/// 用户登录接口
/// </summary>
namespace Hhmocon.Mes.Util
{

    /// <summary>
    /// 用户登录结果
    /// </summary>
    public class LoginResult : Response<string>
    {

        //登录后 返回的连接 
        public string ReturnUrl;
        //登录后 返回的Token
        public string Token;

    }


    /// <summary>
    /// 安卓用户登录后 返回的信息
    /// </summary>
    public class AndroidLoginResult : Response<string>
    {
        public string UserName;
        public string UserCode;

        public string UserId;

        public string UserReportPrcess;



        public AndroidLoginResult()
        {
            UserName = string.Empty;
            UserCode = string.Empty;
            UserId = string.Empty;
            UserReportPrcess = string.Empty;
        }

    }
}
