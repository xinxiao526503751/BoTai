using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Lituo
{
    /// <summary>
    /// 力拓生产任务主表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Lituo", IgnoreApi = false)]
    public class LituoProductionTaskMainController
    {
        private readonly PikachuApp _pikachuApp;
        private readonly LituoProductionTaskMainApp _lituoProductionTaskMainApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        public LituoProductionTaskMainController(PikachuApp pikachuApp, LituoProductionTaskMainApp lituoProductionTaskMainApp)
        {
            _pikachuApp = pikachuApp;
            _lituoProductionTaskMainApp = lituoProductionTaskMainApp;
        }


        /// <summary>
        /// 力拓看板指挥中心页面，左侧订单进度，已完成订单
        /// 0是已完成
        /// 1是订单总
        /// 2是正加工
        /// 3是待加工
        /// </summary>
        /// <returns>int[4]</returns>
        [HttpPost]
        public Response<int[]> TheNumberOfFourTypesOrder()
        {
            Response<int[]> result = new Response<int[]>();
            try
            {
                result.Result = _lituoProductionTaskMainApp.TheNumberOfFourTypesOrder();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 订单工序进度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<OrderOperationProgressResponse>> OrderOperationProgress()
        {
            Response<List<OrderOperationProgressResponse>> result = new Response<List<OrderOperationProgressResponse>>();
            try
            {
                result.Result = _lituoProductionTaskMainApp.OrderOperationProgress();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 获取订单完成三要素 总数 完成数 未完成数
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<OrderNumbers> GetNumbersOfOrder(string ProcessNbm)
        {
            Response<OrderNumbers> result = new Response<OrderNumbers>();
            try
            {
                if (string.IsNullOrEmpty(ProcessNbm))
                {
                    throw new Exception("请选择工序");
                }

                result.Result = _lituoProductionTaskMainApp.GetNumbersOfOrder(ProcessNbm);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 工序加工报表接口
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<ProcessOutputSum>> GetNumOfProcessOut(ProcessOutputDate processoutputDate)
        {
            Response<List<ProcessOutputSum>> result = new Response<List<ProcessOutputSum>>();
            try
            {
                if (processoutputDate.start_time > processoutputDate.end_time)
                {
                    throw new Exception("开始时间不能晚于结束时间");
                }

                if (processoutputDate.start_time.Date > DateTime.Now.Date)
                {
                    throw new Exception("开始时间不能晚于今日");
                }

                if (processoutputDate.end_time.Date > DateTime.Now.Date)
                {
                    throw new Exception("结束时间不能晚于今日");
                }

                result.Result = _lituoProductionTaskMainApp.GetNumOfProcessOutVirtual(processoutputDate);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public Response<processNameOrNum> GetNumOfProcessOut2(ProcessOutputDate processoutputDate, bool lineChart = false)
        {
            Response<processNameOrNum> result = new Response<processNameOrNum>();
            try
            {
                if (processoutputDate.start_time > processoutputDate.end_time)
                {
                    throw new Exception("开始时间不能晚于结束时间");
                }

                if (processoutputDate.start_time.Date > DateTime.Now.Date)
                {
                    throw new Exception("开始时间不能晚于今日");
                }

                if (processoutputDate.end_time.Date > DateTime.Now.Date)
                {
                    throw new Exception("结束时间不能晚于今日");
                }

                result.Result = _lituoProductionTaskMainApp.GetNumOfProcessOut2Virtual(processoutputDate, lineChart);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取今日订单数据
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        [HttpPost]
        public  Response<List<lituo_production_task_main>> getTodayTaskOrders(string ProcessNbm)
        {
            Response<List<lituo_production_task_main>> result = new Response<List<lituo_production_task_main>>();
            try
            {
                if (string.IsNullOrEmpty(ProcessNbm))
                {
                    throw new Exception("请选择工序");
                }

                result.Result = _lituoProductionTaskMainApp.getTodayTaskOrders(ProcessNbm);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 获取今日订单数据
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<lituo_production_task_main_rep>> getTodayTaskOrdersRep(string ProcessNbm)
        {
            Response<List<lituo_production_task_main_rep>> result = new Response<List<lituo_production_task_main_rep>>();
            try
            {
                if (string.IsNullOrEmpty(ProcessNbm))
                {
                    throw new Exception("请选择工序");
                }

                result.Result = _lituoProductionTaskMainApp.getTodayTaskOrdersRep(ProcessNbm);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }

        /// 获取最近七天  所有当天完成的订单数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<int>> GetProductionByDate()
        {
            Response<List<int>> result = new Response<List<int>>();
            try
            {
                DateTime today = DateTime.Now;
                List<DateTime> datetime = new();
                datetime.Add(today);
                datetime.Add(today.AddDays(-1));
                datetime.Add(today.AddDays(-2));
                datetime.Add(today.AddDays(-3));
                datetime.Add(today.AddDays(-4));
                datetime.Add(today.AddDays(-5));
                datetime.Add(today.AddDays(-6));
                List<int> res = new();
                foreach (DateTime dateTime in datetime)
                {
                    int de = _lituoProductionTaskMainApp.GetProductionByDate(dateTime);
                    res.Add(de);
                }
                result.Result = res;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
