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

using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Exam
{
    /// <summary>
    /// 点检计划规则
    /// </summary>
    public class ExamPlanMethodRuleApp
    {
        private readonly IExamPlanMethodRuleRepository _examPlanMethodRuleRepository;
        private readonly PikachuRepository _pikachuRepository;

        public ExamPlanMethodRuleApp(IExamPlanMethodRuleRepository examPlanMethodRuleRepository, PikachuRepository pikachuRepository)
        {
            _examPlanMethodRuleRepository = examPlanMethodRuleRepository;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 添加点检计划规则
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public exam_plan_method_rule InsertExamPlanMethodRule(exam_plan_method_rule data)
        {
            //取ID
            data.exam_plan_method_rule_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新occur_condition
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(exam_plan_method_rule model)
        {
            return _examPlanMethodRuleRepository.UpdateWithField(model, "occur_condition");
        }

        /// <summary>
        /// 通过点检计划id获取点检计划时间规则
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_rule> GetExamPlanMethodRulesByPlanId(string exam_plan_method_id)
        {
            return _examPlanMethodRuleRepository.GetExamPlanMethodRulesByPlanId(exam_plan_method_id);
        }
    }
}
