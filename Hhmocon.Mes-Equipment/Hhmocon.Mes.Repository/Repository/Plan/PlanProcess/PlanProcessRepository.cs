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

using Dapper;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 计划仓储
    /// </summary>
    public class PlanProcessRepository : IPlanProcessRepository, IDependency
    {

        public List<plan_process_rep> QueryPlanProcessResponse(string whereStr)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                string query = null;
                if (!string.IsNullOrEmpty(whereStr))
                {
                    query = "SELECT pp.*,bm.material_name,bm.material_code,bp.process_name " +
                         "FROM plan_process pp LEFT JOIN  base_material bm  ON pp.material_id = bm.material_id" +
                          " LEFT JOIN  base_process bp  ON pp.process_id = bp.process_id where pp.delete_mark=0 and" + whereStr;
                }

                query = "SELECT pp.*,bm.material_name,bm.material_code,bp.process_name " +
                         "FROM plan_process pp LEFT JOIN  base_material bm  ON pp.material_id = bm.material_id" +
                          " LEFT JOIN  base_process bp  ON pp.process_id = bp.process_id where pp.delete_mark=0";
                List<plan_process_rep> _planProcessResponseList = new();
                List<plan_process_rep> b = conn.Query<plan_process, base_material, base_process, List<plan_process_rep>>(query,
                    (planProcess, baseMterial, baseProcess) =>
                    {
                        plan_process_rep plan_Process_Rep = new();

                        if (planProcess != null)
                        {

                            plan_Process_Rep.create_by = planProcess.create_by;
                            plan_Process_Rep.create_by_name = planProcess.create_by_name;
                            plan_Process_Rep.create_time = planProcess.create_time;
                            plan_Process_Rep.delete_mark = planProcess.delete_mark;
                            plan_Process_Rep.end_time = planProcess.end_time;
                            plan_Process_Rep.location_id = planProcess.location_id;
                            plan_Process_Rep.material_id = planProcess.material_id;
                            plan_Process_Rep.modified_by = planProcess.modified_by;
                            plan_Process_Rep.modified_by_name = planProcess.modified_by_name;
                            plan_Process_Rep.modified_time = planProcess.modified_time;
                            plan_Process_Rep.plan_num = planProcess.plan_num;
                            plan_Process_Rep.plan_process_code = planProcess.plan_process_code;
                            plan_Process_Rep.plan_process_id = planProcess.plan_process_id;
                            plan_Process_Rep.sort_num = planProcess.sort_num;
                            plan_Process_Rep.start_time = planProcess.start_time;
                            plan_Process_Rep.process_id = planProcess.process_id;



                        }
                        if (baseProcess != null)
                        {
                            plan_Process_Rep.process_name = baseProcess.process_name;
                        }
                        if (baseMterial != null)
                        {
                            plan_Process_Rep.material_name = baseMterial.material_name;
                            plan_Process_Rep.material_code = baseMterial.material_code;
                        }

                        _planProcessResponseList.Add(plan_Process_Rep);
                        return _planProcessResponseList;
                    }, splitOn: "process_id,material_name,process_name").Distinct().SingleOrDefault();
                return b;
            }
        }
    }



}
