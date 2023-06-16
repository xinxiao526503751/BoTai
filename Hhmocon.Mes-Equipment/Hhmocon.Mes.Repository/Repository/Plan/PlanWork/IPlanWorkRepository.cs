using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 计划工单仓储接口
    /// </summary>
    public interface IPlanWorkRepository
    {
        public List<planWorkResponse> QueryPlanWorkResponse(string id, string whereStr);

    }
}
