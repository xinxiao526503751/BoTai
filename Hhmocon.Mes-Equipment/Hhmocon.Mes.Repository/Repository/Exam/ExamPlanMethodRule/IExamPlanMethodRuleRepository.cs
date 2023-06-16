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

using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Repository
{
    /// <summary>
    /// 点检计划时间规则
    /// </summary>
    public interface IExamPlanMethodRuleRepository
    {
        /// <summary>
        /// 根据exam_plan_method_id在点检规则表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_rule> GetExamPlanMethodRulesByPlanId(string exam_plan_method_id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public int UpdateWithField(exam_plan_method_rule model, string field);

    }



}
