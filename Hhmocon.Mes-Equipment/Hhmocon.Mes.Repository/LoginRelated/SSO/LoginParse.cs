using Hhmocon.Mes.Cache;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Net;

namespace Hhmocon.Mes.Repository.LoginRelated.SSO
{
    /// <summary>
    /// 登录分析类
    /// </summary>
    public class LoginParse : IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ICacheContext _cacheContext;
        public LoginParse(PikachuRepository pikachuRepository, ICacheContext cacheContext)
        {
            _pikachuRepository = pikachuRepository;
            _cacheContext = cacheContext;
        }

        /// <summary>
        /// 登录分析
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LoginResult Do(PassportLoginRequest model)
        {
            LoginResult result = new LoginResult();
            try
            {
                model.Trim();
                ////获取应用信息
                //AppInfo appInfo = _appInfoService.Get(model.AppKey);
                //if (appInfo == null)
                //{
                //    throw new Exception("应用不存在");
                //}
                sys_user userInfo;
                if (model.Account == "System")//如果登录的是系统管理员
                {
                    userInfo = new sys_user
                    {
                        user_id = Guid.Empty.ToString(),
                        user_name = "System",
                        user_cn_name = "超级管理员",
                        user_passwd = "123456".ToMD5()
                    };
                }
                else
                {
                    userInfo = _pikachuRepository.GetByName<sys_user>(model.Account);
                }
                if (userInfo == null)//判断表里是否有该用户
                {
                    throw new Exception("用户不存在");
                }
                if (userInfo.user_passwd != model.Password.ToMD5())//检验密码是否正确
                {
                    throw new Exception("密码错误");
                }

                TokenModelJWT tokenModel = new TokenModelJWT//新建token
                {
                    UId = userInfo.user_id,
                    Account = userInfo.user_name
                };

                string jwtStr = JwtHelper.IssueJWT(tokenModel);//转JWT字符串

                UserAuthSession currentSession = new UserAuthSession//新建Session
                {
                    Account = model.Account,
                    Name = userInfo.user_cn_name,
                    Token = jwtStr,
                    AppKey = model.AppKey,
                    CreateTime = DateTime.Now
                    //IpAddress=HttpContext.Current.Request.UserHostAddress                   
                };

                //创建Session
                _cacheContext.Set(currentSession.Account, currentSession, DateTime.Now.AddDays(1));//写session到缓存
                result.Code = 200;
                //result.ReturnUrl = appInfo.ReturnUrl;
                result.Token = currentSession.Token;
            }
            catch (Exception ex)
            {
                result.Code = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }

            return result;
        }



        public AndroidLoginResult AndroidLoginDo(PassportLoginRequest model)
        {
            AndroidLoginResult result = new AndroidLoginResult();
            try
            {
                model.Trim();

                sys_user userInfo;

                if (model.Account == "System")//如果登录的是系统管理员
                {
                    userInfo = new sys_user
                    {
                        user_id = Guid.Empty.ToString(),
                        user_name = "System",
                        user_cn_name = "超级管理员",
                        user_passwd = "123456".ToMD5(),
                        user_report_process = "9"
                    };
                }
                else
                {
                    userInfo = _pikachuRepository.GetByName<sys_user>(model.Account);
                }
                if (userInfo == null)//判断表里是否有该用户
                {
                    throw new Exception("用户不存在");
                }
                if (userInfo.user_passwd != model.Password.ToMD5())//检验密码是否正确
                {
                    throw new Exception("密码错误");
                }

                result.UserCode = userInfo.user_name;
                result.UserName = userInfo.user_cn_name;
                result.UserId = userInfo.user_id;
                if (string.IsNullOrEmpty(userInfo.user_report_process))
                {
                    result.UserReportPrcess = "0";
                }
                else
                {
                    result.UserReportPrcess = userInfo.user_report_process;
                }

                result.Code = 200;


                //TokenModelJWT tokenModel = new TokenModelJWT//新建token
                //{
                //    UId = userInfo.user_id,
                //    Account = userInfo.user_name
                //};

                //string jwtStr = JwtHelper.IssueJWT(tokenModel);//转JWT字符串

                //UserAuthSession currentSession = new UserAuthSession//新建Session
                //{
                //    Account = model.Account,
                //    Name = userInfo.user_cn_name,
                //    Token = jwtStr,
                //    AppKey = model.AppKey,
                //    CreateTime = DateTime.Now
                //    //IpAddress=HttpContext.Current.Request.UserHostAddress                   
                //};

                ////创建Session
                //_cacheContext.Set(currentSession.Account, currentSession, DateTime.Now.AddDays(1));//写session到缓存

                //result.ReturnUrl = appInfo.ReturnUrl;
                //result.Token = currentSession.Token;
            }
            catch (Exception ex)
            {
                result.Code = (int)HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
