using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface ISysSubRightRepository
    {
        public List<sys_sub_right> GetByName(string name);

        public List<sys_sub_right> GetByPartentId(string name);
    }
}
