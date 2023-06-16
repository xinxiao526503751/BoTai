using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.ProcessRoute
{
    /// <summary>
    /// 工艺路线规则的工序详情
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "ProcessRoute", IgnoreApi = false)]
    [ApiController]
    public class ProcessRouteDetailController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly BaseProcessRouteDetailApp _ProcessRouteDetailApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="ProcessRouteDetailApp"></param>
        public ProcessRouteDetailController(PikachuApp pikachuApp, BaseProcessRouteDetailApp ProcessRouteDetailApp)
        {
            _pikachuApp = pikachuApp;
            _ProcessRouteDetailApp = ProcessRouteDetailApp;
        }

        /// <summary>
        /// 新增工艺路线规则
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_process_route_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_ProcessRouteDetailApp.Insert(obj))
                {
                    result.Result = obj.process_route_detail_id;
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

                if (!_ProcessRouteDetailApp.Delete(ids))
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
        /// 修改工艺路线规则工艺详细信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_process_route_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process_route_detail process_Route = _pikachuApp.GetById<base_process_route_detail>(obj.process_route_detail_id);
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
        /// 根据工艺路线id获取工艺路线工序细节
        /// </summary>
        /// <param name="process_route_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetProcessRouteDetailByProcessRouteId(string process_route_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                List<ProcessRouteDetailResponse> data = _ProcessRouteDetailApp.GetProcessRouteDetailByProcessRouteId(process_route_id);
                data = data.OrderBy(c => c.process_seq).ToList();
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

        /// <summary>
        /// 根据MatertialId获取工艺路线详情数据，不分页
        /// </summary>
        /// <param name="material_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<ProcessRouteDetailResponse>> GetProcessRouteDetailByMaterialId(string material_id)
        {
            Response<List<ProcessRouteDetailResponse>> result = new Response<List<ProcessRouteDetailResponse>>();
            try
            {
                List<ProcessRouteDetailResponse> data = _ProcessRouteDetailApp.GetProcessRouteDetailByMaterialId(material_id);
                result.Result = data;
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
