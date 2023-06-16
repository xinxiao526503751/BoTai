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

using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository.Repository
{
    public class ExamPlanMethodRuleRepository : IExamPlanMethodRuleRepository, IDependency
    {
        /// <summary>
        /// 根据exam_plan_method_id在点检规则表中查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <returns></returns>
        public List<exam_plan_method_rule> GetExamPlanMethodRulesByPlanId(string exam_plan_method_id)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<string> id = new() { exam_plan_method_id };
                List<exam_plan_method_rule> data = conn.GetByIdsWithField<exam_plan_method_rule>("exam_plan_method_id", id).ToList();
                return data;
            }
        }

        /// <summary>
        /// 更改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public int UpdateWithField(exam_plan_method_rule model, string field)
        {
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {

                int result = conn.Update(model, field);
                return result;
            }
        }

    }
}
