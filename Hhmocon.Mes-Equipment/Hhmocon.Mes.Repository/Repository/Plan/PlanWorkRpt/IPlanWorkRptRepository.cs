using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface IPlanWorkRptRepository
    {
        public List<planWorkRptResponse> QueryPlanWorkRptResponse(string id, string whereStr);
    }
}
