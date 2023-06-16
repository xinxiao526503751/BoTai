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
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 点检计划—设备
    /// </summary>
    public interface IExamPlanMethodEquipmentRepository
    {
        /// <summary>
        /// 传入点检计划id  查找点检计划-设备关联表 返回计划-设备关联数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetExamPlanMethodEquipmentByPlanId(string id);

        /// <summary>
        /// 传入设备id和点检计划id 在点检计划-设备表中删除对应的关联
        /// </summary>

        /// <returns></returns>
        public bool Delete(string[] ids);

        /// <summary>
        /// 根据点检计划和设备id在点检计划-设备表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null);


        /// <summary>
        /// 添加点检计划-设备关联
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="method_type"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodEquipment(List<exam_plan_method_equipment> obj, string exam_plan_method_id, string method_type);


    }
}
