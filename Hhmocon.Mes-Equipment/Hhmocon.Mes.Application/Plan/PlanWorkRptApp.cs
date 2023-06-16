using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Plan
{
    public class PlanWorkRptApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PlanWorkRptRepository _planWorkRptRepository;
        private readonly IAuth _auth;

        public PlanWorkRptApp(PikachuRepository pikachuRepository, PlanWorkRptRepository planWorkRptRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _planWorkRptRepository = planWorkRptRepository;
            _auth = auth;
        }

        public plan_work_rpt Insert(plan_work_rpt data)
        {
            //取ID
            data.plan_work_rpt_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.report_date = Time.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public plan_work_rpt InsertPro(string workID, string equId, string num)
        {
            plan_work_rpt _Rpt = new();
            plan_work _Work = _pikachuRepository.GetById<plan_work>(workID);
            //取ID
            _Rpt.plan_work_rpt_id = CommonHelper.GetNextGUID();
            _Rpt.modified_time = Time.Now;
            _Rpt.create_time = DateTime.Now;
            _Rpt.report_date = Time.Now;
            _Rpt.create_by = _auth.GetUserAccount(null);
            _Rpt.create_by_name = _auth.GetUserName(null);
            _Rpt.modified_by = _auth.GetUserAccount(null);
            _Rpt.modified_by_name = _auth.GetUserName(null);
            _Rpt.equipment_id = equId;
            _Rpt.material_id = _Work.material_id;
            _Rpt.plan_num = _Work.plan_num;
            _Rpt.reject_num = num.Split("/")[0].ToInt();
            _Rpt.da_num = num.Split("/")[0].ToInt() + num.Split("/")[1].ToInt();
            if (_pikachuRepository.Insert(_Rpt))
            {
                return _Rpt;
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
        public List<planWorkRptResponse> QueryPlanWorkRptResponse(PageReq req, string loactionId)
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

            return _planWorkRptRepository.QueryPlanWorkRptResponse(loactionId, strKey);
        }

        public plan_work_rpt_rn QueryPlanWorkRptRn(string id)
        {

            return _planWorkRptRepository.QueryPlanWorkRptRn(id);
        }

        public List<base_equipment> GetEqpByPlanWorkId(string PlanWorkId)
        {
            List<plan_work_rpt> _Rpts = _pikachuRepository.GetAll<plan_work_rpt>()
                .Where(a => a.plan_work_id == PlanWorkId).ToList();
            string[] eqpId = (from _Rpt in _Rpts select _Rpt.equipment_id).ToArray();
            List<base_equipment> _Equipments = _pikachuRepository.GetAllByIds<base_equipment>(eqpId);
            return _Equipments;
        }

    }
}
