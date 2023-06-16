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
    /// 
    /// </summary>
    public class EquMaintainItemRepository : IEquMaintainItemRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<EquMaintainItemRepository> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public EquMaintainItemRepository(ISqlHelper sqlHelper, ILogger<EquMaintainItemRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(EquMaintainItem entity)
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
                        string str1 = CommonHelper.GetTableFieldList<EquMaintainItem>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<EquMaintainItem>();

                        string sql = _sqlHelper.InsertSql("EquMaintainItems",str1,str2);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据双键删除
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByTwoId(string id1, string id2)
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
                        string sql = _sqlHelper.SoftDeleteSql3("EquMaintainItems","EQU_ID","Id1","MAINTAIN_ITEM_ID","Id2");

                        await conn.ExecuteAsync(sql,new { Id1=id1,Id2=id2 },dbTransaction);
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
        /// 针对设备id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintainItem> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquMaintainItems", "EQU_ID", "Id");
                    var data = await conn.QueryFirstOrDefaultAsync<EquMaintainItem>(sql, new { id });
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
        public Task<IEnumerable<EquMaintainItem>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据维修项名称查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EquMaintainItem> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquMaintainItems", "MAINTAIN_ITEM_NAME", "name");
                    var data = await conn.QueryFirstOrDefaultAsync<EquMaintainItem>(sql, new { name });
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
        /// 根据双Id查找
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public async Task<EquMaintainItem> FindByTwoId(string id1, string id2)
        {
           using(IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if(conn.State==ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.FindByTwoIdSql("EquMaintainItems","EQU_ID","Id1","MAINTAIN_ITEM_ID","Id2");

                    var data = await conn.QueryFirstOrDefaultAsync<EquMaintainItem>(sql,new { Id1=id1,Id2=id2 });

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
                    string sql = _sqlHelper.GetTotalNum("DELETE_MARK", "EquMaintainItems");
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
        public async Task<IEnumerable<EquMaintainItem>> QueryAsync()
        {
            using(IDbConnection conn=SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquMaintainItems","CREATED_TIME");

                    var data = await conn.QueryAsync<EquMaintainItem>(sql,null);

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
        public async Task<IEnumerable<EquMaintainItem>> QueryAsync(string Q1, string Q2)
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
                        string sql = _sqlHelper.QuerySqlWithLike("EquMaintainItems", "MAINTAIN_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquMaintainItem>(sql, null);
                        return data;
                    }
                    else if (string.IsNullOrEmpty(Q2))
                    {
                        Q1 = Q1.Trim();
                        Q1 = Q1.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquMaintainItems", "EQU_NAME", Q1, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquMaintainItem>(sql, null);
                        return data;
                    }
                    else
                    {
                        Q1 = Q1.Trim();
                        Q2 = Q2.Trim();
                        Q1 = Q1.ToUpper();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquMaintainItems", "EQU_NAME", Q1, "MAINTAIN_ITEM_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquMaintainItem>(sql, null);
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
        /// 根据设备Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintainItem>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquMaintainItems", "EQU_ID", "Id");
                    var data = await conn.QueryAsync<EquMaintainItem>(sql, new { id });
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
        public async Task<IEnumerable<EquMaintainItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("EquMaintainItems", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquMaintainItem>(sql, new { pageindex = pageIndex, pagesize = pageSize });
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
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string id, EquMaintainItem entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新2
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="equMaintainItem"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWithTwoId(string id1, string id2, EquMaintainItem equMaintainItem)
        {
            using(IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using(IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string str = CommonHelper.GetTableFiledListByUpdate<EquMaintainItem>();
                        string sql = _sqlHelper.UpdateWithTwoIdSql("EquMaintainItems",str,"EQU_ID","Id1","MAINTAIN_ITEM_ID","Id2");
                        var parameters = CommonHelper.GetUpdateParameter<EquMaintainItem>(equMaintainItem);

                        parameters.Add("Id1",id1);
                        parameters.Add("Id2",id2);

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
