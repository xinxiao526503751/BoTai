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
    /// 点检计划-点检项表
    /// </summary>
    public interface IExamPlanMethodItemRepository
    {
        /// <summary>
        /// 根据点检计划id和设备id在点检计划-点检项表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_item> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id);

        /// <summary>
        /// 根据点检计划id 获取对应的点检计划项目
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="
        /// 
        /// equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_item> GetItemByPlanId(string exam_plan_method_id);

        /// <summary>
        /// 添加点检计划-项目
        /// </summary>
        /// <param name="data">数组参数data的equipment_id 和 exam_plan_method_id是必传字段，其他内容能填尽量填</param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodItem(List<exam_plan_method_item> data, string equipment_id, string examplan_method_id);

    }
}
