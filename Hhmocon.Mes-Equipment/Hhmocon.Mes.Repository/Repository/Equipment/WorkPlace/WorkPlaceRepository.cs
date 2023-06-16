using Dapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Database.SqlCreate;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.Equipment.WorkPlace
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkPlaceRepository : IWorkPlaceRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<WorkPlaceRepository> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public WorkPlaceRepository(ISqlHelper sqlHelper, ILogger<WorkPlaceRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(WorkShop entity)
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
                        string str1 = CommonHelper.GetTableFieldList<WorkShop>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<WorkShop>();
                        string sql = _sqlHelper.InsertSql("WorkShops", str1, str2);

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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                        string sql = _sqlHelper.SoftDeleteSql("WorkShops", "PlaceId", "Id");

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

        public Task<WorkShop> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkShop>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<WorkShop> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WorkShop>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("WorkShops", "CREATED_TIME");
                    var data = await conn.QueryAsync<WorkShop>(sql, null);
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        public Task<IEnumerable<WorkShop>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据父Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WorkShop>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("WorkShops", "ParentPlaceId", "Id");

                    var data = await conn.QueryAsync<WorkShop>(sql, new { id });

                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }


        public Task<IEnumerable<WorkShop>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, WorkShop entity)
        {
            throw new NotImplementedException();
        }
    }
}
