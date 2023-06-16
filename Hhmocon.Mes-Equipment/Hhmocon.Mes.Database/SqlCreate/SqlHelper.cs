using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Database.SqlCreate
{
    public class SqlHelper : ISqlHelper
    {
        public string FindByIdsSql<T>(T t1, T t2, T t3)
        {
            return string.Format("SELECT * FROM {0} WHERE DELETE_MARK=0 AND {1} IN @{2}",t1,t2,t3);
        }

        public string FindByTwoIdSql<T>(T t1, T t2, T t3, T t4,T t5)
        {
            return string.Format("SELECT * FROM {0} WHERE DELETE_MARK=0 AND {1}=@{2} AND {3}=@{4}",t1,t2,t3,t4,t5);
        }

        public string GetTotalNum<T>(T t1, T t2)
        {
            return string.Format("SELECT COUNT({0}) AS Total FROM {1} WHERE DELETE_MARK=0", t1, t2);
        }

        public string InsertSql<T>(T t1, T t2,T t3)
        {
            return string.Format("INSERT INTO {0} {1} VALUES {2}",t1,t2,t3);
        }

        public string QueryPageSql<T>(T t1, T t2)
        {
            return string.Format("SELECT * FROM {0} WHERE DELETE_MARK=0 ORDER BY {1} DESC OFFSET (@pagesize*(@pageindex-1))" +
                "ROWS FETCH NEXT (@pagesize) ROWS ONLY",t1,t2);
        }

        public string QuerySql<T>(T t1, T t2)
        {
            return string.Format("SELECT * FROM {0} WHERE DELETE_MARK=0 ORDER BY {1} DESC", t1,t2);
        }
        public string QuerySql<T>(T t1, T t2, T t3)
        {
            return string.Format("SELECT * FROM {0} WHERE {1}=@{2} AND DELETE_MARK=0", t1, t2, t3);
        }

        public string QuerySql<T>(T t1, T t2, T t3,T t4)
        {
            return string.Format("SELECT * FROM {0} WHERE {1}=@{2} AND DELETE_MARK=0 ORDER BY {3} DESC",t1,t2,t3,t4);
        }

        public string QuerySqlWithGroup<T>(T t1, T t2, T t3)
        {
            return string.Format("SELECT {0},{1} FROM {2} GROUP BY {0},{1}",t1,t2,t3);
        }

        public string QuerySqlWithLike<T>(T t1, T t2, T t3, T t4)
        {
            return string.Format("SELECT * FROM {0} WHERE {1} LIKE '%{2}%' AND DELETE_MARK=0 ORDER BY {3} DESC",t1,t2,t3,t4);
        }

        public string QuerySqlWithLike<T>(T t1, T t2, T t3, T t4, T t5, T t6)
        {
            return string.Format("SELECT * FROM {0} WHERE {1} LIKE '%{2}%'AND {3} LIKE '%{4}%' AND DELETE_MARK=0 ORDER BY {5} DESC", t1, t2, t3, t4, t5, t6);
        }

        public string SoftDeleteSql<T>(T t1, T t2, T t3)
        {
            return string.Format("UPDATE {0} SET DELETE_MARK=1 WHERE {1}=@{2}",t1,t2,t3);
        }

        public string SoftDeleteSql2<T>(T t1, T t2, T t3)
        {
            return string.Format("UPDATE {0} SET DELETE_MARK=1 WHERE {1} IN @{2}",t1,t2,t3);
        }

        public string SoftDeleteSql3<T>(T t1, T t2, T t3, T t4,T t5)
        {
            return string.Format("UPDATE {0} SET DELETE_MARK=1 WHERE {1}=@{2} AND {3}=@{4}",t1,t2,t3,t4,t5);
        }

        public string UpdateSql<T>(T t1, T t2, T t3, T t4)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2}=@{3}",t1,t2,t3,t4);
        }

        public string UpdateWithTwoIdSql<T>(T t1, T t2, T t3, T t4, T t5, T t6)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2}=@{3} AND {4}=@{5}", t1, t2, t3, t4,t5,t6);
        }
    }
}
