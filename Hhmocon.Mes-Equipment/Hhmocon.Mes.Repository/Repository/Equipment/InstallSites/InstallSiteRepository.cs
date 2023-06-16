using Dapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Database.SqlCreate;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain.Equipment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.Equipment.InstallSites
{
    public class InstallSiteRepository : IInstallSiteRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<InstallSiteRepository> _logger;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public InstallSiteRepository(ISqlHelper sqlHelper, ILogger<InstallSiteRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }

        public async Task<bool> CreateAsync(InstallSite entity)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string str1 = CommonHelper.GetTableFieldList<InstallSite>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<InstallSite>();
                        string sql = _sqlHelper.InsertSql("InstallSites", str1, str2);

                        await conn.ExecuteAsync(sql, entity, dbTransaction);
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        _logger.LogError(e.Message);
                        throw new Exception(e.InnerException?.Message ?? e.Message);
                    }
                }
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = _sqlHelper.SoftDeleteSql("InstallSites", "SiteId", "Id");

                        await conn.ExecuteAsync(sql, new { id }, dbTransaction);

                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        _logger.LogError(e.Message);
                        throw new Exception(e.InnerException?.Message ?? e.Message);
                    }
                }
            }
        }

        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<InstallSite> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InstallSite>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<InstallSite> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstallSite>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("InstallSites", "CREATED_TIME");
                    var data = await conn.QueryAsync<InstallSite>(sql, null);
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        public Task<IEnumerable<InstallSite>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstallSite>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("InstallSites", "PlaceId", "Id");

                    var data = await conn.QueryAsync<InstallSite>(sql, new { id });

                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        public Task<IEnumerable<InstallSite>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, InstallSite entity)
        {
            throw new NotImplementedException();
        }
    }
}
