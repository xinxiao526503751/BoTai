using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface ISysRoleRightRepository
    {
        /// <summary>
        /// 根据角色id获取
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<sys_role_right> GetByRoleId(string roleId);
    }
}
