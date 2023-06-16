using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.ProcessRoute
{
    /// <summary>
    /// 工艺路线规则物料细节
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "ProcessRoute", IgnoreApi = false)]
    [ApiController]
    public class ProcessRouteMaterialController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly BaseProcessRouteDetailMaterialApp _BaseProcessRouteDetailMaterialApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="BaseProcessRouteDetailMaterialApp"></param>
        public ProcessRouteMaterialController(PikachuApp pikachuApp, BaseProcessRouteDetailMaterialApp BaseProcessRouteDetailMaterialApp)
        {
            _pikachuApp = pikachuApp;
            _BaseProcessRouteDetailMaterialApp = BaseProcessRouteDetailMaterialApp;
        }

        /// <summary>
        /// 新增工艺路线规则的工序细节的物料细节
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(List<base_process_route_material> obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_BaseProcessRouteDetailMaterialApp.Insert(obj))
                {
                    result.Result = "写入成功";
                }
                else
                {
                    result.Code = 100;
                    result.Message = "数据写入失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 删除工艺路线规则物料细节
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<base_process_route_material>(ids))
                {
                    result.Code = 100;
                    result.Message = "操作失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 修改工艺路线规则物料细节
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_process_route_material obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process_route_material process_Route = _pikachuApp.GetById<base_process_route_material>(obj.process_route_material_id);
                //如果能够根据id找到
                if (process_Route != null)
                {
                    obj.create_time = process_Route.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.process_route_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.process_route_id;

                if (!_pikachuApp.Update(obj))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "更新失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 得到工艺路线规则工序详细信息的列表数据
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_process_route_detail>(req, ref lcount);
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 通过工艺路线id和工序id获取工艺路线物料需求
        /// </summary>
        /// <param name="process_route_id"></param>
        /// <param name="process_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetProcessRouteMaterialByProcessRouteIdAndProcessId(string process_route_id, string process_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                List<ProcessRouteDetailMaterialResponse> data = _BaseProcessRouteDetailMaterialApp.GetProcessRouteMaterialByProcessRouteIdAndProcessId(process_route_id, process_id);

                pd.Data = data;
                lcount = data.Count;
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


    }
}
