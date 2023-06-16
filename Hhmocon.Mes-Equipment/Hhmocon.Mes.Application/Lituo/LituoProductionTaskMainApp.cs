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

using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 生产任务主表
    /// </summary>
    public class LituoProductionTaskMainApp
    {
        private LituoProductionTaskMainRepository _lituoProductionTaskMainRepository;
        private PikachuRepository _pikachuRepository;
        public LituoProductionTaskMainApp(LituoProductionTaskMainRepository lituoProductionTaskMainRepository, PikachuRepository pikachuRepository)
        {
            _lituoProductionTaskMainRepository = lituoProductionTaskMainRepository;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 获取已经完成的订单
        /// </summary>
        /// <returns></returns>
        public int[] TheNumberOfFourTypesOrder()
        {
            return _lituoProductionTaskMainRepository.TheNumberOfFourTypesOrder();
        }

        /// <summary>
        /// 订单工序进度
        /// </summary>
        /// <returns></returns>
        public List<OrderOperationProgressResponse> OrderOperationProgress()
        {
            return _lituoProductionTaskMainRepository.OrderOperationProgress();
        }

        /// <summary>
        /// 通过日期获取当天完成的订单
        /// </summary>
        /// <returns></returns>
        public int GetProductionByDate(DateTime dateTime)
        {
            return _lituoProductionTaskMainRepository.GetProductionByDate(dateTime);
        }

        /// <summary>
        /// 获取订单完成三要素 总数 完成数 未完成数
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public OrderNumbers GetNumbersOfOrder(string ProcessNbm)
        {
            OrderNumbers numbers = new();
            //List<lituo_production_task_main> TodayTaskOrders = _lituoProductionTaskMainRepository.getTodayTaskOrders(ProcessNbm);
            List<lituo_production_task_main> TodayTaskOrders = _lituoProductionTaskMainRepository.getTodayTaskOrders("1");
            List<lituo_production_task_main> fihTaskOrders = _lituoProductionTaskMainRepository.getfihTaskOrders(ProcessNbm);
            numbers.TotalOrderNumber = TodayTaskOrders.Count;
            numbers.CompleteOrderNumber = fihTaskOrders.Count;
            numbers.IncompleteOrderNumber = TodayTaskOrders.Count - fihTaskOrders.Count;
            return numbers;
        }

        /// <summary>
        /// 获取每个工序加工时间的汇总数据
        /// 带款式型号
        /// </summary>
        /// <param name="processoutputDate"></param>
        /// <returns></returns>
        public List<ProcessOutputSum> GetNumOfProcessOut(ProcessOutputDate processoutputDate)
        {

            List<lituo_production_task_main> taskMains = _pikachuRepository.GetAll<lituo_production_task_main>();

            List<ProcessOutputSum> outputSumList = new();

            for (DateTime date = processoutputDate.start_time; date <= processoutputDate.end_time; date = date.AddDays(1))
            {
                ProcessOutputSum outputSum = new();
                outputSum.processTime = date;
                //获取所有产品类型的加工工序1完成时间为第一天的主表订单号
                var query1 = (from taskMain in taskMains
                              where (taskMain.process1_finish_time == date.Date)
                              select taskMain.DHDH).ToArray();
                List<lituo_production_task_detail> taskDetails1 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                   .Where(a => query1.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                var detail1 = taskDetails1.Where(a => processoutputDate.product_name.Contains(a.KSXH)).ToList();
                int num1 = 0;
                foreach (var item in detail1)
                {
                    num1 = num1 + item.DHSL;
                }
                outputSum.process1Num = num1;


                var query2 = from taskMain in taskMains
                             where (taskMain.process2_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails2 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                   .Where(a => query2.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                var detail2 = taskDetails2.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num2 = 0;
                foreach (var item in detail2)
                {
                    num2 = num2 + item.DHSL;
                }
                outputSum.process2Num = num2;
                var query3 = from taskMain in taskMains
                             where (taskMain.process3_finish_time == date.Date)
                             select taskMain.DHDH;

                List<lituo_production_task_detail> taskDetails3 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query3.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                var detail3 = taskDetails3.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num3 = 0;
                foreach (var item in detail3)
                {
                    num3 = num3 + item.DHSL;
                }
                outputSum.process3Num = num3;



                var query4 = from taskMain in taskMains
                             where (taskMain.process4_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails4 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query4.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                var detail4 = taskDetails4.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num4 = 0;
                foreach (var item in detail4)
                {
                    num4 = num4 + item.DHSL;
                }
                outputSum.process4Num = num4;



                var query5 = from taskMain in taskMains
                             where (taskMain.process5_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails5 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query5.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                var detail5 = taskDetails5.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num5 = 0;
                foreach (var item in detail5)
                {
                    num2 = num5 + item.DHSL;
                }
                outputSum.process5Num = num5;

                outputSum.processTotalNum = num5 + num4 + num3 + num2 + num1;
                //获取所有产品类型的加工工序1完成时间为第一天的明细表
                outputSumList.Add(outputSum);

            }

            return outputSumList;
        }

        public List<ProcessOutputSum> GetNumOfProcessOutVirtual(ProcessOutputDate processoutputDate)
        {

            List<ProcessOutputSum> outputSumList = new();
            Random rand = new Random();
            for (DateTime date = processoutputDate.start_time; date <= processoutputDate.end_time; date = date.AddDays(1))
            {
                lituo_production_task_virtual virtualData = _pikachuRepository.GetAll<lituo_production_task_virtual>().Where(a => a.datetime.Date == date.Date).FirstOrDefault();
                ProcessOutputSum outputSum = new();
                if (virtualData == null) continue;
                outputSum.processTime = date;

                outputSum.process1Num = virtualData.unloading_process;
                outputSum.process2Num = virtualData.engraving_process;
                outputSum.process3Num = virtualData.welting_process;
                outputSum.process4Num = virtualData.packaging_process;
                outputSum.process5Num = virtualData.platening_process;

                outputSum.processTotalNum = outputSum.process1Num + outputSum.process2Num + outputSum.process3Num + outputSum.process4Num + outputSum.process5Num;
                //获取所有产品类型的加工工序1完成时间为第一天的明细表
                outputSumList.Add(outputSum);

            }

            return outputSumList;
        }


        /// <summary>
        /// 获取每个工序加工时间的汇总数据
        /// 不带款式型号
        /// </summary>
        /// <param name="processoutputDate"></param>
        /// <returns></returns>
        public processNameOrNum GetNumOfProcessOut2(ProcessOutputDate processoutputDate, bool lineChart = false)
        {

            List<lituo_production_task_main> taskMains = _pikachuRepository.GetAll<lituo_production_task_main>();
            processNameOrNum processNameOrNum = new();
            processNameOrNum.process1nameOrNum = new();
            processNameOrNum.process2nameOrNum = new();
            processNameOrNum.process3nameOrNum = new();
            processNameOrNum.process4nameOrNum = new();
            processNameOrNum.process5nameOrNum = new();
            processNameOrNum.dateString = new();
            processNameOrNum.total = new();
            if (!lineChart)
            {
                processNameOrNum.process1nameOrNum.Add("下料");
                processNameOrNum.process2nameOrNum.Add("雕刻");
                processNameOrNum.process3nameOrNum.Add("贴边");
                processNameOrNum.process4nameOrNum.Add("磨边");
                processNameOrNum.process5nameOrNum.Add("打包");
                processNameOrNum.dateString.Add("工序名称");
            }

            int process1Num = 0; int process2Num = 0; int process3Num = 0; int process4Num = 0; int process5Num = 0;
            for (DateTime date = processoutputDate.start_time; date <= processoutputDate.end_time; date = date.AddDays(1))
            {

                //获取所有产品类型的加工工序1完成时间为第一天的主表订单号
                var query1 = (from taskMain in taskMains
                              where (taskMain.process1_finish_time == date.Date)
                              select taskMain.DHDH).ToArray();
                List<lituo_production_task_detail> taskDetails1 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                   .Where(a => query1.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                //var detail1 = taskDetails1.Where(a => processoutputDate.product_name.Contains(a.KSXH)).ToList();
                int num1 = 0;
                foreach (var item in taskDetails1)
                {
                    num1 = num1 + item.DHSL;
                }
                process1Num += num1;
                processNameOrNum.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                processNameOrNum.process1nameOrNum.Add(num1.ToString());


                var query2 = from taskMain in taskMains
                             where (taskMain.process2_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails2 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                   .Where(a => query2.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                //var detail2 = taskDetails2.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num2 = 0;
                foreach (var item in taskDetails2)
                {
                    num2 = num2 + item.DHSL;
                }

                process2Num += num2;
                processNameOrNum.process2nameOrNum.Add(num2.ToString());
                var query3 = from taskMain in taskMains
                             where (taskMain.process3_finish_time == date.Date)
                             select taskMain.DHDH;

                List<lituo_production_task_detail> taskDetails3 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query3.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                //var detail3 = taskDetails3.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num3 = 0;
                foreach (var item in taskDetails3)
                {
                    num3 = num3 + item.DHSL;
                }
                process3Num += num3;
                processNameOrNum.process3nameOrNum.Add(num3.ToString());

                var query4 = from taskMain in taskMains
                             where (taskMain.process4_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails4 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query4.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                // var detail4 = taskDetails4.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num4 = 0;
                foreach (var item in taskDetails4)
                {
                    num4 = num4 + item.DHSL;
                }
                process4Num += num4;
                processNameOrNum.process4nameOrNum.Add(num4.ToString());

                var query5 = from taskMain in taskMains
                             where (taskMain.process5_finish_time == date.Date)
                             select taskMain.DHDH;
                List<lituo_production_task_detail> taskDetails5 = _pikachuRepository.GetAll<lituo_production_task_detail>()
                 .Where(a => query5.Contains(a.DHDH)).ToList();

                //明细表中符合产品类型的数据
                //var detail5 = taskDetails5.Where(a => processoutputDate.product_name.Contains(a.KSXH));
                int num5 = 0;
                foreach (var item in taskDetails5)
                {
                    num2 = num5 + item.DHSL;
                }
                process5Num += num5;
                processNameOrNum.process5nameOrNum.Add(num5.ToString());

            }
            processNameOrNum.total.Add(process1Num);
            processNameOrNum.total.Add(process2Num);
            processNameOrNum.total.Add(process3Num);
            processNameOrNum.total.Add(process4Num);
            processNameOrNum.total.Add(process5Num);
            return processNameOrNum;
        }

        public processNameOrNum GetNumOfProcessOut2Virtual(ProcessOutputDate processoutputDate, bool lineChart = false)
        {
            Random rand = new Random();

            processNameOrNum processNameOrNum = new();
            processNameOrNum.process1nameOrNum = new();
            processNameOrNum.process2nameOrNum = new();
            processNameOrNum.process3nameOrNum = new();
            processNameOrNum.process4nameOrNum = new();
            processNameOrNum.process5nameOrNum = new();
            processNameOrNum.dateString = new();
            processNameOrNum.total = new();
            if (!lineChart)
            {
                processNameOrNum.process1nameOrNum.Add("下料");
                processNameOrNum.process2nameOrNum.Add("雕刻");
                processNameOrNum.process3nameOrNum.Add("贴边");
                processNameOrNum.process4nameOrNum.Add("磨边");
                processNameOrNum.process5nameOrNum.Add("打包");
                processNameOrNum.dateString.Add("工序名称");
            }

            int process1Num = 0; int process2Num = 0; int process3Num = 0; int process4Num = 0; int process5Num = 0;
            for (DateTime date = processoutputDate.start_time; date <= processoutputDate.end_time; date = date.AddDays(1))
            {
                lituo_production_task_virtual virtualData = _pikachuRepository.GetAll<lituo_production_task_virtual>().Where(a => a.datetime.Date == date.Date).FirstOrDefault();
                if (virtualData == null) continue;

                process1Num += virtualData.unloading_process;
                process2Num += virtualData.engraving_process;
                process3Num += virtualData.welting_process;
                process4Num += virtualData.packaging_process;
                process5Num += virtualData.platening_process;

                processNameOrNum.process1nameOrNum.Add(virtualData.unloading_process.ToString());
                processNameOrNum.process2nameOrNum.Add(virtualData.engraving_process.ToString());
                processNameOrNum.process3nameOrNum.Add(virtualData.welting_process.ToString());
                processNameOrNum.process4nameOrNum.Add(virtualData.packaging_process.ToString());
                processNameOrNum.process5nameOrNum.Add(virtualData.platening_process.ToString());


                processNameOrNum.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
            }
            processNameOrNum.total.Add(process1Num);
            processNameOrNum.total.Add(process2Num);
            processNameOrNum.total.Add(process3Num);
            processNameOrNum.total.Add(process4Num);
            processNameOrNum.total.Add(process5Num);
            return processNameOrNum;
        }
        /// <summary>
        /// 获取今日订单总数
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public List<lituo_production_task_main> getTodayTaskOrders(string ProcessNbm)
        {
 
            List<lituo_production_task_main> TodayTaskOrders = _lituoProductionTaskMainRepository.getTodayTaskOrders("1");
            return TodayTaskOrders;
        }

        public List<lituo_production_task_main_rep> getTodayTaskOrdersRep(string ProcessNbm)
        {
            List<lituo_production_task_main_rep> TodayTaskOrders = _lituoProductionTaskMainRepository.getTodayTaskOrdersRep("1");
            return TodayTaskOrders;
        }



        public string OrderLoadingShelf(string orderid, string hjcode, string userid)
        {
            string ret = string.Empty;
            try
            {
            
                lituo_production_task_main lituoMain = _pikachuRepository.GetById<lituo_production_task_main>(orderid);
                if (lituoMain == null)
                {
                    return "订单不存在，请确认订单号是否正确！";
                }
                if(!string.IsNullOrEmpty(lituoMain.hjcode))
                {
                    if (lituoMain.hjcode.IndexOf(hjcode) >= 0)
                    {
                        return "该订单已经放入该货架，不需要重复操作！";
                    }
                }

                if (lituoMain.send_flag == 1)
                {
                    return "订单已发货，不能进行上架操作！";
                }

                if (lituoMain.order_finish_flag != 1)
                {
                    return "该订单完成未报工，不能进行上架操作！";
                }

                string UpdateFields = string.Empty;

                UpdateFields = "hjcode,hj_enter_user,hj_enter_time";

                bool bret = false;


                lituoMain.hjcode = hjcode;
                lituoMain.hj_enter_time = DateTime.Now;
                lituoMain.hj_enter_user = userid;

                bret = _pikachuRepository.Update<lituo_production_task_main>(lituoMain, UpdateFields);
                if (bret)
                {
                    ret = "订单[" + hjcode + "] 放入[" + hjcode + "]货架完成！";
                }
                else
                {
                    ret = "订单[" + hjcode + "] 上架失败，请重试！";
                }

            }
            catch (Exception ex)
            {
                ret = "订单上架异常，原因为：" + ex.ToString();
            }

            return ret;
        }


        public string OrderShelfEnterStock(string hjcode, string kwcode, string userid)
        {
            string ret = string.Empty;
            try
            {
                string wheresql = "WHERE 1=1 ";
                wheresql = wheresql + " AND hjcode LIKE '%" + hjcode + "%' AND (send_flag is null or send_flag<>1) ";

                List<lituo_production_task_main> lituo_Production_Task_Mains = _pikachuRepository.GetAll<lituo_production_task_main>(wheresql, "ORDER BY DHDH");
                if ((lituo_Production_Task_Mains == null)||(lituo_Production_Task_Mains.Count<=0))
                {
                    return "系统中没有找到该货架中的订单信息，请先将订单上架！";
                }
                string UpdateFields = string.Empty;

                UpdateFields = "kwcode,kw_enter_user,kw_enter_time";

                bool bret = false;
                foreach (lituo_production_task_main lituoMain in lituo_Production_Task_Mains)
                {
                    lituoMain.kwcode = kwcode;
                    lituoMain.kw_enter_time = DateTime.Now;
                    lituoMain.kw_enter_user = userid;

                    bret = _pikachuRepository.Update<lituo_production_task_main>(lituoMain, UpdateFields);
                }

                if (bret)
                {
                    ret = "货架[" + hjcode + "] 放入["+ kwcode + "]库位完成！";
                }
                else
                {
                    ret = "货架[" + hjcode + "] 入库失败，请重试！";
                }

            }
            catch (Exception ex)
            {
                ret = "货架入库异常，原因为：" + ex.ToString();
            }

            return ret;
        }

        public string OrderShip(string orderid, string processUserid)
        {
            string ret = string.Empty;
            try
            {
                lituo_production_task_main lituoMain = _pikachuRepository.GetById<lituo_production_task_main>(orderid);
                if (lituoMain == null)
                {
                    return "订单不存在，请确认订单号是否正确！";
                }

                if (lituoMain.send_flag == 1)
                {
                    return "订单已发货，不能重复操作！";
                }

                if (lituoMain.order_finish_flag != 1)
                {
                    return "该订单完成未报工，不能进行发货操作！";
                }

                string UpdateFields = string.Empty;
                lituoMain.send_flag = 1;
                lituoMain.send_time = DateTime.Now;
                lituoMain.send_user = processUserid;
                UpdateFields = "send_flag,send_time,send_user";


                bool bret = _pikachuRepository.Update<lituo_production_task_main>(lituoMain, UpdateFields);
                if (bret)
                {
                    ret = "订单[" + orderid + "] 发货完成！";
                }
                else
                {
                    ret = "订单[" + orderid + "] 发货失败，请重试！";
                }

            }


            catch (Exception ex)
            {
                ret = "订单发货异常，原因为：" + ex.ToString();
            }

            return ret;
        }


        /// <summary>
        /// 报工接口
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="processid"></param>
        /// <param name="processUserid"></param>
        /// <returns></returns>
        public string reportPrcess(string orderid, string processid, string processUserid)
        {
            string ret = string.Empty;
            try
            {
                string UpdateFields = string.Empty;


                //根据订单号 查询出 订单的相关信息，然后拼接 更新字符串
                lituo_production_task_main lituoMain = _pikachuRepository.GetById<lituo_production_task_main>(orderid);

                if (lituoMain == null)
                {
                    return "订单不存在，请确认订单号是否正确！";
                }

                // 根据订货单号  判断订单状态
                switch (processid)
                {
                    case "1":
                        if (lituoMain.process1_finish_flag > 0)
                        {
                            ret = "该订单下料工序已报工！";
                            return ret;
                        }
                        lituoMain.process1_finish_flag = 1;
                        lituoMain.process1_finish_time = DateTime.Now;
                        lituoMain.process1_finish_user = processUserid;
                        UpdateFields = "process1_finish_flag,process1_finish_time,process1_finish_user";
                        break;
                    case "2":
                        if (lituoMain.process2_finish_flag > 0)
                        {
                            ret = "该订单雕刻工序已报工！";
                            return ret;
                        }

                        if (lituoMain.process1_finish_flag > 0)
                        {
                            lituoMain.process2_finish_flag = 1;
                            lituoMain.process2_finish_time = DateTime.Now;
                            lituoMain.process2_finish_user = processUserid;
                            UpdateFields = "process2_finish_flag,process2_finish_time,process2_finish_user";
                        }
                        else
                        {
                            lituoMain.process1_finish_flag = 1;
                            lituoMain.process1_finish_time = DateTime.Now;
                            lituoMain.process1_finish_user = processUserid;
                            lituoMain.process2_finish_flag = 1;
                            lituoMain.process2_finish_time = DateTime.Now;
                            lituoMain.process2_finish_user = processUserid;
                            UpdateFields = "process1_finish_flag,process1_finish_time,process1_finish_user,process2_finish_flag,process2_finish_time,process2_finish_user";
                        }


                        break;
                    case "3":
                        if (lituoMain.process3_finish_flag > 0)
                        {
                            ret = "该订单贴边工序已报工！";
                            return ret;
                        }


                        if (lituoMain.process1_finish_flag <= 0)
                        {
                            lituoMain.process1_finish_flag = 1;
                            lituoMain.process1_finish_time = DateTime.Now;
                            lituoMain.process1_finish_user = processUserid;
                            UpdateFields = "process1_finish_flag,process1_finish_time,process1_finish_user";
                        }

                        if (lituoMain.process2_finish_flag <= 0)
                        {
                            lituoMain.process2_finish_flag = 1;
                            lituoMain.process2_finish_time = DateTime.Now;
                            lituoMain.process2_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }

                        }


                        if (lituoMain.process3_finish_flag <= 0)
                        {
                            lituoMain.process3_finish_flag = 1;
                            lituoMain.process3_finish_time = DateTime.Now;
                            lituoMain.process3_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                        }



                        break;
                    case "4":
                        if (lituoMain.process4_finish_flag > 0)
                        {
                            ret = "该订单磨边工序已报工！";
                            return ret;
                        }

                        if (lituoMain.process1_finish_flag <= 0)
                        {
                            lituoMain.process1_finish_flag = 1;
                            lituoMain.process1_finish_time = DateTime.Now;
                            lituoMain.process1_finish_user = processUserid;
                            UpdateFields = "process1_finish_flag,process1_finish_time,process1_finish_user";
                        }

                        if (lituoMain.process2_finish_flag <= 0)
                        {
                            lituoMain.process2_finish_flag = 1;
                            lituoMain.process2_finish_time = DateTime.Now;
                            lituoMain.process2_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }

                        }


                        if (lituoMain.process3_finish_flag <= 0)
                        {
                            lituoMain.process3_finish_flag = 1;
                            lituoMain.process3_finish_time = DateTime.Now;
                            lituoMain.process3_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                        }

                        if (lituoMain.process4_finish_flag <= 0)
                        {
                            lituoMain.process4_finish_flag = 1;
                            lituoMain.process4_finish_time = DateTime.Now;
                            lituoMain.process4_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process4_finish_flag,process4_finish_time,process4_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process4_finish_flag,process4_finish_time,process4_finish_user";
                            }
                        }



                        break;
                    case "5":
                        if (lituoMain.process5_finish_flag > 0)
                        {
                            ret = "该订单打包工序已报工！";
                            return ret;
                        }



                        if (lituoMain.process1_finish_flag <= 0)
                        {
                            lituoMain.process1_finish_flag = 1;
                            lituoMain.process1_finish_time = DateTime.Now;
                            lituoMain.process1_finish_user = processUserid;
                            UpdateFields = "process1_finish_flag,process1_finish_time,process1_finish_user";
                        }

                        if (lituoMain.process2_finish_flag <= 0)
                        {
                            lituoMain.process2_finish_flag = 1;
                            lituoMain.process2_finish_time = DateTime.Now;
                            lituoMain.process2_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process2_finish_flag,process2_finish_time,process2_finish_user";
                            }

                        }


                        if (lituoMain.process3_finish_flag <= 0)
                        {
                            lituoMain.process3_finish_flag = 1;
                            lituoMain.process3_finish_time = DateTime.Now;
                            lituoMain.process3_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process3_finish_flag,process3_finish_time,process3_finish_user";
                            }
                        }

                        if (lituoMain.process4_finish_flag <= 0)
                        {
                            lituoMain.process4_finish_flag = 1;
                            lituoMain.process4_finish_time = DateTime.Now;
                            lituoMain.process4_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process4_finish_flag,process4_finish_time,process4_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process4_finish_flag,process4_finish_time,process4_finish_user";
                            }
                        }

                        if (lituoMain.process5_finish_flag <= 0)
                        {
                            lituoMain.process5_finish_flag = 1;
                            lituoMain.process5_finish_time = DateTime.Now;
                            lituoMain.process5_finish_user = processUserid;

                            if (string.IsNullOrEmpty(UpdateFields))
                            {
                                UpdateFields = "process5_finish_flag,process5_finish_time,process5_finish_user";
                            }
                            else
                            {
                                UpdateFields = UpdateFields + "," + "process5_finish_flag,process5_finish_time,process5_finish_user";
                            }
                        }

                        lituoMain.order_finish_flag = 1;
                        lituoMain.order_finish_time = DateTime.Now;

                        if (string.IsNullOrEmpty(UpdateFields))
                        {
                            UpdateFields = "order_finish_flag,order_finish_time";
                        }
                        else
                        {
                            UpdateFields = UpdateFields + "," + "order_finish_flag,order_finish_time";
                        }

                        break;
                }

                string pname = string.Empty;

                switch (processid)
                {
                    case "1":
                        pname = "下料";
                        break;
                    case "2":
                        pname = "雕刻";
                        break;
                    case "3":
                        pname = "贴边";
                        break;
                    case "4":
                        pname = "磨边";
                        break;
                    case "5":
                        pname = "打包";
                        break;
                }

                bool bret = _pikachuRepository.Update<lituo_production_task_main>(lituoMain, UpdateFields);
                if (bret)
                {
                    ret = "订单[" + orderid + "][" + pname + "]工序报工完成！";
                }
                else
                {
                    ret = "订单[" + orderid + "][" + pname + "]工序报工失败，请重试！";
                }


            }
            catch (Exception ex)
            {
                ret = "订单报工异常，原因为：" + ex.ToString();
            }

            return ret;
        }
    }
}
