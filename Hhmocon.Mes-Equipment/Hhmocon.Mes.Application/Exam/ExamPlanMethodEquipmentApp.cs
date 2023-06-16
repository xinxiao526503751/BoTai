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
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Exam
{
    /// <summary>
    /// 点检计划-关联设备
    /// </summary>
    public class ExamPlanMethodEquipmentApp
    {
        private readonly IExamPlanMethodEquipmentRepository _examPlanMethodEuipmentRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public ExamPlanMethodEquipmentApp(IExamPlanMethodEquipmentRepository examPlanMethodEquipmentRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _examPlanMethodEuipmentRepository = examPlanMethodEquipmentRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加点检计划-设备关联
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodEquipment(List<exam_plan_method_equipment> obj, string exam_plan_method_id, string method_type)
        {
            return _examPlanMethodEuipmentRepository.InsertExamPlanMethodEquipment(obj, exam_plan_method_id, method_type);
        }

        /// <summary>
        ///  传入点检计划id 查找点检计划-设备表 返回关联数据 前端拿到计划下的设备相关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetExamPlanMethodEquipmentByPlanId(string id)
        {
            return _examPlanMethodEuipmentRepository.GetExamPlanMethodEquipmentByPlanId(id);
        }

        /// <summary>
        /// 删除数据 假删除 传入设备id和点检计划id  在点检计划-设备表 删除关联
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            return _examPlanMethodEuipmentRepository.Delete(ids);
        }

        /// <summary>
        /// 根据点检计划id和设备id在点检计划-设备关联表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_equipment> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id)
        {

            return _examPlanMethodEuipmentRepository.GetItemByPlanIdAndEquipmentId(exam_plan_method_id, equipment_id);
        }

    }
}
