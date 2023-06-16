using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Exam
{
    public class ExamPlanRecApp
    {
        private readonly IExamPlanRecRepository _examPlanRecRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public ExamPlanRecApp(IExamPlanRecRepository examPlanRecRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _examPlanRecRepository = examPlanRecRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public exam_plan_rec InsertExamPlanRec(exam_plan_rec data)
        {
            //取ID
            data.exam_plan_rec_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.equipment_name = _pikachuRepository.GetById<base_equipment>(data.equipment_id)?.equipment_name;
            //data.create_by = _auth.GetUserAccount(null);
            //data.create_by_name = _auth.GetUserName(null);
            //data.modified_by = _auth.GetUserAccount(null);
            //data.modified_by_name = _auth.GetUserName(null);
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
        /// 根据EquipmentId获取数据
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_rec> GetByEquipmentId(string equipment_id)
        {
            List<exam_plan_rec> data = _examPlanRecRepository.GetRecByEquipmentId(equipment_id);
            return data;
        }

        public List<exam_plan_rec> GetByRecId(string RecId)
        {
            List<exam_plan_rec> data = _examPlanRecRepository.GetByRecId(RecId);
            return data;
        }

    }
}
