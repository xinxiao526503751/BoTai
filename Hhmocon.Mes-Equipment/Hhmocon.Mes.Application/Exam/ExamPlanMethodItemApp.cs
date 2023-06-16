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
    /// 点检计划-点检项目
    /// </summary>
    public class ExamPlanMethodItemApp
    {
        private readonly IExamPlanMethodItemRepository _examPlanMethodItemRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public ExamPlanMethodItemApp(IExamPlanMethodItemRepository examPlanMethodItemRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _examPlanMethodItemRepository = examPlanMethodItemRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 添加点检计划-项目
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertExamPlanMethodItem(List<exam_plan_method_item> data, string equipment_id, string exam_plan_method_id)
        {

            return _examPlanMethodItemRepository.InsertExamPlanMethodItem(data, equipment_id, exam_plan_method_id);
        }

        /// <summary>
        /// 传入点检计划id 和 设备id,在点检计划-点检项表中查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<exam_plan_method_item> GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, List<string> equipment_id)
        {

            //在点检计划关联设备表中查询
            List<exam_plan_method_item> exam_Plan_Method = _examPlanMethodItemRepository.GetItemByPlanIdAndEquipmentId(exam_plan_method_id, equipment_id);
            return exam_Plan_Method;
        }



        public List<exam_plan_method_item> GetItemByPlanId(string exam_plan_method_id)
        {

            //在点检计划关联设备表中查询
            List<exam_plan_method_item> exam_Plan_Method = _examPlanMethodItemRepository.GetItemByPlanId(exam_plan_method_id);
            return exam_Plan_Method;
        }

    }
}
