using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface IExamPlanRecRepository
    {
        public List<exam_plan_rec> GetRecByEquipmentId(string equipment_id);

        public List<exam_plan_rec> GetByRecId(string RecId);

    }
}
