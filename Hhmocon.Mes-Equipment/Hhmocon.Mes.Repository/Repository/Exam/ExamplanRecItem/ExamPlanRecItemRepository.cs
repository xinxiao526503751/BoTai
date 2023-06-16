using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public class ExamPlanRecItemRepository : IExamPlanRecItemRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;

        public ExamPlanRecItemRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 获取该点检记录下的点检项目记录
        /// </summary>
        /// <param name="examPlanRecId"></param>
        /// <returns></returns>
        public List<exam_plan_rec_item> GetExamPlanRecItemByRecId(string examPlanRecId)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetExamPlanRecItemByRecId<exam_plan_rec_item>(examPlanRecId);
                List<exam_plan_rec_item> data = _pikachuRepository.GetbySql<exam_plan_rec_item>(sql);

                return data;
            }
        }
    }
}
