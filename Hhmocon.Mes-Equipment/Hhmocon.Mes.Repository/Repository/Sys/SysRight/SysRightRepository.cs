using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public class SysRightRepository : ISysRightRepository, IDependency
    {
        /// <summary>
        /// 根据name获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<sys_right> GetByName(string name)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_right> data = conn.GetByOneFeildsSql<sys_right>("right_name", name);
                return data;
            }
        }

        /// <summary>
        /// 根据系统级权限id获取menu数据
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        public List<sys_right> GetByParentRightId(string SysId)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_right> data = conn.GetByOneFeildsSql<sys_right>("parent_right_id", SysId);
                return data;
            }
        }
    }
}
