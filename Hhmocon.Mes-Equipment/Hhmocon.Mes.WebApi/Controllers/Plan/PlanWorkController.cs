using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Plan;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Plan
{
    /// <summary>
    /// 计划工单控制器 任务调度
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Plan", IgnoreApi = false)]
    [ApiController]
    public class PlanWorkController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly PlanWorkApp _planWorkApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="picachuApp"></param>
        /// <param name="planWorkApp"></param>
        public PlanWorkController(PikachuApp pikachuApp, PlanWorkApp planWorkApp)
        {
            _pikachuApp = pikachuApp;
            _planWorkApp = planWorkApp;
        }



        /// <summary>
        /// 排产接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(plan_work obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_planWorkApp.Insert(obj) != null)
                {
                    result.Result = obj.plan_work_id;
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
        /// 工单调度接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> SchedulingOrder(plan_work obj)
        {
            Response<string> result = new Response<string>();
            try
            {

                if (_planWorkApp.InsertScheduling(obj) != null)
                {
                    result.Result = obj.plan_work_id;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "数据name重复，写入失败！";
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
        /// 获取计划工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<plan_work>(req, ref lcount);
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
        /// 获取设备工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="LoactionId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetListByLocationId(PageReq req, string LocationId)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<plan_work>(req, ref lcount);
                pd.Total = lcount;
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    List<planWorkResponse> data = _planWorkApp.QueryPlanWorkResponse(req, LocationId);
                    pd.Data = data != null ? data.Skip((iPage - 1) * iRows).Take(iRows) : null;
                    pd.Total = data != null ? data.Count : 0;
                }
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
        /// 获取设备工单详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<plan_work> GetDetail(string id)
        {
            Response<plan_work> result = new Response<plan_work>();
            try
            {
                result.Result = _pikachuApp.GetById<plan_work>(id);
                //result.Result = _planWorkApp.QueryPlanWorkResponse(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新报工
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(plan_work obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work _plan_work = _pikachuApp.GetById<plan_work>(obj.plan_work_id);
                //如果能够根据id找到
                if (_plan_work != null)
                {
                    obj.plan_work_code = _plan_work.plan_work_code;//锁死code
                    obj.create_time = _plan_work.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                    obj.state_end_time = DateTime.Now;
                    obj.state_start_time = DateTime.Now;
                    obj.report_time = DateTime.Now;
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.plan_work_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.plan_work_id;

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
        /// 启动接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Start(string id)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work _plan_work = _pikachuApp.GetById<plan_work>(id);
                //如果能够根据id找到计划工单
                if (_plan_work != null)
                {
                    _plan_work.create_time = _plan_work.create_time;//锁死创建时间
                    _plan_work.modified_time = Time.Now;
                    _plan_work.state_start_time = Time.Now;
                    _plan_work.work_state = 1;
                }
                else
                { //找不到要返回错误信息
                    result.Result = id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此设备工单信息";
                    return result;
                }
                result.Result = id;

                if (!_pikachuApp.Update(_plan_work))
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
        /// 暂停接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Pause(string id)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work _plan_work = _pikachuApp.GetById<plan_work>(id);
                //如果能够根据id找到设备工单
                if (_plan_work != null)
                {
                    _plan_work.create_time = _plan_work.create_time;//锁死创建时间
                    _plan_work.modified_time = Time.Now;
                    _plan_work.work_state = 0;
                }
                else
                { //找不到要返回错误信息
                    result.Result = id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此设备工单信息";
                    return result;
                }
                result.Result = id;

                if (!_pikachuApp.Update(_plan_work))
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
        /// 完成接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Finish(string id)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work _plan_work = _pikachuApp.GetById<plan_work>(id);
                //如果能够根据id找到设备工单
                if (_plan_work != null)
                {
                    _plan_work.create_time = _plan_work.create_time;//锁死创建时间
                    _plan_work.modified_time = Time.Now;
                    _plan_work.state_end_time = Time.Now;
                    _plan_work.work_state = 9;
                }
                else
                { //找不到要返回错误信息
                    result.Result = id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此设备工单信息";
                    return result;
                }
                result.Result = id;

                if (!_pikachuApp.Update(_plan_work))
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

    }
}
