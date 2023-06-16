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

namespace Hhmocon.Mes.Repository.Repository.Equipment.Maintain
{
    /// <summary>
    /// 仓储层接口实现类
    /// </summary>
    public class MaintainItemRepository : IMaintainItemRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<MaintainItemRepository> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public MaintainItemRepository(ISqlHelper sqlHelper, ILogger<MaintainItemRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(MaintainItem entity)
        {
            using(IDbConnection conn=SqlServerDbHelper.GetConn())
            {
                if(conn.State==ConnectionState.Closed)
                {
                    conn.Open();
                }
                using(IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string str1 = CommonHelper.GetTableFieldList<MaintainItem>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<MaintainItem>();

                        string sql = _sqlHelper.InsertSql("MaintainItems",str1,str2);

                        await conn.ExecuteAsync(sql,entity,dbTransaction);
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        _logger.LogError(e.Message);
                        throw new Exception(e.InnerException?.Message??e.Message);
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
                        string sql = _sqlHelper.SoftDeleteSql("MaintainItems", "MAINTAIN_ITEM_ID", "Id");
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
                        string sql = _sqlHelper.SoftDeleteSql2("MaintainItems", "MAINTAIN_ITEM_ID", "ids");
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
        /// 根据Id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MaintainItem> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("MaintainItems", "MAINTAIN_ITEM_ID", "Id");
                    var data = await conn.QueryFirstOrDefaultAsync<MaintainItem>(sql, new { id });
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
        public async Task<IEnumerable<MaintainItem>> FindByIds(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.FindByIdsSql("MaintainItems", "MAINTAIN_ITEM_ID", "ids");
                    var data = await conn.QueryAsync<MaintainItem>(sql, new { ids });
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
        public async Task<MaintainItem> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("MaintainItems", "MAINTAIN_ITEM_NAME", "name", "CREATED_TIME");
                    var data = await conn.QueryFirstOrDefaultAsync<MaintainItem>(sql, new { name });
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
        /// 获取数据总量
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
                    string sql = _sqlHelper.GetTotalNum("MAINTAIN_ITEM_ID", "MaintainItems");
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
        public async Task<IEnumerable<MaintainItem>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("MaintainItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<MaintainItem>(sql, null);
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
        public async Task<IEnumerable<MaintainItem>> QueryAsync(string Q1, string Q2)
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
                        string sql = _sqlHelper.QuerySqlWithLike("MaintainItems", "MAINTAIN_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<MaintainItem>(sql, null);
                        return data;
                    }
                    else if (string.IsNullOrEmpty(Q2))
                    {
                        Q1 = Q1.Trim();
                        Q1 = Q1.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("MaintainItems", "MAINTAIN_ITEM_ID", Q1, "CREATED_TIME");
                        var data = await conn.QueryAsync<MaintainItem>(sql, null);
                        return data;
                    }
                    else
                    {
                        Q1 = Q1.Trim();
                        Q2 = Q2.Trim();
                        Q1 = Q1.ToUpper();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("MaintainItems", "MAINTAIN_ITEM_ID", Q1, "MAINTAIN_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<MaintainItem>(sql, null);
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
        /// 根据TypeId查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MaintainItem>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("MaintainItems", "TYPE_ID", "Id", "CREATED_TIME");
                    var data = await conn.QueryAsync<MaintainItem>(sql, new { id });
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
        public async Task<IEnumerable<MaintainItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("MaintainItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<MaintainItem>(sql, new { pageindex = pageIndex, pagesize = pageSize });
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
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string id, MaintainItem entity)
        {
            using(IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if(conn.State==ConnectionState.Closed)
                {
                    conn.Open();
                }

                using(IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string str = CommonHelper.GetTableFiledListByUpdate<MaintainItem>();
                        string sql = _sqlHelper.UpdateSql("MaintainItems",str,"MAINTAIN_ITEM_ID","Id");

                        var parameters = CommonHelper.GetUpdateParameter<MaintainItem>(entity);
                        parameters.Add("Id",id);

                        await conn.ExecuteAsync(sql,parameters,dbTransaction);
                        dbTransaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        _logger.LogError(e.Message);
                        throw new Exception(e.InnerException?.Message??e.Message);
                    }
                }
            }
        }
    }
}
