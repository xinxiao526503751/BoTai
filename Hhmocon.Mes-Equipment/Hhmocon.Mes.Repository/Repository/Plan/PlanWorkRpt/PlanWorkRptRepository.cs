using Dapper;
using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository
{
   public class PlanWorkRptRepository:IPlanWorkRptRepository,IDependency
    {
        private SqlHelper _sqlHelper;
        private PikachuRepository _pikachuRepository;

        public PlanWorkRptRepository(SqlHelper sqlHelper, PikachuRepository pikachuRepository)
        {
            _sqlHelper = sqlHelper;
            _pikachuRepository = pikachuRepository;
        }

        public List<planWorkRptResponse> QueryPlanWorkRptResponse(string id, string whereStr)
        {
            using var conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                if (!string.IsNullOrEmpty(id)) //特定地点的搜索
                    query = "SELECT pwr.*,be.equipment_name,be.equipment_code,bm.material_name,bp.process_name " +
                         "FROM plan_work_rpt pwr LEFT JOIN  base_equipment be  ON pwr.equipment_id = be.equipment_id"+
                           "LEFT JOIN  base_material bm  ON pwr.material_id = bm.material_id"+
                          "LEFT JOIN  base_process bp  ON pwr.process_id = bp.process_id WHERE pwr.location_id = @id " + whereStr;
                //全部地点的搜索
                else if (!string.IsNullOrEmpty(whereStr)) query = "SELECT pwr.*,be.equipment_name,be.equipment_code,bm.material_name,bp.process_name " +
                         "FROM plan_work_rpt pwr LEFT JOIN  base_equipment be  ON pwr.equipment_id = be.equipment_id" +
                           "LEFT JOIN  base_material bm  ON pwr.material_id = bm.material_id" +
                          "LEFT JOIN  base_process bp  ON pwr.process_id = bp.process_id" + whereStr.Remove(0, 3);

                //没有搜索返回所有
                else query = "SELECT pwr.*,be.equipment_name,be.equipment_code,bm.material_name,bp.process_name " +
                         "FROM plan_work_rpt pwr LEFT JOIN  base_equipment be  ON pwr.equipment_id = be.equipment_id" +
                           "LEFT JOIN  base_material bm  ON pwr.material_id = bm.material_id" +
                          "LEFT JOIN  base_process bp  ON pwr.process_id = bp.process_id";

                List<planWorkRptResponse> _planWorkRptResponseList = new();
                var b = conn.Query<plan_work_rpt, base_equipment, base_material, base_process,List<planWorkRptResponse>>(query,
                    (planWorkRpt, baseEquipment, baseMterial, baseProcess) =>
                    {
                        planWorkRptResponse _planWorkRptResponse = new();

                        if (planWorkRpt != null)
                        {

                            _planWorkRptResponse.plan_work_rpt_id = planWorkRpt.plan_work_rpt_id;
                            _planWorkRptResponse.plan_num = planWorkRpt.plan_num;
                            _planWorkRptResponse.da_num = planWorkRpt.da_num;
                            _planWorkRptResponse.equipment_id = planWorkRpt.equipment_id;
                            _planWorkRptResponse.plan_num = planWorkRpt.plan_num;
                        }
                        if (baseProcess != null)
                        {
                            _planWorkRptResponse.process_name = baseProcess.process_name;
                        }
                        if (baseMterial != null)
                        {
                            _planWorkRptResponse.material_name = baseMterial.material_name;
                        }
                        if (baseEquipment != null)
                        {
                            _planWorkRptResponse.equipment_name = baseEquipment.equipment_name;
                            _planWorkRptResponse.equipment_code = baseEquipment.equipment_code;
                        }
                        _planWorkRptResponseList.Add(_planWorkRptResponse);
                        return _planWorkRptResponseList;
                    }, new { id }, splitOn: "process_id,location_id,equipment_name,material_name,process_name").Distinct().SingleOrDefault();
                return b;
            }
        }

        public plan_work_rpt_rn QueryPlanWorkRptRn(string id)
        {
            using var conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                query = "SELECT pw.*,bm.* " +
                         "FROM plan_work pw LEFT JOIN  base_material bm  ON pw.material_id = bm.material_id" +
                         " WHERE pw.plan_work_id = @id ";

               // plan_work_rpt_rn _planWorkRptResponseRn = new();
                var b = conn.Query<plan_work, base_material, plan_work_rpt_rn>(query,
                    (planWorkRpt, baseMterial) =>
                    {
                        plan_work_rpt_rn _planWorkRptResponseRn = new();

                        if (planWorkRpt != null)
                        {
                            _planWorkRptResponseRn.plan_num = planWorkRpt.plan_num;
                            int completed_num_before = 0;
                            List<plan_work_rpt> plan_Work_Rpt_before = _pikachuRepository.GetAll<plan_work_rpt>().Where(a=>a.plan_work_id == planWorkRpt.plan_work_id).ToList();
                            if (plan_Work_Rpt_before.Count == 0) _planWorkRptResponseRn.completed_num = 0;
                            else {
                                foreach (var item in plan_Work_Rpt_before)
                                {
                                    completed_num_before += item.da_num;
                                }

                                _planWorkRptResponseRn.completed_num = completed_num_before;
                        
                            } ;



                        }
                        if (baseMterial != null)
                        {
                            _planWorkRptResponseRn.material_name = baseMterial.material_name;
                            _planWorkRptResponseRn.material_code = baseMterial.material_code;
                            _planWorkRptResponseRn.material_spec = baseMterial.material_spec;
                        }                     
                     
                   
                        return _planWorkRptResponseRn;
                    }, new { id }, splitOn: "material_id").Distinct().SingleOrDefault();
                return b;
            }
        }

    }
}
