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

namespace Hhmocon.Mes.Repository.Repository.Equipment.Upkeep
{
    /// <summary>
    /// 
    /// </summary>
    public class UpkeepItemRepository : IUpkeepItemRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<UpkeepItemRepository> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public UpkeepItemRepository(ISqlHelper sqlHelper, ILogger<UpkeepItemRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(UpkeepItem entity)
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
                        string str1 = CommonHelper.GetTableFieldList<UpkeepItem>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<UpkeepItem>();

                        string sql = _sqlHelper.InsertSql("UpkeepItems", str1, str2);
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
        /// 软删除
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
                        string sql = _sqlHelper.SoftDeleteSql("UpkeepItems", "UPKEEP_ITEM_ID", "Id");
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
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string[] ids)
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
                        string sql = _sqlHelper.SoftDeleteSql2("UpkeepItems", "UPKEEP_ITEM_ID", "ids");
                        await conn.ExecuteAsync(sql, new { ids }, dbTransaction);
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
        /// 查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UpkeepItem> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("UpkeepItems", "UPKEEP_ITEM_ID", "Id");
                    var data = await conn.QueryFirstOrDefaultAsync(sql, new { id });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 批量查找
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UpkeepItem>> FindByIds(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.FindByIdsSql("UpkeepItems", "UPKEEP_ITEM_ID", "ids");
                    var data = await conn.QueryAsync<UpkeepItem>(sql, new { ids });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 根据name查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<UpkeepItem> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("UpkeepItems", "UPKEEP_ITEM_NAME", "name", "CREATED_TIME");
                    var data = await conn.QueryFirstOrDefaultAsync(sql, new { name });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalNum()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.GetTotalNum("UPKEEP_ITEM_ID", "UpkeepItems");
                    int num = await conn.QueryFirstAsync<int>(sql, null);
                    return num;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UpkeepItem>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("UpkeepItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<UpkeepItem>(sql, null);
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UpkeepItem>> QueryAsync(string Q1, string Q2)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(Q1) && string.IsNullOrWhiteSpace(Q2))
                    {
                        return await QueryAsync();
                    }
                    if (string.IsNullOrWhiteSpace(Q1))
                    {
                        Q2 = Q2.Trim();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("UpkeepItems", "UPKEEP_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<UpkeepItem>(sql, null);
                        return data;
                    }
                    else if (string.IsNullOrEmpty(Q2))
                    {
                        Q1 = Q1.Trim();
                        Q1 = Q1.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("UpkeepItems", "UPKEEP_ITEM_ID", Q1, "CREATED_TIME");
                        var data = await conn.QueryAsync<UpkeepItem>(sql, null);
                        return data;
                    }
                    else
                    {
                        Q1 = Q1.Trim();
                        Q2 = Q2.Trim();
                        Q1 = Q1.ToUpper();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("UpkeepItems", "UPKEEP_ITEM_ID", Q1, "UPKEEP_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<UpkeepItem>(sql, null);
                        return data;
                    }

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        /// <summary>
        /// 根据typeId查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UpkeepItem>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("UpkeepItems", "TYPE_ID", "Id", "CREATED_TIME");
                    var data = await conn.QueryAsync<UpkeepItem>(sql, new { id });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UpkeepItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("UpkeepItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<UpkeepItem>(sql, new { pageindex = pageIndex, pagesize = pageSize });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        public Task<bool> UpdateAsync(string id, UpkeepItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
