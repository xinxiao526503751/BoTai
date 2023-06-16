using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseLocationApp
    {
        private readonly IBaseLocationRepository _Repository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;

        public BaseLocationApp(IBaseLocationRepository repository, PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth)
        {
            _Repository = repository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
        }


        /// <summary>
        /// 写入地点数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_location InsertBaseLoction(base_location data)
        {
            //取ID
            data.location_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            if (data.location_parentid == "")
            {
                data.location_parentid = null;
            }

            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _Repository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }

        /// <summary>
        /// 删除地点
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteLocation(string[] ids, IDbConnection dbConnection = null)
        {
            _Repository.DeleteLocation(ids, dbConnection);
        }
    }
}

