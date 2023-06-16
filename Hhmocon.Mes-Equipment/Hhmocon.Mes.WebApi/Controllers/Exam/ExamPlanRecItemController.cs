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
    public class ExamPlanRecItemController : Controller
    {
        private readonly ExamPlanRecItemApp _app;
        private readonly ExamPlanRecApp _app2;
        private readonly ExamPlanMethodItemApp _app3;
        private readonly PikachuApp _pikachuApp;
        public ExamPlanRecItemController(ExamPlanRecItemApp app, ExamPlanRecApp app2, ExamPlanMethodItemApp app3, PikachuApp picachuApp)
        {
            _app = app;
            _app2 = app2;
            _app3 = app3;
            _pikachuApp = picachuApp;
        }

        /// <summary>
        /// 创建点检项目记录，只在调试用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreateItem(exam_plan_rec_item obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                exam_plan_rec_item data = _app.InsertExamPlanRecItem(obj);
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
        /// 手动创建点检计划记录与项目
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreateRecAndItem(List<exam_plan_method_item> objList)
        //public Response<string> CreateRecAndItem(string [] exam_plan_method_item_id)
        {
            Response<string> result = new Response<string>();
            try
            {
                // List<exam_plan_method_item> objList = _pikachuApp.GetAllByIds<exam_plan_method_item>(exam_plan_method_item_id);

                exam_plan_method_item objFirst = objList.First();
                //根据传入的exam_plan_method_item类的exam_plan_method_id查找对应的设备点检计划exam_plan_method
                exam_plan_method MethodData = _pikachuApp.GetById<exam_plan_method>(objFirst.exam_plan_method_id);

                #region 逐一给设备点检保养计划记录赋值
                exam_plan_rec RecData = new();
                RecData.exam_plan_code = MethodData.exam_plan_method_code + "sd";
                RecData.exam_plan_name = MethodData.exam_plan_method_name + "sd";
                RecData.exam_method_id = MethodData.exam_method_id;
                RecData.exam_schema_id = MethodData.exam_schema_id;
                RecData.exam_method_type = MethodData.exam_method_type;
                RecData.is_stop_machine = MethodData.is_stop_machine;
                RecData.is_stop_machine = MethodData.is_stop_machine;
                RecData.description = MethodData.description;
                RecData.plan_level = MethodData.plan_level;
                RecData.exam_schema_name = MethodData.exam_schema_name;
                RecData.lead_time = MethodData.lead_time;
                RecData.lead_time_unit = MethodData.lead_time_unit;
                RecData.effect_start_time = MethodData.effect_start_time;
                RecData.effect_end_time = MethodData.effect_end_time;
                RecData.status = MethodData.status;
                RecData.exam_plan_method_id = MethodData.exam_plan_method_id;
                RecData.equipment_id = objFirst.equipment_id;

                #endregion
                exam_plan_rec returnPlanRec = _app2.InsertExamPlanRec(RecData);


                #region 逐一给设备点检保养计划记录项目赋值

                foreach (exam_plan_method_item obj in objList)
                {

                    exam_plan_rec_item RecItemData = new exam_plan_rec_item
                    {
                        exam_plan_rec_id = returnPlanRec.exam_plan_rec_id,
                        examschema_id = obj.examschema_id,
                        examitem_id = obj.examitem_id,
                        examitem_code = obj.examitem_code,
                        examitem_name = obj.examitem_name,
                        examitem_std = obj.examitem_std,
                        value_type = obj.value_type
                    };
                    //RecItemData.result = obj.result;

                    #endregion
                    exam_plan_rec_item returnPlanRecItem = _app.InsertExamPlanRecItem(RecItemData);
                    if (returnPlanRec != null || returnPlanRecItem != null)
                    {
                        result.Result = returnPlanRec.exam_plan_rec_id;
                    }
                    else
                    {
                        //更新失败
                        result.Code = 100;
                        result.Message = "数据写入失败！";
                    }
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
        /// 获取该点检单下的具体点检项目 res传1 获取合格，res传0获取不合格，其他获取所有
        /// </summary>
        /// <param name="RecId"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        public Response<CountData> GetExamPlanRecItemByRecId(string RecId, int res)
        {
            Response<CountData> result = new Response<CountData>();
            CountData cd = new CountData();
            try
            {
                List<exam_plan_rec_item> RecItem = new();
                //合格
                if (res == 1)
                {
                    RecItem = _app.GetExamPlanRecItemByRecId(RecId).Where(a => a.result == 1).ToList();
                }
                //不合格
                if (res == 0)
                {
                    _app.GetExamPlanRecItemByRecId(RecId).Where(a => a.result == 0).ToList();
                }
                //合格加不合格
                else
                {
                    RecItem = _app.GetExamPlanRecItemByRecId(RecId);
                }

                int QualifieNumber = 0, UnqualifieNumber = 0;
                //根据equipment_id查找对应的设备点检计划记录
                cd.Data = RecItem;
                cd.Total = RecItem.Count;
                foreach (exam_plan_rec_item Item in RecItem)
                {
                    if (Item.result == 1)
                    {
                        ++QualifieNumber;
                    }
                    else
                    {
                        ++UnqualifieNumber;
                    }
                }
                cd.QualifieNumber = QualifieNumber;
                cd.UnqualifieNumber = UnqualifieNumber;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            result.Result = cd;
            return result;
        }

        /// <summary>
        /// 置为全部合格接口
        /// 把点检计划以及挂载的项目全部置为合格
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> qualifyAll(string recId)
        {
            Response<string> result = new Response<string>();
            try
            {
                exam_plan_rec rec = _pikachuApp.GetById<exam_plan_rec>(recId);
                rec.result = 1;
                if (!_pikachuApp.Update(rec))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "更新失败！";
                }
                List<exam_plan_rec_item> RecItem = _app.GetExamPlanRecItemByRecId(recId);
                //挂载的项目全部置为合格
                // RecItem.ForEach(a => a.result = 1);
                foreach (exam_plan_rec_item item in RecItem)
                {
                    item.result = 1;
                    _pikachuApp.Update(item);
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
