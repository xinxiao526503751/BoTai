using System;
using System.Data;

namespace Hhmocon.Mes.DataBase
{
    internal class BuilderFactory
    {
        private static readonly ISqlBuilder sqlserver = new SqlServerBuilder();

        /// <summary>
        /// 根据传入的IDbConnection的类型名判断是sqlserver还是别的
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static ISqlBuilder GetBuilder(IDbConnection conn)
        {
            string dbType = conn.GetType().Name;
            if (dbType.Equals("SqlConnection"))
            {
                return sqlserver;
            }
            else
            {
                throw new Exception("Unknown DbType:" + dbType);
            }
        }
    }
}
