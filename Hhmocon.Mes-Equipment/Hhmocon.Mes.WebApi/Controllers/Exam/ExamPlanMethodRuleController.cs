using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Exam;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Exam
{
    /// <summary>
    /// 设备-点检项目关联表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamPlanMethodRuleController
    {
        private readonly ExamPlanMethodRuleApp _app;
        private readonly PikachuApp _pikachuApp;


        public ExamPlanMethodRuleController(ExamPlanMethodRuleApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }


        /// <summary>
        /// 删除点检计划-时间
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

                if (!_pikachuApp.DeleteMask<exam_plan_method_rule>(ids))
                {
                    //更新失败
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
        /// 更改点检规则
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(exam_plan_method_rule obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                result.Result = obj.exam_plan_method_rule_id;

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
        /// 根据点检计划Id获取点检计划下挂载的时间规则
        /// </summary>
        /// <param name="exam_plan_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetExamPlanMethodRuleByExamPlanId(string exam_plan_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                List<exam_plan_method_rule> exam_Plan_Method_Rules = _app.GetExamPlanMethodRulesByPlanId(exam_plan_id);
                if (exam_Plan_Method_Rules != null)
                {
                    lcount = exam_Plan_Method_Rules.Count;
                }

                pd.Data = exam_Plan_Method_Rules;
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
