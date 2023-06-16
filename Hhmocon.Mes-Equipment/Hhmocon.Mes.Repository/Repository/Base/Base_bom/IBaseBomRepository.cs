using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface IBaseBomRepository
    {
        public List<base_bom_response> QuertBomListResponse(string id, string whereStr);
        public List<base_bom_response> QuertBomListByParentId(string id);
    }
}
