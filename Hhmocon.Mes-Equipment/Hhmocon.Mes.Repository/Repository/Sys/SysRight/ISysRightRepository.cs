using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface ISysRightRepository
    {
        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_right> GetByName(string name);

        /// <summary>
        /// 根据上级类型Id获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_right> GetByParentRightId(string name);

    }
}
