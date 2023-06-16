using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Database.SqlCreate
{
    public interface ISqlHelper
    {
        string QuerySql<T>(T t1,T t2);
        string QuerySql<T>(T t1, T t2,T t3);
        string QuerySql<T>(T t1, T t2, T t3,T t4);
        string GetTotalNum<T>(T t1,T t2);
        string InsertSql<T>(T t1, T t2 ,T t3);
        string FindByIdsSql<T>(T t1,T t2,T t3);
        string FindByTwoIdSql<T>(T t1,T t2,T t3,T t4,T t5);
        string UpdateSql<T>(T t1,T t2,T t3,T t4);
        string UpdateWithTwoIdSql<T>(T t1, T t2, T t3, T t4,T t5,T t6);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <returns></returns>
        string SoftDeleteSql<T>(T t1, T t2,T t3);
        string SoftDeleteSql2<T>(T t1, T t2, T t3);
        string SoftDeleteSql3<T>(T t1,T t2,T t3,T t4,T t5);
        string QueryPageSql<T>(T t1,T t2);

        string QuerySqlWithLike<T>(T t1, T t2, T t3, T t4);
        string QuerySqlWithLike<T>(T t1, T t2, T t3, T t4,T t5,T t6);
        string QuerySqlWithGroup<T>(T t1,T t2,T t3);
    }
}
