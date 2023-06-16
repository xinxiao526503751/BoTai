using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 工艺路线仓储层
    /// </summary>
    public class BaseProcessRouteRepository : IBaseProcessRouteRepository, IDependency
    {
        private readonly PikachuRepository _pikachuRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuRepository"></param>
        public BaseProcessRouteRepository(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            int flag = 0;
            int Pass_flag = 0;
            switch (chartName)
            {
                //找自身表
                case "base_process_route":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_process_route>("process_route_id", id).Count() > 1)
                        {
                            throw new Exception("工艺路线出现两个相同id数据");
                        }
                    }

                    break;
                case "base_process_route_detail"://详细工艺路线有没有数据
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_process_route_detail>("process_route_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
                case "base_process_route_material":
                    {
                        if (_pikachuRepository.GetByOneFeildsSql<base_process_route_material>("process_route_id", id).Count() > 0)
                        {
                            flag++;
                        }
                        Pass_flag++;
                    }
                    break;
            }

            if (Pass_flag == 0)
            {
                throw new Exception($"CheckChartIfExistsData出现未预设的表单{chartName}");
            }
            if (flag > 0)
            {
                referenceCharts.Add(chartName);
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public void DeleteProcessRoute(string[] ids, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                foreach (string id in ids)
                {
                    base_process_route base_Process_Route = _pikachuRepository.GetById<base_process_route>(id, tran: transaction, dbConnection: conn);
                    if (base_Process_Route == null)
                    {
                        throw new Exception($"无效的工艺路线id={id}");
                    }
                    else
                    {
                        string[] s = new string[] { id };
                        _pikachuRepository.Delete_Mask<base_process_route_detail>(s, tran: transaction, dbConnection: conn);
                    }

                    List<base_process_route_detail> base_Process_Route_Details = _pikachuRepository.GetByOneFeildsSql<base_process_route_detail>("process_route_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_process_route_detail temp in base_Process_Route_Details)
                    {
                        string[] s = new string[] { temp.process_route_detail_id };
                        _pikachuRepository.Delete_Mask<base_process_route_detail>(s, tran: transaction, dbConnection: conn);
                    }

                    List<base_process_route_material> base_Process_Route_Materials = _pikachuRepository.GetByOneFeildsSql<base_process_route_material>("process_route_material_id", id, tran: transaction, dbConnection: conn);
                    foreach (base_process_route_material temp in base_Process_Route_Materials)
                    {
                        string[] s = new string[] { temp.process_route_material_id };
                        _pikachuRepository.Delete_Mask<base_process_route_material>(s, tran: transaction, dbConnection: conn);
                    }

                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

        }
    }
}