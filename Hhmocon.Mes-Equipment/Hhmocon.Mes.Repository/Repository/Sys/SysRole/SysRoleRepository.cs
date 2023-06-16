using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public class SysRoleRepository : ISysRoleRepository, IDependency
    {
        public List<sys_role> GetByName(string name)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_role> data = conn.GetByOneFeildsSql<sys_role>("role_name", name);
                return data;
            }
        }





    }
}
