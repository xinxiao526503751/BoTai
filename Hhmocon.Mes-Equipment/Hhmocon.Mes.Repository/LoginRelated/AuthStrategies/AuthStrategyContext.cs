using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.AuthStrategies
{
    /// <summary>
    /// 授权策略上下文，一个典型的策略模式
    /// </summary>
    public class AuthStrategyContext
    {
        private readonly IAuthStrategy _strategy;

        /// <summary>
        /// ctor构造函数
        /// </summary>
        /// <param name="strategy"></param>
        public AuthStrategyContext(IAuthStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        public sys_user User => _strategy.User;

        /// <summary>
        /// 角色
        /// </summary>
        public List<sys_role> Roles => _strategy.Roles;

        /// <summary>
        /// 权限菜单
        /// </summary>
        public List<MenuTree> Rights => _strategy.Rights;

        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public List<KeyDescription> GetProperties(string moduleCode)
        {
            return _strategy.GetProperties(moduleCode);
        }
    }
}
