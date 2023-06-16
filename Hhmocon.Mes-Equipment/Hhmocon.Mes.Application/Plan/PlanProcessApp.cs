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

using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Plan
{
    /// <summary>
    /// 计划表
    /// </summary>
    public class PlanProcessApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PlanProcessRepository _planProcessRepository;
        private readonly IAuth _auth;

        public PlanProcessApp(PikachuRepository pikachuRepository, PlanProcessRepository planProcessRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _planProcessRepository = planProcessRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增计划
        /// 计划是从物料订单增加的
        /// </summary>
        /// <returns></returns>
        public bool InsertByOreder(sale_order_detail obj)
        {
            plan_process plan_Process = new();
            ////对工单的code进行查重
            //List<plan_process> exists =
            //_pikachuRepository.GetAll<plan_process>().Where(c =>
            //    c.plan_process_code == obj.plan_process_code
            //).ToList();
            //if (exists.Count > 0)
            //{
            //    throw new Exception("计划工单编号重复");
            //}

            //取ID
            plan_Process.plan_process_id = CommonHelper.GetNextGUID();
            //plan_Process.plan_process_code = "p" + Time.Now;
            plan_Process.modified_time = Time.Now;
            plan_Process.create_time = DateTime.Now;
            plan_Process.process_id = "先空着";
            plan_Process.plan_num = obj.qty;
            plan_Process.start_time = obj.start_time;
            plan_Process.end_time = "1970-01-01 00:00:00".ToDate();
            plan_Process.material_id = obj.material_id;
            plan_Process.plan_process_code = obj.sale_order_detail_code;//计划编号 = 物料订单编号
            plan_Process.create_by = _auth.GetUserAccount(null);
            plan_Process.create_by_name = _auth.GetUserName(null);
            plan_Process.modified_by = _auth.GetUserAccount(null);
            plan_Process.modified_by_name = _auth.GetUserName(null);
            return (_pikachuRepository.Insert(plan_Process));
        }

        public plan_process Insert(plan_process data)
        {
            //取ID
            data.plan_process_id = CommonHelper.GetNextGUID();
            //data.plan_process_code = "p" + Time.Now;
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);


            //Test test = new(_baseCustomerRepository, _pikachuRepository, _auth);
            //test.ExportByClassName(base_Customer, base_Customer);
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }
        public List<plan_process_rep> QueryPlanProcessResponse(PageReq req, ref long lcount)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
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
            List<plan_process_rep> data = _planProcessRepository.QueryPlanProcessResponse(whereStr);
            lcount = data != null ? data.Count : 0;
            return data;
        }





    }
}
