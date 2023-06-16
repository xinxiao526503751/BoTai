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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 生产任务主表仓储
    /// </summary>
    public class LituoProductionTaskMainRepository : ILituoProductionTaskMainRepository, IDependency
    {
        private PikachuRepository _PikachuRepository;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuRepository"></param>
        public LituoProductionTaskMainRepository(PikachuRepository pikachuRepository)
        {
            _PikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 力拓指挥中心页面左上角订单进度的 已完成订单、正在加工订单、待加工订单、总订单数
        /// 注意数据都是当天的数据，之前没做完的要累积
        /// </summary>
        /// <returns></returns>
        public int[] TheNumberOfFourTypesOrder()
        {
            using (var conn = SqlServerDbHelper.GetConn())
            {
                //找到所有  当天的订单  
                string sql = $"select * from lituo_production_task_main where PGRQ = '{DateTime.Now.Date}' " +
                    $" union" +
                    $" select * from lituo_production_task_main where PGRQ < '{DateTime.Now.Date}' and order_finish_flag = 0" +//找到所有之前的未完成的订单
                    $" union " +
                    $"select * from lituo_production_task_main where PGRQ < '{DateTime.Now.Date}' and order_finish_time = '{DateTime.Now.Date}'";//找到所有之前的完工时间在今天的订单

                List<lituo_production_task_main> lituo_Production_Task_Mains = conn.Query<lituo_production_task_main>(sql).ToList();

                int[] result = new int[4];
                result[0] = lituo_Production_Task_Mains.Where(c => c.order_finish_flag == 1 && c.order_finish_time == DateTime.Now.Date).ToList().Count;//已完成订单数(今天内)
                result[1] = lituo_Production_Task_Mains.Count;//订单总数

                result[2] = lituo_Production_Task_Mains.Where(c => (c.process1_finish_flag == 1 ||
                                                                c.process2_finish_flag == 1 ||
                                                                c.process3_finish_flag == 1 ||
                                                                c.process4_finish_flag == 1 ||
                                                                c.process5_finish_flag == 1) && c.order_finish_flag == 0)
                                                                .ToList().Count;//正在加工订单(第一道工序已经完成，且订单标志位为0)
                result[3] = lituo_Production_Task_Mains.Where(c => (c.process1_finish_flag == 0 &&
                                                                  c.process2_finish_flag == 0 &&
                                                                  c.process3_finish_flag == 0 &&
                                                                  c.process4_finish_flag == 0 &&
                                                                  c.process5_finish_flag == 0) && c.order_finish_flag == 0).ToList().Count;  //待加工订单
                return result;
            }
        }

        /// <summary>
        /// 订单工序进度
        /// </summary>
        /// <returns></returns>
        public List<OrderOperationProgressResponse> OrderOperationProgress()
        {
            using (var conn = SqlServerDbHelper.GetConn())
            {
                //找到所有  当天的订单  

             string sql = $"select * from [lituo_production_task_main] " +
            $" where[lituo_production_task_main].process5_finish_time is null " +
            $"or " +
             $"cast(convert(varchar(10), [lituo_production_task_main].process5_finish_time, 120) as datetime) = '{DateTime.Now.Date}'  order by 'XSGH' ";




                List<lituo_production_task_main> lituo_Production_Task_Mains = conn.Query<lituo_production_task_main>(sql).ToList();

                List<OrderOperationProgressResponse> orderOperationProgressResponses = new();

                sql = $"select * from lituo_production_task_detail left join lituo_production_task_main on [lituo_production_task_main].DHDH = [lituo_production_task_detail].DHDH";
                List<lituo_production_task_detail> lituo_Production_Task_Details = conn.Query<lituo_production_task_detail>(sql).ToList();

                foreach (lituo_production_task_main temp in lituo_Production_Task_Mains)
                {
                    OrderOperationProgressResponse orderOperationProgressResponse = new();

                    orderOperationProgressResponse.DHDH = temp.DHDH;
                    orderOperationProgressResponse.process1_finish_flag = temp.process1_finish_flag;
                    orderOperationProgressResponse.process2_finish_flag = temp.process2_finish_flag;
                    orderOperationProgressResponse.process3_finish_flag = temp.process3_finish_flag;
                    orderOperationProgressResponse.process4_finish_flag = temp.process4_finish_flag;
                    orderOperationProgressResponse.process5_finish_flag = temp.process5_finish_flag;

                    lituo_production_task_detail lituo_Production_Task_Detail = lituo_Production_Task_Details.Where(c => c.DHDH == temp.DHDH).FirstOrDefault();
                    if (lituo_Production_Task_Detail == null)
                    {
                        orderOperationProgressResponse.material_name = "无效的订货单号";
                    }
                    else
                    {
                        orderOperationProgressResponse.material_name = lituo_Production_Task_Detail?.CZ;//材质
                    }
                    orderOperationProgressResponses.Add(orderOperationProgressResponse);
                }
                return orderOperationProgressResponses;
            }
        }

        /// <summary>
        /// 通过日期获取所有当天完成的订单数
        /// </summary>
        /// <returns></returns>
        public int GetProductionByDate(DateTime dateTime)
        {
            using (var conn = SqlServerDbHelper.GetConn())
            {
                string sql = "select * from lituo_production_task_main where order_finish_flag = '1'";
                List<lituo_production_task_main> lituo_Production_Task_Mains = conn.Query<lituo_production_task_main>(sql).ToList();
                return lituo_Production_Task_Mains.Count;
            }

        }

        /// <summary>
        /// 获取订单总数
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public List<lituo_production_task_main> getTodayTaskOrders2(string ProcessNbm)
        {
            //今日加工订单
            List<lituo_production_task_main> TodayTaskOrders = new();
            //昨日剩余未完成订单
            List<lituo_production_task_main> YdUnfTaskOrders = new();

            if (ProcessNbm == "1")
                //TodayTaskOrders = _PikachuRepository.GetbySql<lituo_production_task_main>(string.Format("WHERE process1_finish_flag='0'and PGRQ ={0}", DateTime.Today));
                //派工日期是过去日期 且未完成该工序的订单
                TodayTaskOrders = _PikachuRepository.GetbySql<lituo_production_task_main>(string.Format("WHERE process1_finish_flag='0'"))
                    .Where(a => a.PGRQ.Date <= DateTime.Now.Date).ToList();
            else
            {
                //派工日期是过去日期 且未完成该工序的订单
                TodayTaskOrders = _PikachuRepository.GetbySql<lituo_production_task_main>
                    (string.Format("WHERE process{0}_finish_flag='1'and process{1}_finish_flag = '0' ", int.Parse(ProcessNbm) - 1, ProcessNbm))
                    .Where(a => a.PGRQ.Date <= DateTime.Now.Date).ToList();
            }
            //已经完成该工序的 但是工序完成时间为今天的订单
            YdUnfTaskOrders = _PikachuRepository.GetbySql<lituo_production_task_main>
                  (string.Format("WHERE process{0}_finish_flag='1' AND process{1}_finish_time BETWEEN '{2}' AND '{3}'", ProcessNbm, ProcessNbm, DateTime.Now.Date, DateTime.Now.AddDays(1).Date));
            //(string.Format("WHERE process{0}_finish_flag='1'", ProcessNbm, ProcessNbm))
            //.Where(a => a.process1_finish_time.Date== DateTime.Now.Date).ToList();//"process{0}_finish_flag='1'条件可能可删
            TodayTaskOrders.AddRange(YdUnfTaskOrders);
            return TodayTaskOrders;
        }

        public List<lituo_production_task_main> getTodayTaskOrders (string ProcessNbm)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (conn)
            {
                string query = null;
                //query = string.Format("SELECT M.*,D.SL FROM [lituo_production_task_main] M LEFT JOIN(SELECT DHDH, ISNULL(SUM(DHSL),0) SL FROM[lituo_production_task_detail]  GROUP BY DHDH) D ON D.DHDH = M.DHDH" +
                //    " WHERE M.PGRQ BETWEEN '{0}' AND '{1}' OR (M.order_finish_flag='0' AND M.PGRQ <= '{2}') OR M.order_finish_time BETWEEN '{3}' AND '{4}' ",
                //    DateTime.Now.Date, DateTime.Now.AddDays(1).Date, DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                //List<lituo_production_task_main> datas = conn.Query<lituo_production_task_main>(query);
                query = string.Format("SELECT M.*,D.SL FROM [lituo_production_task_main] M LEFT JOIN(SELECT DHDH, ISNULL(SUM(DHSL),0) SL FROM[lituo_production_task_detail]  GROUP BY DHDH) D ON D.DHDH = M.DHDH" +
              " WHERE M.process5_finish_time BETWEEN '{0}' AND '{1}' OR process5_finish_time is null ", DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                List<lituo_production_task_main> datas = new();
                List<lituo_production_task_main> b = conn.Query<lituo_production_task_main, lituo_production_task_detail, List<lituo_production_task_main>>(query,
                   (task_main, task_detail) =>
                   {
                       lituo_production_task_main data = new();
                       data = task_main;
                       data.SCBS = task_detail.DHSL;
                       datas.Add(data);
                       return datas;
                   },splitOn: "process5_finish_user").Distinct().SingleOrDefault();
                return b;


            }
        }

        public List<lituo_production_task_main_rep> getTodayTaskOrdersRep(string ProcessNbm)
        {
            using System.Data.IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            using (conn)
            {
                string query = null;
                //query = string.Format("SELECT M.*,D.SL FROM [lituo_production_task_main] M LEFT JOIN(SELECT DHDH, ISNULL(SUM(DHSL),0) SL FROM[lituo_production_task_detail]  GROUP BY DHDH) D ON D.DHDH = M.DHDH" +
                //    " WHERE M.PGRQ BETWEEN '{0}' AND '{1}' OR (M.process5_finish_flag='0' AND M.PGRQ <= '{2}') OR M.process5_finish_time BETWEEN '{3}' AND '{4}' ",
                //DateTime.Now.Date, DateTime.Now.AddDays(1).Date, DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                //List<lituo_production_task_main> datas = conn.Query<lituo_production_task_main>(query);

                query = string.Format("SELECT M.*,D.SL FROM [lituo_production_task_main] M LEFT JOIN(SELECT DHDH, ISNULL(SUM(DHSL),0) SL FROM[lituo_production_task_detail]  GROUP BY DHDH) D ON D.DHDH = M.DHDH" +
                  " WHERE M.process5_finish_time BETWEEN '{0}' AND '{1}' OR process5_finish_time is null ", DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
               var datas = conn.Query<lituo_production_task_main_rep>(query).ToList();

                return datas;
            }
        }

        /// <summary>
        /// 获取已经完成的订单
        /// </summary>
        /// <param name="ProcessNbm"></param>
        /// <returns></returns>
        public List<lituo_production_task_main> getfihTaskOrders(string ProcessNbm)
        {
            List<lituo_production_task_main> fihTaskOrders = new();
            fihTaskOrders = _PikachuRepository.GetbySql<lituo_production_task_main>
                    (string.Format("WHERE process{0}_finish_flag = '1'  AND process{1}_finish_time BETWEEN '{2}' AND '{3}'", ProcessNbm, ProcessNbm, DateTime.Now.Date, DateTime.Now.AddDays(1).Date));
                     //(string.Format("WHERE process{0}_finish_flag = '1'", ProcessNbm, ProcessNbm, DateTime.Now.Date));
            return fihTaskOrders;
        }

        public void ComputeNumByprocess(string processNbm, ProcessOutputDate processoutputDate)
        {
            List<lituo_production_task_main> taskMains = _PikachuRepository.GetAll<lituo_production_task_main>();

            ProcessOutputSum outputSum = new();
            var date = processoutputDate.start_time;
            //获取所有产品类型的加工工序1完成时间为第一天的主表订单号
            var query1 = from taskMain in taskMains
                         where (taskMain.process1_finish_time == date)
                         select taskMain.DHDH;

            var quer2 = from taskMain in taskMains
                        where (taskMain.process2_finish_time == date)
                        select taskMain.DHDH;

            var query3 = from taskMain in taskMains
                         where (taskMain.process3_finish_time == date)
                         select taskMain.DHDH;

            var query4 = from taskMain in taskMains
                         where (taskMain.process4_finish_time == date)
                         select taskMain.DHDH;

            var query5 = from taskMain in taskMains
                         where (taskMain.process5_finish_time == date)
                         select taskMain.DHDH;

            //获取所有产品类型的加工工序1完成时间为第一天的明细表
            List<lituo_production_task_detail> taskDetails1 = _PikachuRepository.GetAll<lituo_production_task_detail>()
                .Where(a => query1.Contains(a.DHDH)).ToList();

            //明细表中符合产品类型的数据
            var detail1 = taskDetails1.Where(a => processoutputDate.product_name.Contains(a.KSXH));
            int num = 0;
            foreach (var item in detail1)
            {
                num = num + item.DHSL;
            }
            outputSum.process1Num = num;
        }
    }
}