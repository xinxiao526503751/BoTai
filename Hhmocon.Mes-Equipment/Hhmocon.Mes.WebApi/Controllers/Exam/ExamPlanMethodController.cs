using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Exam
{
    /// <summary>
    /// 点检计划-点检时间
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamPlanMethodController : ControllerBase
    {
        private readonly ExamPlanMethodApp _app;
        private readonly PikachuApp _pikachuApp;


        public ExamPlanMethodController(ExamPlanMethodApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }


        /// <summary>
        /// 添加点检计划,和点检规则
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(List<object> all)
        {
            Response<string> result = new Response<string>();
            try
            {
                JObject a = JObject.Parse(all[0].ToString());
                exam_plan_method t = a.ToObject<exam_plan_method>();
                JArray jArray = all[1] as JArray;
                for (int i = 0; i < jArray.Count; i++)
                {
                    if (jArray[i]["trigger_time"].IsEmpty())//前端如果没给时间，要把时间默认值给上
                    {
                        jArray[i]["trigger_time"] = Time.ToDate("1753-1-1 00:00:00");
                    }
                }
                exam_plan_method_rule[] ii = JsonConvert.DeserializeObject<exam_plan_method_rule[]>(jArray.ToString());

                List<exam_plan_method_rule> rules = ii.ToList();

                //不允许计划的开始时间小于结束时间
                if (t.effect_start_time > t.effect_end_time)
                {
                    throw new Exception("不允许计划的开始时间小于结束时间");
                }

                //检查是否有三要素重复的点检规则
                foreach (exam_plan_method_rule temp_rule in rules)
                {
                    List<exam_plan_method_rule> rules2 = new(rules);
                    rules2.Remove(temp_rule);
                    foreach (exam_plan_method_rule temp_rule2 in rules2)
                    {
                        if (temp_rule.trigger_time == temp_rule2.trigger_time && temp_rule.unit_mode == temp_rule2.unit_mode && temp_rule.occur_condition == temp_rule2.occur_condition)
                        {
                            throw new Exception("不允许重复添加相同的规则");
                        }
                    }
                }

                List<exam_plan_method> getbycodes = _pikachuApp.GetByOneFeildsSql<exam_plan_method>("exam_plan_method_code", t.exam_plan_method_code);
                List<exam_plan_method> getbynames = _pikachuApp.GetByOneFeildsSql<exam_plan_method>("exam_plan_method_name", t.exam_plan_method_name);
                exam_plan_method getbycode = getbycodes.Where(c => c.exam_method_type == t.exam_method_type).FirstOrDefault();
                exam_plan_method getbyname = getbynames.Where(c => c.exam_method_type == t.exam_method_type).FirstOrDefault();

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，点检或保养中已存在有该名称={0}或编码={1}的数据，请检查并重新填写", t.exam_plan_method_name, t.exam_plan_method_code);
                    return result;
                }

                bool result_deal = _app.InsertExamPlanMethodAndExamPlanMethodRule(t, rules);

                if (result_deal)
                {
                    result.Result = t.exam_method_id;
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
        /// 删除点检计划
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

                if (!_app.Delete(ids))
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
        /// 更改点检计划/更改维修/保养计划
        /// 参数同create
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(List<object> all)
        {
            Response<string> result = new Response<string>();
            try
            {
                _app.Update(all, ref result);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 点检计划分页/保养计划分页
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
                pd.Data = _pikachuApp.GetList<exam_plan_method>(req, ref lcount);
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
        ///  根据ID得到点检计划明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<exam_plan_method> GetDetail(string id)
        {
            Response<exam_plan_method> result = new Response<exam_plan_method>();
            try
            {
                result.Result = _pikachuApp.GetById<exam_plan_method>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取所有点检计划/保养计划
        /// </summary>
        /// <param name="exam_method_type">"1"点检   "2"保养 "3"保养</param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<exam_plan_method>> GetAll(string exam_method_type)
        {
            Response<List<exam_plan_method>> result = new Response<List<exam_plan_method>>();
            try
            {
                result.Result = _pikachuApp.GetAll<exam_plan_method>().Where(c => c.exam_method_type == exam_method_type).ToList();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 根据EquipmentId获取该设备下的点检/保养/维修计划
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <param name="req">req="1"点检  "2"保养 "3"维修</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetExamPlanMethodByEquipmentId(string equipment_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            List<exam_plan_method> examPlan = new();

            try
            {
                //根据equipment_id查找对应的设备点检计划
                List<exam_plan_method_equipment> examPlanMethodEq = _app.GetepmeByEquipmentId(equipment_id).Where(c => c.method_type == req.key).ToList();
                foreach (exam_plan_method_equipment temp in examPlanMethodEq)
                {
                    examPlan.Add(_pikachuApp.GetById<exam_plan_method>(temp.exam_plan_method_id));
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            if (req != null)
            {
                int iPage = req.page;
                int iRows = req.rows;
                //分页
                examPlan = examPlan.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            pd.Data = examPlan;
            pd.Total = examPlan.Count;
            result.Result = pd;
            return result;
        }

    }
}
