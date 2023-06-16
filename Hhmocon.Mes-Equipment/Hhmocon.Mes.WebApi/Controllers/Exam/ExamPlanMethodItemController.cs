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
    /// 点检计划-点检项关联表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamPlanMethodItemController : ControllerBase
    {
        private readonly ExamPlanMethodItemApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public ExamPlanMethodItemController(ExamPlanMethodItemApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }

        /// <summary>
        /// 新增设备-点检项关联
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="equipment_id"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(List<exam_plan_method_item> obj, string equipment_id, string exam_plan_method_id)
        {
            Response<string> result = new Response<string>();
            try
            {
                bool data = _app.InsertExamPlanMethodItem(obj, equipment_id, exam_plan_method_id);
                if (data)
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
        /// 传入点检计划和设备id 在计划-项目表的数据中 获取计划-点检项目关联  前端获取计划中设备下的点检项信息
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<exam_plan_method_item>> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, string equipment_id)
        {
            Response<List<exam_plan_method_item>> result = new Response<List<exam_plan_method_item>>();
            try
            {
                List<string> data_equipment_id = new();
                data_equipment_id.Add(equipment_id);
                //在点检计划关联设备表中查询
                List<exam_plan_method_item> exam_Plan_Method = _app.GetItemByPlanIdAndEquipmentId(exam_plan_method_id, data_equipment_id);

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
        /// 传入设备点检计划id，获取对应的设备点检计划项目
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<exam_plan_method_item>> GetItemByPlanId(string exam_plan_method_id)
        {
            Response<List<exam_plan_method_item>> result = new Response<List<exam_plan_method_item>>();
            try
            {
                //在点检计划关联设备表中查询
                List<exam_plan_method_item> exam_Plan_Method = _app.GetItemByPlanId(exam_plan_method_id);

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
        /// 传入点检计划-点检项目关联信息id  删除对应的关联
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

                if (!_pikachuApp.DeleteMask<exam_plan_method_item>(id))
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
