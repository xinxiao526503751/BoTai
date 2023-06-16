using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 工艺路线规则物料数据
    /// </summary>
    public class BaseProcessRouteDetailMaterialApp
    {
        private readonly IBaseProcessRouteDetailMaterialRepository _baseProcessRouteDetailMaterialRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseProcessRouteDetailMaterialApp(IBaseProcessRouteDetailMaterialRepository repository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseProcessRouteDetailMaterialRepository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加工艺路线规则物料数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert(List<base_process_route_material> data)
        {
            List<base_process_route_material> insert_data = new();

            foreach (base_process_route_material temp in data)
            {
                //取ID
                temp.process_route_material_id = CommonHelper.GetNextGUID();
                temp.modified_time = Time.Now;
                temp.create_time = DateTime.Now;
                temp.create_by = _auth.GetUserAccount(null);
                temp.create_by_name = _auth.GetUserName(null);
                temp.modified_by = _auth.GetUserAccount(null);
                temp.modified_by_name = _auth.GetUserName(null);
                if (!_pikachuRepository.Insert(temp))
                {
                    return false;
                }
                //insert_data.Add(temp);
            }
            return true;
            //if (_pikachuRepository.Insert(insert_data))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// 通过工艺路线id和工序id获取工艺路线物料需求
        /// </summary>
        /// <param name="process_route_id"></param>
        /// <param name="process_id"></param>
        /// <returns></returns>
        public List<ProcessRouteDetailMaterialResponse> GetProcessRouteMaterialByProcessRouteIdAndProcessId(string process_route_id, string process_id)
        {
            List<string> Field = new();
            Field.Add(process_id);
            List<base_process_route_material> base_Process_Route_Materials = new();
            base_Process_Route_Materials = _pikachuRepository.GetByTwoFeildsSql<base_process_route_material>("process_route_id", process_route_id, "process_id", Field);
            List<ProcessRouteDetailMaterialResponse> processRouteDetailMaterialResponses = new();
            foreach (base_process_route_material base_Process_Route_Material in base_Process_Route_Materials)
            {
                ProcessRouteDetailMaterialResponse processRouteDetailMaterialResponse = new();
                base_material base_Material = _pikachuRepository.GetById<base_material>(base_Process_Route_Material.material_id);
                processRouteDetailMaterialResponse.is_main = base_Process_Route_Material.is_main;
                processRouteDetailMaterialResponse.material_code = base_Material.material_code;
                processRouteDetailMaterialResponse.material_id = base_Process_Route_Material.material_id;
                processRouteDetailMaterialResponse.material_name = base_Material.material_name;
                processRouteDetailMaterialResponse.qty = base_Process_Route_Material.qty;
                processRouteDetailMaterialResponses.Add(processRouteDetailMaterialResponse);

            }
            return processRouteDetailMaterialResponses;
        }

    }
}
