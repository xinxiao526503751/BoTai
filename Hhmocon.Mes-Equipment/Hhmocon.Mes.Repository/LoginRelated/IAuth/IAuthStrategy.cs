using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 授权策略接口
    /// </summary>
    public interface IAuthStrategy
    {

        /// <summary>
        /// 权限
        /// </summary>
        List<MenuTree> Rights { get; }

        /// <summary>
        /// 角色
        /// </summary>
        List<sys_role> Roles { get; }

        /// <summary>
        /// 用户
        /// </summary>
        sys_user User
        {
            get; set;
        }
        /// <summary>
        /// 根据模块Code获取可访问的模块字段
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        List<KeyDescription> GetProperties(string moduleCode);
    }

    public class KeyDescription
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 键的描述
        /// </summary>
        public string Description { get; set; }
    }
}

