using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Util.AutofacManager;
using hmocon.Mes.Repository.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository.Repository
{
    public class DippingDevRepository : IDippingDevRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly ILogger _logger;
        public DippingDevRepository(PikachuRepository pikachuRepository, ILogger<DippingDevRepository> logger)
        {
            _pikachuRepository = pikachuRepository;
            _logger = logger;
        }

        /// <summary>
        /// 查找符合条件的dipping_dev_data
        /// </summary>
        /// <param name="EquipmentName"></param>
        /// <param name="VariableCode"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<dipping_dev_data> GetDippingDevs(string EquipmentName, string VariableCode, DateTime StartTime, DateTime EndTime)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            try
            {
                string table = "dipping_dev_data";
                string sql = "";
                if (EquipmentName == "面纸" || EquipmentName == "底纸")
                {
                    table = "dipping_dev_data";
                    sql = $"select {VariableCode},datatime from {table} where dipping_dev_code = '{EquipmentName}' and datatime between '{StartTime}' and '{EndTime}'";
                }
                List<dipping_dev_data> dipping_Dev_Datas = conn.Query<dipping_dev_data>(sql).ToList();
                return dipping_Dev_Datas;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// 查找符合条件的dipping_platen_data
        /// </summary>
        /// <param name="EquipmentName"></param>
        /// <param name="VariableCode"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<dipping_platen_data> GetPlatenDevs(string EquipmentName, string VariableCode, DateTime StartTime, DateTime EndTime)
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
                if (EquipmentName == "压板")
                {
                    sql = $"select {VariableCode},datetime from {table} where dipping_platen_code = '{EquipmentName}' and datetime between '{StartTime}' and '{EndTime}'";
                }
                List<dipping_platen_data> dipping_Platen_Datas = conn.Query<dipping_platen_data>(sql).ToList();
                return dipping_Platen_Datas;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.Message);
            }
        }

        public List<dipping_dev_data> GetDippingDataLast(string dippingDevCode)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                string table = "dipping_dev_data";
                string sql = "";
                sql = $"select * from {table} where dipping_dev_code = '{dippingDevCode}'";
                List<dipping_dev_data>  dipping_Dev_Datas = conn.Query<dipping_dev_data>(sql).ToList();
                return dipping_Dev_Datas;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.Message);
            }
        }
    }
}
