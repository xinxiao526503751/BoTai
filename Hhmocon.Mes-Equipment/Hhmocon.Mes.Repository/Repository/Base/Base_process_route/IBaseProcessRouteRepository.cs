using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 工艺路线仓储层接口
    /// </summary>
    public interface IBaseProcessRouteRepository
    {
        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName);

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteProcessRoute(string[] ids, IDbConnection dbConnection = null);

    }
}
