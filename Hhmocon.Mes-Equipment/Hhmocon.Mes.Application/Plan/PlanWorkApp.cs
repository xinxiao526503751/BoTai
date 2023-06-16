using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Plan
{
    /// <summary>
    /// 计划工单表
    /// </summary>
    public class PlanWorkApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PlanWorkRepository _planWorkRepository;
        private readonly IAuth _auth;

        public PlanWorkApp(PikachuRepository pikachuRepository, PlanWorkRepository planWorkRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _planWorkRepository = planWorkRepository;
            _auth = auth;
        }

        public plan_work Insert(plan_work data)
        {
            //取ID
            data.plan_work_id = CommonHelper.GetNextGUID();
            data.plan_work_code = "p" + Time.Now;
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            data.state_end_time = DateTime.Now;
            data.state_start_time = DateTime.Now;
            data.report_time = DateTime.Now;
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public plan_work InsertScheduling(plan_work data)
        {

            plan_work originalOrder = _pikachuRepository.GetById<plan_work>(data.plan_work_id);
            data.plan_work_code = originalOrder.plan_work_code;
            data.plan_process_id = originalOrder.plan_process_id;
            data.urgency = originalOrder.urgency;
            data.plan_process_id = originalOrder.plan_process_id;

            data.state_end_time = DateTime.Now;
            data.state_start_time = DateTime.Now;
            data.report_time = DateTime.Now;
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            //取ID
            data.plan_work_id = CommonHelper.GetNextGUID();
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
        /// 根据locatiomId多表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<planWorkResponse> QueryPlanWorkResponse(PageReq req, string loactionId)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            //string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;
            if (!string.IsNullOrEmpty(strOrder))
            {
                ordStr = strOrder;
                if (!string.IsNullOrEmpty(strSort))
                {
                    ordStr = "ORDER BY " + ordStr + " " + strSort;
                }
                else
                {
                    ordStr = "ORDER BY " + ordStr + " ";
                }
            }

            return _planWorkRepository.QueryPlanWorkResponse(loactionId, strKey);
        }
    }
}
