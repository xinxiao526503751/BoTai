using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Util.AutofacManager;
using hmocon.Mes.Repository.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.DippingDev.DippingPlaten
{
    public class DippingPlantenRepository : IDippingPlantenRepository,IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger _logger;
        public DippingPlantenRepository(PikachuRepository pikachuRepository, ILogger<DippingPlantenRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }
        public List<dipping_platen_data> getDippingPlatenLatest()
        {
        
                using IDbConnection conn = SqlServerDbHelper.GetConn();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string table = "dipping_platen_data";
                    string sql = "";
                    sql = $"select top 1 * from {table} order by 'datetime' desc ";
                    //sql = $"select top 1 * from {table}";
                    List<dipping_platen_data> dipping_platen_Datas = conn.Query<dipping_platen_data>(sql).ToList();
                    return dipping_platen_Datas;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }
            
        }
    }
}
