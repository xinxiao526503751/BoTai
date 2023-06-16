using Dapper;
using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 计划工单仓储
    /// </summary>
    public class PlanWorkRepository : IPlanWorkRepository, IDependency
    {

        private readonly SqlHelper _sqlHelper;
        private readonly PikachuRepository _pikachuRepository;

        public PlanWorkRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }
        /// <summary>
        ///  多表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<planWorkResponse> QueryPlanWorkResponse(string id, string whereStr)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                if (!string.IsNullOrEmpty(id)) //特定地点的搜索
                {
                    query = "SELECT pw.* FROM  plan_work pw LEFT JOIN plan_process pp ON pp.plan_process_id = pw.plan_process_id WHERE pw.location_id = @id " + whereStr;
                }
                //全部地点的搜索
                else if (!string.IsNullOrEmpty(whereStr))
                {
                    query = "SELECT pw.* FROM  plan_work pw INNER JOIN plan_process pp ON pp.plan_process_id = pw.plan_process_id WHERE " + whereStr.Remove(0, 3);
                }

                //没有搜索返回所有
                else
                {
                    query = "SELECT pw.* FROM  plan_work pw INNER JOIN plan_process pp ON pp.plan_process_id = pw.plan_process_id ";
                }

                List<planWorkResponse> _planWorkResponseList = new();
                List<planWorkResponse> b = conn.Query<plan_work, plan_process, List<planWorkResponse>>(query,
                    (planWork, planProcess) =>
                    {
                        planWorkResponse _planWorkResponse = new();

                        if (planWork != null)
                        {

                            _planWorkResponse.plan_work_id = planWork.plan_work_id;
                            _planWorkResponse.plan_work_code = planWork.plan_work_code;
                            _planWorkResponse.da_num = planWork.da_num;
                            _planWorkResponse.debug_num = planWork.debug_num;
                            _planWorkResponse.end_time = planWork.end_time;
                            _planWorkResponse.equipment_id = planWork.equipment_id;
                            _planWorkResponse.plan_num = planWork.plan_num;
                            _planWorkResponse.plan_process_id = planWork.plan_process_id;
                            _planWorkResponse.process_id = planWork.process_id;
                            _planWorkResponse.plan_state = planWork.plan_state;
                            _planWorkResponse.work_state = planWork.work_state;
                            _planWorkResponse.start_time = planWork.start_time;
                            _planWorkResponse.plan_work_code = planWork.plan_work_code;
                            _planWorkResponse.max_num = planWork.max_num;
                            _planWorkResponse.material_id = planWork.material_id;
                        }
                        //if (planProcess != null)
                        //{
                        //    _planWorkResponse.location_id = planProcess.location_id;
                        //}
                        _planWorkResponse.equipment_name = _planWorkResponse.equipment_id != null
                        ? _pikachuRepository.GetById<base_equipment>(_planWorkResponse.equipment_id)?.equipment_name : null;

                        _planWorkResponse.equipment_code = _planWorkResponse.equipment_id != null
                       ? _pikachuRepository.GetById<base_equipment>(_planWorkResponse.equipment_id)?.equipment_code : null;

                        _planWorkResponse.material_name = _planWorkResponse.material_id != null
                        ? _pikachuRepository.GetById<base_material>(_planWorkResponse.material_id)?.material_name : null;

                        _planWorkResponse.process_name = _planWorkResponse.process_id != null
                        ? _pikachuRepository.GetById<base_process>(_planWorkResponse.process_id)?.process_name : null;

                        _planWorkResponseList.Add(_planWorkResponse);
                        return _planWorkResponseList;
                    }, new { id }, splitOn: "modified_by_name").Distinct().SingleOrDefault();//, splitOn: "plan_work_id"
                return b;
            }
        }
    }
}

