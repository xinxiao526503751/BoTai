/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 部门仓储接口
    /// </summary>
    public interface ISysDeptRepository
    {
        /// <summary>
        /// 根据Name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_dept> GetByName(string name);

        /// <summary>
        /// 在User表中通过Dept_id获取user
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public List<sys_user> GetUserByDeptIdInUserChart(string dept_id);

        /// <summary>
        /// 将list [sys_dept]转换成list [TreeModel]
        /// </summary>
        /// <param name="list">需要转化的list</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode(List<sys_dept> list);

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteDepartment(string[] ids, IDbConnection dbConnection);
    }
}
