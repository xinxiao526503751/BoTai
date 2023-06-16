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

namespace Hhmocon.Mes.Repository.Repository.Equipment.Check
{
    /// <summary>
    /// 设备点检项控制层
    /// </summary>
    public class EquCheckItemRepository : IEquCheckItemRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<EquCheckItemRepository> _logger;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public EquCheckItemRepository(ISqlHelper sqlHelper, ILogger<EquCheckItemRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(EquCheckItem entity)
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
                        string str1 = CommonHelper.GetTableFieldList<EquCheckItem>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<EquCheckItem>();
                        string sql = _sqlHelper.InsertSql("EquCheckItems", str1, str2);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
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
                        string sql = _sqlHelper.SoftDeleteSql2("EquCheckItems", "PLAN_ID", "ids");
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
        public async Task<EquCheckItem> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquCheckItems", "PLAN_ID", "Id");
                    var data = await conn.QueryFirstOrDefaultAsync<EquCheckItem>(sql, new { id });
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
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<IEnumerable<EquCheckItem>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据name查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EquCheckItem> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquCheckItems", "CHECK_ITEM_NAME", "name");
                    var data = await conn.QueryFirstOrDefaultAsync<EquCheckItem>(sql, new { name });
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
        /// 
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
                    string sql = _sqlHelper.GetTotalNum("PLAN_ID", "EquCheckItems");
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
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquCheckItem>> QueryAsync()
        {
            using(IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquCheckItems","CREATED_TIME");

                    var data = await conn.QueryAsync<EquCheckItem>(sql,null);
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message??e.Message);
                }
            }
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquCheckItem>> QueryAsync(string Q1, string Q2)
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
                        string sql = _sqlHelper.QuerySqlWithLike("EquCheckItems", "EQU_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquCheckItem>(sql, null);
                        return data;
                    }
                    else if (string.IsNullOrEmpty(Q2))
                    {
                        Q1 = Q1.Trim();
                        Q1 = Q1.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquCheckItems", "PLAN_ID", Q1, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquCheckItem>(sql, null);
                        return data;
                    }
                    else
                    {
                        Q1 = Q1.Trim();
                        Q2 = Q2.Trim();
                        Q1 = Q1.ToUpper();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquCheckItems", "PLAN_ID", Q1, "EQU_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquCheckItem>(sql, null);
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
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquCheckItem>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquCheckItems", "EQU_ID", "Id");
                    var data = await conn.QueryAsync<EquCheckItem>(sql, new { id });
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
        public async Task<IEnumerable<EquCheckItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("EquCheckItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquCheckItem>(sql, new { pageindex = pageIndex, pagesize = pageSize });
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
        public async Task<bool> UpdateAsync(string id, EquCheckItem entity)
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
                        string str = CommonHelper.GetTableFiledListByUpdate<EquCheckItem>();
                        string sql = _sqlHelper.UpdateSql("EquCheckItems", str, "PLAN_ID", "Id");
                        //利用反射获取更新参数
                        var parameters = CommonHelper.GetUpdateParameter<EquCheckItem>(entity);
                        parameters.Add("Id",id);

                        await conn.ExecuteAsync(sql,parameters,dbTransaction);
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
    }
}
