using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public class SysSubRightRepository : ISysSubRightRepository, IDependency
    {

        public List<sys_sub_right> GetByName(string name)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_sub_right> data = conn.GetByOneFeildsSql<sys_sub_right>("sub_right_name", name);
                return data;
            }
        }

        public List<sys_sub_right> GetByPartentId(string parent_right_id)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<sys_sub_right> data = conn.GetByOneFeildsSql<sys_sub_right>("right_id", parent_right_id);
                return data;
            }
        }
    }
}
