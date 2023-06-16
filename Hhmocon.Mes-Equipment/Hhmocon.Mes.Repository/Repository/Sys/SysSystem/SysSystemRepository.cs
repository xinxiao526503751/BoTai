using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 系统仓储
    /// </summary>
    public class SysSystemRepository : ISysSystemRepository, IDependency
    {
        public List<sys_system> GetByName(string name)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_system> data = conn.GetByOneFeildsSql<sys_system>("sys_name", name);
                return data;
            }
        }

        public List<sys_right> GetBySysId(string SysId)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_right> data = conn.GetByOneFeildsSql<sys_right>("sys_id", SysId);
                return data;
            }
        }
    }
}
