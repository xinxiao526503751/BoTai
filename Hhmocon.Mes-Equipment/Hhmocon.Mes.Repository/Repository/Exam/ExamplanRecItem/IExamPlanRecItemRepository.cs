using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public interface IExamPlanRecItemRepository
    {
        public List<exam_plan_rec_item> GetExamPlanRecItemByRecId(string examPlanRecId);

    }
}
