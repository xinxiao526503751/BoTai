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

namespace Hhmocon.Mes.Repository.Repository.Equipment.EquType
{
    /// <summary>
    /// 设备类型接口实现类
    /// </summary>
    public class EquTypeRepository : IEquTypeRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ILogger<EquTypeRepository> _logger;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="logger"></param>
        public EquTypeRepository(ISqlHelper sqlHelper, ILogger<EquTypeRepository> logger)
        {
            this._sqlHelper = sqlHelper;
            this._logger = logger;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(EquipmentType entity)
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
                        string str1 = CommonHelper.GetTableFieldList<EquipmentType>();
                        string str2 = CommonHelper.GetTableFieldListWithSign<EquipmentType>();

                        string sql = _sqlHelper.InsertSql("EquipmentTypes", str1, str2);
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
        /// 软删除操作，可进行批量删除
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
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
                        string sql = _sqlHelper.SoftDeleteSql("EquipmentTypes", "EQU_TYPE_ID", "Id");
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
        /// 根据Id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquipmentType> FindById(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentTypes", "EQU_TYPE_ID", "Id", "CREATED_TIME");
                    var data = await conn.QueryFirstOrDefaultAsync<EquipmentType>(sql, new { id });

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
        public Task<IEnumerable<EquipmentType>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据name查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EquipmentType> FindByName(string name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentTypes", "EQU_TYPE_NAME", "name", "CREATED_TIME");
                    var data = await conn.QueryFirstOrDefaultAsync<EquipmentType>(sql, new { name });
                    return data;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message ?? e.Message);
                }
            }
        }

        public Task<EquipmentType> FindByTwoIds(string id1, string id2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalNum()
        {
            using (IDbConnection conn=SqlServerDbHelper.GetConn())
            {
                if(conn.State==ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.GetTotalNum("EQU_TYPE_ID","EquipmentTypes");
                    var num = await conn.QueryFirstAsync<int>(sql,null);
                    return num;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new Exception(e.InnerException?.Message??e.Message);
                }
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentType>> QueryAsync()
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    var sql = _sqlHelper.QuerySql("EquipmentTypes", "CREATED_TIME");
                    var data = await conn.QueryAsync<EquipmentType>(sql, null);
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
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns></returns>
        public Task<IEnumerable<EquipmentType>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<EquipmentType>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentType>> QueryPageAsync(int pageIndex, int pageSize)
        {
            using(IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if(conn.State==ConnectionState.Closed)
                {
                    conn.Open();
                }
                try
                {
                    string sql = _sqlHelper.QueryPageSql("EquipmentTypes", "CREATED_TIME");
                    var data =await conn.QueryAsync<EquipmentType>(sql, new { pageindex = pageIndex, pagesize = pageSize });
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
        public Task<bool> UpdateAsync(string id, EquipmentType entity)
        {
            throw new NotImplementedException();
        }
    }
}
