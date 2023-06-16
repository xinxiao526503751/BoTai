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
    /// 工艺路线规则
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "ProcessRoute", IgnoreApi = false)]
    [ApiController]
    public class ProcessRouteController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly BaseProcessRouteApp _ProcessRouteApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="planWorkApp"></param>
        public ProcessRouteController(PikachuApp pikachuApp, BaseProcessRouteApp ProcessRouteApp)
        {
            _pikachuApp = pikachuApp;
            _ProcessRouteApp = ProcessRouteApp;
        }

        /// <summary>
        /// 新增工艺路线规则
        /// 同一种物料可以新增多条工艺路线
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_process_route obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_ProcessRouteApp.Insert(obj) != null)
                {
                    result.Result = obj.process_route_id;
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
        /// 删除工艺路线规则
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
                if (ids.Length == 0 || ids == null)
                {
                    throw new Exception("未选中工艺路线");
                }
                _ProcessRouteApp.DeleteProcessRoute(ids);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 修改工艺路线规则
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_process_route obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process_route process_Route = _pikachuApp.GetById<base_process_route>(obj.process_route_id);
                //如果能够根据id找到
                if (process_Route != null)
                {
                    obj.process_route_code = process_Route.process_route_code;//锁死code
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
        /// 得到工艺路线规则列表数据
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
                pd.Data = _pikachuApp.GetList<base_process_route>(req, ref lcount);
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
        /// 根据物料id获取工艺路线
        /// </summary>
        /// <param name="req"></param>
        /// <param name="material_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetProcessRouteByMaterialId(PageReq req, string material_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                int lcount = 0;
                List<ProcessRouteResponse> data = _ProcessRouteApp.GetProcessRouteByMaterialId(req, material_id, ref lcount);
                pd.Data = data;
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
