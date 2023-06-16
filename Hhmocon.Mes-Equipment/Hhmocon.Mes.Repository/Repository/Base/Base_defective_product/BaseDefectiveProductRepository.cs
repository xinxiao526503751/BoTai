using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 产品缺陷仓储
    /// </summary>
    public class BaseDefectiveProductRepostiory : IBaseDefectiveProductRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        private readonly ILogger<BaseDefectiveProductRepostiory> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pikachuRepository"></param>
        /// <param name="auth"></param>
        /// <param name="logger"></param>
        public BaseDefectiveProductRepostiory(PikachuRepository pikachuRepository, IAuth auth, ILogger<BaseDefectiveProductRepostiory> logger)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
            _logger = logger;
        }

        /// <summary>
        /// 通过时间范围和不合格原因找产品
        /// </summary>
        public List<base_defective_product> GetByTimeScopeAndReason(DateTime StartTime, DateTime EndTime, base_defective_reason base_Defective_Reason)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            string sql = $"select * from base_defective_product where create_time between '{StartTime}' and '{EndTime}' and defective_reason_id = '{base_Defective_Reason.defective_reason_id}' and delete_mark = '0'";

            List<base_defective_product> Defective_Products = conn.Query<base_defective_product>(sql: sql).ToList();
            return Defective_Products;
        }


    }
}
