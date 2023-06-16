using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface IBaseDetailBomRespository
    {
        public List<base_bom_detail_response> QuertBomDetailListResponse(string id, string whereStr);
    }
}
