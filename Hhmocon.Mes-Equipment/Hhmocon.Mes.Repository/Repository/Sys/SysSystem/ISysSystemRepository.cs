using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 系统仓储接口
    /// </summary>
    public interface ISysSystemRepository
    {
        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_system> GetByName(string name);

        public List<sys_right> GetBySysId(string SysId);

    }
}
