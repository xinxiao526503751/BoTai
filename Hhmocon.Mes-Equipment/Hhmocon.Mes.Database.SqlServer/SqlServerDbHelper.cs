using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Hhmocon.Mes.DataBase.SqlServer
{
    /// <summary>
    /// SqlServer数据库帮助类
    /// </summary>
    public class SqlServerDbHelper
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetConn()
        {
            string sqlserverconn =
                ConfigurationManager.Configuration.GetConnectionString("sqlserverconn");
            return new SqlConnection(sqlserverconn);
        }
    }



    public class ConfigurationManager
    {
        public static readonly IConfiguration Configuration;

        static ConfigurationManager()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();
        }
    }
}
