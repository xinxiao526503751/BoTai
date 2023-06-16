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

namespace Hhmocon.Mes.Repository
{

    /// <summary>
    /// 点检计划仓储
    /// </summary>
    public interface IExamPlanMethodRepository
    {
        public List<exam_plan_method> GetByEquipmentId(string equipment_id);

        public List<exam_plan_method_equipment> GetepmeByEquipmentId(string equipment_id);


        /// <summary>
        /// 同时插入点检计划和点检规则
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rule_data"></param>
        /// <returns></returns>

        public bool InsertExamPlanMethodAndExamPlanMethodRule(exam_plan_method data, List<exam_plan_method_rule> rule_data);
        /// <summary>
        /// 根据传入的点检计划ids在ExamPlanMethod表中删除相应的点检计划，并且删除ExamPlanMethodRule表中挂载了该计划的规则
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// 

        public bool Delete(string[] ids);
    }
}
