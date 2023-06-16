using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Exam
{
    public class ExamPlanRecItemApp
    {
        private readonly IExamPlanRecItemRepository _examPlanRecItemRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public ExamPlanRecItemApp(IExamPlanRecItemRepository examPlanRecItemRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _examPlanRecItemRepository = examPlanRecItemRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 根据examPlanRecId获取具体项目记录
        /// </summary>
        /// <param name="examPlanRecId"></param>
        /// <returns></returns>
        public List<exam_plan_rec_item> GetExamPlanRecItemByRecId(string examPlanRecId)
        {
            List<exam_plan_rec_item> data = _examPlanRecItemRepository.GetExamPlanRecItemByRecId(examPlanRecId);
            return data;

        }

        /// <summary>
        /// 调试用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public exam_plan_rec_item InsertExamPlanRecItem(exam_plan_rec_item data)
        {
            //取ID
            data.exam_plan_rec_item_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
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

    }
}
