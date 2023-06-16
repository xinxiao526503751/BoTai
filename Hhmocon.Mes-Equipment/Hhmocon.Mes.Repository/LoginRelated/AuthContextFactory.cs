using Hhmocon.Mes.Repository.AuthStrategies;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.LoginRelated.AuthStrategies;
using Hhmocon.Mes.Util.AutofacManager;
using System.Linq;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// 加载用户所有可访问的资源/机构/模块
    /// </summary>
    public class AuthContextFactory : IDependency
    {
        private readonly SystemAuthStrategy _systemAuth;

        private readonly NormalAuthStrateg _normalAuthStrategy;
        private readonly PikachuRepository _pikachuRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="sysStrategy"></param>
        /// <param name="normalAuthStrategy"></param>
        /// <param name="pikachuRepository"></param>
        public AuthContextFactory(SystemAuthStrategy sysStrategy
            , NormalAuthStrateg normalAuthStrategy
            , PikachuRepository pikachuRepository
           )
        {
            _systemAuth = sysStrategy;
            _normalAuthStrategy = normalAuthStrategy;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 获取当前登录用户的授权策略，区分system管理员和normal用户，获取不同的授权策略
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public AuthStrategyContext GetAuthStrategyContext(string username)
        {
            IAuthStrategy service;
            if (username == "System")
            {
                service = _systemAuth;//采用超级管理员策略
            }
            else
            {
                service = _normalAuthStrategy;//采用普通用户策略
                service.User = _pikachuRepository.GetAll<sys_user>()
                    .Where(u => u.user_name.Equals(username)).FirstOrDefault();//设置用户的信息给策略
            }
            return new AuthStrategyContext(service);
        }
    }
}
