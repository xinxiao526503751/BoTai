using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 工艺路线App
    /// </summary>
    public class BaseProcessRouteApp
    {
        private readonly IBaseProcessRouteRepository _baseProcessRouteRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;

        public BaseProcessRouteApp(IBaseProcessRouteRepository repository, PikachuRepository pikachuRepository, IAuth auth, PikachuApp pikachuApp)
        {
            _baseProcessRouteRepository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
            _pikachuApp = pikachuApp;
        }
        /// <summary>
        /// 添加工艺路线数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_process_route Insert(base_process_route data)
        {
            //查找物料下的工艺路线
            List<base_process_route> base_Process_Routes;
            base_Process_Routes = _pikachuRepository.GetByOneFeildsSql<base_process_route>("material_id", data.material_id)?.Where(c => c.process_route_code == data.process_route_code && c.process_route_name == data.process_route_name).ToList();
            if (base_Process_Routes.Count != 0)
            {
                throw new Exception($"编码{data.process_route_code}和名称{data.process_route_name}重复");
            }
            base_Process_Routes = _pikachuRepository.GetByOneFeildsSql<base_process_route>("material_id", data.material_id)?.Where(c => c.process_route_code == data.process_route_code).ToList();
            if (base_Process_Routes.Count != 0)
            {
                throw new Exception($"编码{data.process_route_code}重复");
            }
            base_Process_Routes = _pikachuRepository.GetByOneFeildsSql<base_process_route>("material_id", data.material_id)?.Where(c => c.process_route_name == data.process_route_name).ToList();
            if (base_Process_Routes.Count != 0)
            {
                throw new Exception($"名称{data.process_route_name}重复");
            }
            //取ID
            data.process_route_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.end_time = DateTime.Now;
            data.check_time = DateTime.Now;
            data.start_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
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
        /// 根据物料id获取所有工艺路线
        /// </summary>
        /// <param name="material_id"></param>
        /// <returns></returns>
        public List<ProcessRouteResponse> GetProcessRouteByMaterialId(PageReq req, string material_id, ref int count)
        {
            List<ProcessRouteResponse> processRouteResponses = new();
            List<base_process_route> base_Process_Routes = _pikachuRepository.GetByOneFeildsSql<base_process_route>("material_id", material_id);
            foreach (base_process_route temp in base_Process_Routes)
            {
                base_material base_Material = _pikachuRepository.GetById<base_material>(temp.material_id);
                ProcessRouteResponse processRouteResponse = new();
                processRouteResponse.process_route_id = temp.process_route_id;
                processRouteResponse.process_route_code = temp.process_route_code;
                processRouteResponse.process_route_name = temp.process_route_name;
                processRouteResponse.material_code = base_Material.material_code;
                processRouteResponse.material_name = base_Material.material_name;
                processRouteResponse.is_master = temp.is_master;
                processRouteResponse.create_time = temp.create_time;
                processRouteResponses.Add(processRouteResponse);
            }
            count = processRouteResponses.Count();
            //分页
            if (req != null)
            {
                int iPage = req.page;
                int iRows = req.rows;

                processRouteResponses = processRouteResponses.Skip((iPage - 1) * iRows).Take(iRows).OrderByDescending(c => c.create_time).ToList();
            }

            return processRouteResponses;
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteProcessRoute(string[] ids, IDbConnection dbConnection = null)
        {
            _baseProcessRouteRepository.DeleteProcessRoute(ids, dbConnection);
        }

        /// <summary>
        /// 检查表单是否存在数据
        /// </summary>
        /// <param name="referenceCharts"></param>
        /// <param name="id"></param>
        /// <param name="chartName"></param>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _baseProcessRouteRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }
    }
}
