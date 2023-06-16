using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Exam;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Exam
{
    /// <summary>
    /// 点检计划-设备关联表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamPlanMethodEquipmentController : ControllerBase
    {
        private readonly ExamPlanMethodEquipmentApp _app;
        private readonly PikachuApp _pikachuApp;

        public ExamPlanMethodEquipmentController(ExamPlanMethodEquipmentApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }

        /// <summary>
        /// 新增点检计划-设备关联/保养计划-设备关联
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="method_type"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(List<exam_plan_method_equipment> obj, string exam_plan_method_id, string method_type)
        {
            Response<string> result = new Response<string>();
            try
            {
                bool data = _app.InsertExamPlanMethodEquipment(obj, exam_plan_method_id, method_type);
                if (data == true)
                {
                    result.Result = "操作成功";
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
        /// 传入点检计划id获取 点检计划-设备 关联信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<exam_plan_method_equipment>> GetExamPlanMethodEquipmentByPlanId(string id)
        {
            Response<List<exam_plan_method_equipment>> result = new Response<List<exam_plan_method_equipment>>();
            try
            {
                //在点检计划关联设备表中查询
                List<exam_plan_method_equipment> exam_Plan_Method = _app.GetExamPlanMethodEquipmentByPlanId(id).OrderBy(c => c.create_time).ToList();
                result.Result = exam_Plan_Method;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 传入点检计划-设备关联信息id  删除对应的关联
        /// 同时删除计划-点检项目中对应的关联
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] id)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = id.ToArray();


                _app.Delete(id);

                if (!_pikachuApp.DeleteMask<exam_plan_method_equipment>(id))
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



    }
}
