using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository
{
    public class ExamPlanRecRepository : IExamPlanRecRepository, IDependency
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;

        public ExamPlanRecRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 根据epuipemntId获取设备点检保养计划记录数据
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<exam_plan_rec> GetRecByEquipmentId(string equipment_id)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByEquipmentId<exam_plan_rec>(equipment_id);
                List<exam_plan_rec> data = _pikachuRepository.GetbySql<exam_plan_rec>(sql);
                return data;
            }
        }

        /// <summary>
        /// 根据RecId获取点检项目记录对应的点检计划数据
        /// </summary>
        /// <param name="RecId"></param>
        /// <returns></returns>
        public List<exam_plan_rec> GetByRecId(string RecId)
        {
            string sql = _sqlHelper.GetByRecId<exam_plan_rec>(RecId);
            List<exam_plan_rec> data = _pikachuRepository.GetbySql<exam_plan_rec>(sql);
            return data;
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
