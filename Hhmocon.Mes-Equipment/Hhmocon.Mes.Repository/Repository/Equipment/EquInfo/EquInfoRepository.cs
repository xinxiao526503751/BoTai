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

namespace Hhmocon.Mes.Repository.Repository.Equipment.EquInfo
{
    /// <summary>
    /// 设备信息接口实现类
    /// </summary>
    public class EquInfoRepository : IEquInfoRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<EquInfoRepository> _logger;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public EquInfoRepository(ISqlHelper sqlHelper, ILogger<EquInfoRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(EquipmentInfo entity)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                };
                using (IDbTransaction dbTransaction = conn.BeginTransaction())
                {
                    try
                    {
                        string str1 = CommonHelper.GetTableFieldList<EquipmentInfo>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<EquipmentInfo>();

                        string sql = _sqlHelper.InsertSql("EquipmentInfos", str1, str2);
                        var data = await conn.ExecuteAsync(sql, entity, dbTransaction);
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
                        string sql = _sqlHelper.SoftDeleteSql2("EquipmentInfos", "EQU_ID", "Ids");
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
                        string sql = _sqlHelper.SoftDeleteSql("EquipmentInfos", "EQU_ID", "Id");
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
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquipmentInfo> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentInfos", "EQU_ID", "Id");
                    var data = await conn.QueryFirstOrDefaultAsync<EquipmentInfo>(sql, new { id });
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
        /// 根据ids批量查找
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentInfo>> FindByIds(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.FindByIdsSql("EquipmentInfos", "EQU_ID", "ids");
                    var data = await conn.QueryAsync<EquipmentInfo>(sql, new { ids });
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
        public async Task<EquipmentInfo> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentInfos", "EQU_NAME", "name");
                    var data = await conn.QueryFirstOrDefaultAsync<EquipmentInfo>(sql, new { name });
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
                    var sql = _sqlHelper.GetTotalNum("EQU_ID", "EquipmentInfos");
                    var num = await conn.QueryFirstAsync<int>(sql, null);
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
        public async Task<IEnumerable<EquipmentInfo>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentInfos", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquipmentInfo>(sql, null);
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
        public async Task<IEnumerable<EquipmentInfo>> QueryAsync(string Q1, string Q2)
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
                        string sql = _sqlHelper.QuerySqlWithLike("EquipmentInfos", "EQU_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquipmentInfo>(sql, null);
                        return data;
                    }
                    else if (string.IsNullOrEmpty(Q2))
                    {
                        Q1 = Q1.Trim();
                        Q1 = Q1.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquipmentInfos", "EQU_ID", Q1, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquipmentInfo>(sql, null);
                        return data;
                    }
                    else
                    {
                        Q1 = Q1.Trim();
                        Q2 = Q2.Trim();
                        Q1 = Q1.ToUpper();
                        Q2 = Q2.ToUpper();
                        string sql = _sqlHelper.QuerySqlWithLike("EquipmentInfos", "EQU_ID", Q1, "EQU_NAME", Q2, "CREATED_TIME");
                        var data = await conn.QueryAsync<EquipmentInfo>(sql, null);
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
        public async Task<IEnumerable<EquipmentInfo>> QueryByIdAsync(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QuerySql("EquipmentInfos", "TYPE_ID", "Id", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquipmentInfo>(sql, new { id });
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
        public async Task<IEnumerable<EquipmentInfo>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("EquipmentInfos", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquipmentInfo>(sql, new { pageindex = pageIndex, pagesize = pageSize });
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
        public async Task<bool> UpdateAsync(string id, EquipmentInfo entity)
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
                        string str = CommonHelper.GetTableFiledListByUpdate<EquipmentInfo>();
                        string sql = _sqlHelper.UpdateSql("EquipmentInfos", str, "EQU_ID", "Id");
                        await conn.ExecuteAsync(sql, new
                        {
                            EQU_ID = entity.EQU_ID,
                            EQU_NAME = entity.EQU_NAME,
                            TYPE_ID = entity.TYPE_ID,
                            EQU_SPEC = entity.EQU_SPEC,
                            EQU_STATUS = entity.EQU_STATUS,
                            WORK_SHOP = entity.WORK_SHOP,
                            INSTALL_SITE = entity.INSTALL_SITE,
                            HEAD = entity.HEAD,
                            PHONE_NO = entity.PHONE_NO,
                            MANUFACTURER = entity.MANUFACTURER,
                            VENDOR = entity.VENDOR,
                            PUR_TIME = entity.PUR_TIME,
                            ENABLE_TIME = entity.ENABLE_TIME,
                            DELETE_MARK = entity.DELETE_MARK,
                            CREATED_TIME = entity.CREATED_TIME,
                            CREATED_BY = entity.CREATED_BY,
                            UPDATE_TIME = entity.UPDATE_TIME,
                            UPDATE_BY = entity.UPDATE_BY,
                            Id = id
                        }, dbTransaction);
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
