/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 点检计划
    /// </summary>
    public class ExamPlanMethodApp
    {
        private readonly IExamPlanMethodRepository _examPlanMethodRepository;
        private readonly IAuth _auth;
        private readonly PikachuRepository _pikachuRepository;

        public ExamPlanMethodApp(IExamPlanMethodRepository examPlanMethodRepository, IAuth auth, PikachuRepository pikachuRepository)
        {
            _examPlanMethodRepository = examPlanMethodRepository;
            _auth = auth;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 添加点检计划和点检规则
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodAndExamPlanMethodRule(exam_plan_method data, List<exam_plan_method_rule> rules)
        {
            //取ID
            data.exam_plan_method_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);

            foreach (exam_plan_method_rule item in rules)
            {
                item.exam_plan_method_rule_id = CommonHelper.GetNextGUID();
                item.modified_time = Time.Now;
                item.create_time = DateTime.Now;
                item.exam_plan_method_id = data.exam_plan_method_id;//将计划和规则绑定
                item.create_by = _auth.GetUserAccount(null);
                item.create_by_name = _auth.GetUserName(null);
                item.modified_by = _auth.GetUserAccount(null);
                item.modified_by_name = _auth.GetUserName(null);
            }

            bool result = _examPlanMethodRepository.InsertExamPlanMethodAndExamPlanMethodRule(data, rules);

            return result;
        }

        /// <summary>
        /// 根据EquipmentId获取数据
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method> GetByEquipmentId(string equipment_id)
        {
            List<exam_plan_method> data = _examPlanMethodRepository.GetByEquipmentId(equipment_id);
            return data;
        }

        public List<exam_plan_method_equipment> GetepmeByEquipmentId(string equipment_id)
        {
            List<exam_plan_method_equipment> data = _examPlanMethodRepository.GetepmeByEquipmentId(equipment_id);
            return data;
        }
        /// 根据传入的点检计划ids在ExamPlanMethod表中删除相应的点检计划，并且删除ExamPlanMethodRule表中挂载了该计划的规则
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            bool result = _examPlanMethodRepository.Delete(ids);
            return result;
        }

        /// <summary>
        /// 更新点检计划，同时更新点检计划规则
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public Response<string> Update(List<object> all, ref Response<string> result)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    //参数解析
                    JObject a = JObject.Parse(all[0].ToString());

                    //获取前端更新的点检计划
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

                    //获取前端更新的点检规则
                    List<exam_plan_method_rule> rules = ii.ToList();

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
                    //写入点检计划对应的新规则
                    foreach (exam_plan_method_rule exam_Plan_Method_Rule_temp in rules)
                    {

                        if (exam_Plan_Method_Rule_temp.exam_plan_method_rule_id != null)
                        {
                            continue;
                        }
                        //给新的计划赋予id
                        exam_Plan_Method_Rule_temp.exam_plan_method_rule_id = CommonHelper.GetNextGUID();
                        exam_Plan_Method_Rule_temp.exam_plan_method_id = t.exam_plan_method_id;
                        //更新创建时间和时间
                        exam_Plan_Method_Rule_temp.create_time = Time.Now;
                        exam_Plan_Method_Rule_temp.modified_time = Time.Now;
                        exam_Plan_Method_Rule_temp.modified_by = _auth.GetUserName();
                        exam_Plan_Method_Rule_temp.create_by = _auth.GetUserName();
                        exam_Plan_Method_Rule_temp.rule_type = t.exam_method_type;
                        _pikachuRepository.Insert(exam_Plan_Method_Rule_temp, tran: transaction, dbConnection: conn);
                    }


                    exam_plan_method _Exam_plan_method = _pikachuRepository.GetById<exam_plan_method>(t.exam_plan_method_id, tran: transaction, dbConnection: conn);

                    result.Result = t.exam_plan_method_id;
                    //如果能够根据id找到
                    if (_Exam_plan_method != null)
                    {
                        t.exam_plan_method_code = _Exam_plan_method.exam_plan_method_code;//锁死code
                        t.create_time = _Exam_plan_method.create_time;//锁死创建时间
                        t.modified_time = Time.Now;
                        //更新点检计划
                        _pikachuRepository.Update(t);
                    }
                    else
                    { //找不到要返回错误信息
                        result.Code = 100;
                        result.Message = "更新失败！没有此id信息";
                    }


                    transaction.Commit();
                    return result;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception(exception.Message);
                }
            }
        }
    }
}
