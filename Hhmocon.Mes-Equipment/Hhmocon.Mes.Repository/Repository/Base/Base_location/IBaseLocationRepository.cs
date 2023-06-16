using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseLocationRepository
    {
        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName);

        /// <summary>
        /// 删除地点
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteLocation(string[] ids, IDbConnection dbConnection = null);
    }
}
