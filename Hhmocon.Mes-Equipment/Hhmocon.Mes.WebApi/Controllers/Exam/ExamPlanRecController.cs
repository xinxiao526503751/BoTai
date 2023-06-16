using Hhmocon.Mes.Application;
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
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamPlanRecController : Controller
    {
        private readonly ExamPlanRecApp _app;
        private readonly PikachuApp _pikachuApp;
        public ExamPlanRecController(ExamPlanRecApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }

        /// <summary>
        /// 手动创建点检/保养/维修计划记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(exam_plan_rec obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                exam_plan_rec data = _app.InsertExamPlanRec(obj);

                if (data != null)
                {
                    result.Result = data.exam_plan_rec_id;
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
        /// 获取所有设备下的点检计划记录
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
                pd.Data = _pikachuApp.GetList<exam_plan_rec>(req, ref lcount);
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
        /// 根据EquipmentId获取该设备下的点检/保养/维修计划记录
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetExamPlanRecByEquipmentId(string equipment_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            List<exam_plan_rec> examPlanRec = new();

            try
            {
                //根据equipment_id查找对应的设备点检计划记录
                examPlanRec = equipment_id != null ? _app.GetByEquipmentId(equipment_id)
                                   .Where(c => c.exam_method_type == req.key).ToList() : _pikachuApp.GetAll<exam_plan_rec>()
                                   .Where(c => c.exam_method_type == req.key).ToList();
                pd.Total = examPlanRec.Count;
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    examPlanRec = examPlanRec.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = examPlanRec;

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
