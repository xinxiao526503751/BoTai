using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Repository
{
    public class SysRoleRightRepository : ISysRoleRightRepository, IDependency
    {
        public List<sys_role_right> GetByRoleId(string roleId)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_role_right> data = conn.GetByOneFeildsSql<sys_role_right>("role_id", roleId);
                return data;
            }
        }
    }
}
