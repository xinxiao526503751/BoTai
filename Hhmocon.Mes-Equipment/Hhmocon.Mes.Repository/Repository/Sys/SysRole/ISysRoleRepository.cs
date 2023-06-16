using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface ISysRoleRepository
    {
        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_role> GetByName(string name);
    }
}
