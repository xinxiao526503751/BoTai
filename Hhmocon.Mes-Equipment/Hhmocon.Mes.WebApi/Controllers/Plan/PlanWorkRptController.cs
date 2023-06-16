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
    /// 生产报工控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Plan", IgnoreApi = false)]
    [ApiController]
    public class PlanWorkRptController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly PlanWorkRptApp _planWorkRptApp;

        public PlanWorkRptController(PikachuApp pikachuApp, PlanWorkRptApp planWorkRptApp)
        {
            _pikachuApp = pikachuApp;
            _planWorkRptApp = planWorkRptApp;
        }

        /// <summary>
        /// 生产报工
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(plan_work_rpt obj)
        {
            Response<string> result = new Response<string>();
            try
            {

                if (_planWorkRptApp.Insert(obj) != null)
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
        /// 更新修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(plan_work_rpt obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work_rpt _plan_work_rpt = _pikachuApp.GetById<plan_work_rpt>(obj.plan_work_rpt_id);
                //如果能够根据id找到
                if (_plan_work_rpt != null)
                {
                    obj.plan_work_rpt_code = _plan_work_rpt.plan_work_rpt_code;//锁死code
                    obj.create_time = _plan_work_rpt.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.plan_work_rpt_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.plan_work_rpt_id;

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



        [HttpPost]
        public Response<PageData> GetListByLocationId(PageReq req, string LocationId)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    List<planWorkRptResponse> data = _planWorkRptApp.QueryPlanWorkRptResponse(req, LocationId);
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
        /// 报工详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        public Response<plan_work_rpt_rn> GetDetail(string id)
        {
            Response<plan_work_rpt_rn> result = new Response<plan_work_rpt_rn>();
            try
            {
                result.Result = _planWorkRptApp.QueryPlanWorkRptRn(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 根据计划工单表对应报工记录表 找出设备列表 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="PlanWorkId"></param>
        /// <returns></returns>

        [HttpPost]
        public Response<PageData> GetEqpByPlanWorkId(PageReq req, string PlanWorkId)
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
                    List<base_equipment> data = _planWorkRptApp.GetEqpByPlanWorkId(PlanWorkId);
                    pd.Data = data?.Skip((iPage - 1) * iRows).Take(iRows);
                    pd.Total = (long)(data?.Count);
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
        /// 报工pro版 处理报工数量 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreatePro(string workID, string equId, string num)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_work_rpt data = _planWorkRptApp.InsertPro(workID, equId, num);
                if (data != null)
                {
                    result.Result = data.plan_work_rpt_id;
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
    }
}
