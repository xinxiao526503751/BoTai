using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Plan;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Plan
{
    /// <summary>
    /// 计划排产控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Plan", IgnoreApi = false)]
    [ApiController]
    public class PlanProcessController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly PlanProcessApp _planProcessApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="picachuApp"></param>
        /// <param name="planProcessApp"></param>
        public PlanProcessController(PikachuApp picachuApp, PlanProcessApp planProcessApp)
        {
            _pikachuApp = picachuApp;
            _planProcessApp = planProcessApp;
        }


        /// <summary>
        /// 新增计划
        /// 计划是从物料订单增加的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreateByOrder(sale_order_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_planProcessApp.InsertByOreder(obj))
                {
                    result.Result = obj.sale_order_detail_id;
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


        [HttpPost]
        public Response<string> Create(plan_process obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_process data = _planProcessApp.Insert(obj);
                if (data != null)
                {
                    result.Result = data.plan_process_id;
                }
                else
                {
                    //更新失败
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
        /// 删除计划
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

                if (!_pikachuApp.DeleteMask<plan_process>(ids))
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
        /// 更新计划
        /// </summary>
        /// <param name="obj">仓库定义对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(plan_process obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                plan_process plan_Process = _pikachuApp.GetById<plan_process>(obj.plan_process_id);

                if (plan_Process != null)
                {
                    obj.plan_process_code = plan_Process.plan_process_code;//锁死code
                    obj.create_time = plan_Process.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.plan_process_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.plan_process_id;

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
        /// 得到计划列表数据
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
                pd.Data = _planProcessApp.QueryPlanProcessResponse(req, ref lcount);
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
