using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Exam;
using Hhmocon.Mes.Application.Lituo;
using Hhmocon.Mes.Repository.Domain;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.TimeJob
{
    public class TestJob : Job
    {
        private ExamPlanRecApp _app;
        private ExamPlanMethodRuleApp _app2;
        private ExamPlanMethodItemApp _app3;
        private ExamPlanRecItemApp _app4;
        private PikachuApp _pikachuApp;
        private LituoEnergyConsumptionApp _app5;
        public TestJob(ExamPlanRecApp app, ExamPlanMethodRuleApp app2, ExamPlanMethodItemApp app3, ExamPlanRecItemApp app4, LituoEnergyConsumptionApp app5, PikachuApp picachuApp)
        {
            _app = app;
            _app2 = app2;
            _app3 = app3;
            _app4 = app4;
            _app5 = app5;
            _pikachuApp = picachuApp;
        }

        [Invoke(Begin = "2021-8-3 00:00", Interval = 5000, SkipWhileExecuting=true)]
        public void Test(IServiceProvider service)
        { 
            Method1();
            Method2();
        }
       
        //单次添加标志位
        public static string MonthlyAddFlag;
        public static string WeeklyAddFlag;
        public static string DailyAddFlag;
        public static string SpecifiedTimeAddFlag;
        //上次执行时间
        public static int LastExecutionDate;
        public static int LastExecutionMonth;
        public static DateTime LastExecutionTime;
        public void Method1()
        {
           
            var examPlanMethodList = _pikachuApp.GetAll<exam_plan_method>();//获取所有点检计划
            var currentTime = DateTime.Now;

            foreach (var item in examPlanMethodList)
            {
                //Console.WriteLine("test");
                if (item.effect_start_time> currentTime && item.effect_end_time < currentTime) continue;//判断点检计划是否在有效期            
                    List<exam_plan_method_rule> planMethodRuleList = _app2.GetExamPlanMethodRulesByPlanId(item.exam_plan_method_id);//获取点检计划对应的时间方案
                foreach (var planMethodRule in planMethodRuleList)
                {
                   //触发时间比较对象
                   string AdvanceOccurCondition;
                   //根据不同的触发类型执行不同的触发函数
                   DateTime ActualTriggerTime = DateTime.Now;//任意给个初始值
                        DateTime ComparetimeAdvance = DateTime.Now;
                        //提前时间单位判断计算实际触发时间
                        switch (item.lead_time_unit)//根据提前期 修正触发时间
                        {
                            case "0"://分钟

                                ActualTriggerTime = planMethodRule.trigger_time.
                                    AddMinutes(item.lead_time);
                                ComparetimeAdvance = ComparetimeAdvance.AddMilliseconds(-item.lead_time);
                                break;
                            case "1"://小时
                                ActualTriggerTime = planMethodRule.trigger_time.
                                    AddHours(item.lead_time);
                                ComparetimeAdvance = ComparetimeAdvance.AddHours(-item.lead_time);
                                break;
                            case "2"://天
                                ActualTriggerTime = planMethodRule.trigger_time.
                                    AddDays(item.lead_time);
                                ComparetimeAdvance = ComparetimeAdvance.AddDays(-item.lead_time);
                                break;
                        }
                     //   //只有在在达到开始时间的提前期的时间后才进行扫描设备点检计划时间方案
                        //根据不同时间计划执行不同的扫描函数
                        string occur_condition = planMethodRule.occur_condition;
                        switch (planMethodRule.unit_mode)
                        {
                            //case "2"://day日计划
                            //    AdvanceOccurCondition = ComparetimeAdvance.Hour.ToString();
                            //    dailyScheduleFunction(occur_condition, AdvanceOccurCondition, item);
                            //    break;
                            case "2"://day间隔天数计划
                                AdvanceOccurCondition = ComparetimeAdvance.Day.ToString();
                                var startTime = planMethodRule.modified_time;
                                dailyScheduleFunction(occur_condition, startTime, planMethodRule);
                                break;
                            case "1"://week周计划
                                AdvanceOccurCondition = ComparetimeAdvance.DayOfWeek.ToString();
                                weeklyPlanningFunction(occur_condition, AdvanceOccurCondition, planMethodRule);
                                break;
                            case "3"://month月计划
                                AdvanceOccurCondition = ComparetimeAdvance.Day.ToString();
                                monthlylyPlanningFunction(occur_condition, AdvanceOccurCondition, planMethodRule);
                                break;
                            case "0"://temporary指定日期
                                AdvanceOccurCondition = ComparetimeAdvance.ToString("yyyy-MM-dd");
                                specifyDateFunction(planMethodRule.trigger_time.ToString("yyyy-MM-dd"), AdvanceOccurCondition, planMethodRule);
                                break;
                        }                
                }
            }

        }//

        /// <summary>
        /// 能耗虚拟数据生成函数
        /// </summary>
        public void Method2()
        {
           
            Random rand = new Random();
            //找有没有当天电的数据
            bool existPower = _pikachuApp.GetAll<base_energy_consumption>().Where(a=>a.consumption_date.Date==DateTime.Now.Date&&a.energy_flag==0).ToList().Count==0;
            if (existPower)
            {

                base_energy_consumption lastPowerConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 0).ToList().LastOrDefault();
                base_energy_consumption powerConsumption = new();
                powerConsumption = lastPowerConsumption;
                powerConsumption.consumption_date = DateTime.Now;
                powerConsumption.create_time = DateTime.Now;
                powerConsumption.modified_time = DateTime.Now;
                powerConsumption.power_consumption_1 = lastPowerConsumption.power_consumption_1 + rand.Next(3, 8);
                powerConsumption.power_consumption_2 = lastPowerConsumption.power_consumption_2 + rand.Next(4, 10);
                powerConsumption.power_consumption_3 = lastPowerConsumption.power_consumption_3 + rand.Next(6, 10);
                _app5.Insert(powerConsumption);
                Console.WriteLine("添加电数据");

            }
            bool existWater = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date && a.energy_flag == 1).ToList().Count == 0;
            if (existWater)
            {
                base_energy_consumption lastWaterConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 1).ToList().LastOrDefault();
                base_energy_consumption waterConsumption = new();
                waterConsumption = lastWaterConsumption;
                waterConsumption.consumption_date = DateTime.Now;
                waterConsumption.create_time = DateTime.Now;
                waterConsumption.modified_time = DateTime.Now;
                waterConsumption.water_consumption_1 = lastWaterConsumption.water_consumption_1 + rand.Next(3, 6);
                waterConsumption.water_consumptio_2 = lastWaterConsumption.water_consumptio_2 + rand.Next(35, 56);
                _app5.Insert(waterConsumption);
                Console.WriteLine("添加水数据");
            }

            bool existGas = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date && a.energy_flag == 2).ToList().Count == 0;
            if (existGas)
            {
                base_energy_consumption lastGasConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 2).ToList().LastOrDefault();
                base_energy_consumption gasConsumption = new();
                gasConsumption = lastGasConsumption;
                gasConsumption.consumption_date = DateTime.Now;
                gasConsumption.create_time = DateTime.Now;
                gasConsumption.modified_time = DateTime.Now;
                gasConsumption.natural_gas_energy_consumption = (lastGasConsumption.natural_gas_energy_consumption*5 - rand.Next(6200,7000))/5;       
                _app5.Insert(gasConsumption);
                Console.WriteLine("添加气数据");
            }

        }
        public void autoaddFunction(exam_plan_method item,string exam_plan_method_rule_id) 
        {
            exam_plan_rec data = new exam_plan_rec();
            #region 逐一赋值
            data.exam_plan_method_id = item.exam_plan_method_id;
            data.exam_plan_method_rule_id = exam_plan_method_rule_id;
            data.create_time = DateTime.Now;
            data.effect_end_time = item.effect_end_time;
            data.effect_start_time = item.effect_start_time;
            //时间要改
            data.modified_time = DateTime.Now;
            data.exam_plan_code = item.exam_plan_method_code;
            data.exam_method_type = item.exam_method_type;
            data.is_stop_machine = item.is_stop_machine;
            data.calendar_type = item.calendar_type;
            data.is_manual = item.is_manual;
            data.description = item.description;
            data.plan_level = item.plan_level;
            data.exam_method_name = item.exam_method_name;
            data.exam_schema_name = item.exam_schema_name;
            data.lead_time = item.lead_time;
            data.lead_time_unit = item.lead_time_unit;
            data.status = item.status;
            data.delete_mark = item.delete_mark;
            data.status = "新建";       
            switch (item.lead_time_unit)//根据提前期 修正触发时间
            {
                case "0"://分钟

                    data.trigger_time = DateTime.Now.
                        AddMinutes(item.lead_time).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "1"://小时
                    data.trigger_time = DateTime.Now.
                      AddHours(item.lead_time).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case "2"://天
                    data.trigger_time = DateTime.Now.
                      AddDays(item.lead_time).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
            }
            //通过计划设备关联表找出该计划对应需要对哪些设备进行点检
            List<exam_plan_method_equipment> exam_Plan_Method_Equipment = _pikachuApp.GetAll<exam_plan_method_equipment>()
                .Where(a => a.exam_plan_method_id == item.exam_plan_method_id).ToList();
            foreach (var  it in exam_Plan_Method_Equipment)
            {
                data.equipment_id = it.equipment_id;
                data.equipment_name = _pikachuApp.GetById<base_equipment>(it.equipment_id)?.equipment_name;
                exam_plan_rec returnData = _app.InsertExamPlanRec(data);
                Console.WriteLine("自动添加计划记录函数执行一次");

                //找该计划对应的点检项目
                List<exam_plan_method_item> ItemDataList = _app3.GetItemByPlanId(item.exam_plan_method_id)
                                                           .Where(a=>a.equipment_id==data.equipment_id).ToList();
                foreach (var ItemData in ItemDataList)
                {
                    exam_plan_rec_item RecItemData = new exam_plan_rec_item();
                    RecItemData.exam_plan_rec_id = data.exam_plan_rec_id;
                    RecItemData.examschema_id = ItemData.examschema_id;
                    RecItemData.examitem_id = ItemData.examitem_id;
                    RecItemData.examitem_code = ItemData.examitem_code;
                    RecItemData.examitem_name = ItemData.examitem_name;
                    RecItemData.examitem_std = ItemData.examitem_std;
                    RecItemData.value_type = ItemData.value_type;
                    //RecItemData.result = obj.result;

                    exam_plan_rec_item returnPlanRecItem = _app4.InsertExamPlanRecItem(RecItemData);
                    Console.WriteLine("自动添加计划函数记录项目执行一次");
                    #endregion
                }
            }


        }
        /// <summary>
        /// 指定时间执行函数
        /// </summary>
        /// <param name="occur_condition"></param>
        /// <param name="AdvanceOccurCondition"></param>
        /// <param name="item"></param>
        private void specifyDateFunction(string occur_condition, string AdvanceOccurCondition, exam_plan_method_rule planMethodRule)
        {
            exam_plan_method item = _pikachuApp.GetById<exam_plan_method>(planMethodRule.exam_plan_method_id);
            //var _exist = _pikachuApp.GetAll<exam_plan_rec>().Where(a => a.exam_method_id == item.exam_method_id
            //   && a.exam_plan_method_id == item.exam_plan_method_id
            //   && a.exam_schema_id == item.exam_schema_id
            //   && a.create_time.Date == DateTime.Now.Date
            ////   ).ToList();  
            //var _exist = _pikachuApp.GetAll<exam_plan_rec>()
            //    .Where(a =>a.exam_plan_method_rule_id==planMethodRule.exam_plan_method_rule_id&& a.create_time.Date == DateTime.Now.Date).ToList();
            var _exist = _pikachuApp.GetByOneFeildsSql<exam_plan_rec>("exam_plan_method_rule_id", planMethodRule.exam_plan_method_rule_id)
                  .Where(a => a.create_time.Date == DateTime.Now.Date).ToList(); 
            if (_exist.Count==0 && AdvanceOccurCondition == occur_condition )//指定时间 一天添加一次 
            {
                autoaddFunction(item, planMethodRule.exam_plan_method_rule_id);
                SpecifiedTimeAddFlag = occur_condition;
                Console.WriteLine("添加指定计划记录一次");
            }
        }
        /// <summary>
        /// 月计划执行函数
        /// </summary>
        /// <param name="occur_condition"></param>
        /// <param name="AdvanceOccurCondition"></param>
        /// <param name="item"></param>
        private void monthlylyPlanningFunction(string occur_condition, string AdvanceOccurCondition, exam_plan_method_rule planMethodRule)
        {
            exam_plan_method item = _pikachuApp.GetById<exam_plan_method>(planMethodRule.exam_plan_method_id);
            //var _exist = _pikachuApp.GetAll<exam_plan_rec>()
            //     .Where(a => a.exam_plan_method_rule_id == planMethodRule.exam_plan_method_rule_id && a.create_time.Date == DateTime.Now.Date).ToList();
            var _exist = _pikachuApp.GetByOneFeildsSql<exam_plan_rec>("exam_plan_method_rule_id", planMethodRule.exam_plan_method_rule_id)
                 .Where(a => a.create_time.Date == DateTime.Now.Date).ToList();
            string[] occur_condition_split = occur_condition.Split(",");
            //判断当前的时间是否是触发条件的一种
            bool exists = ((IList)occur_condition_split).Contains(AdvanceOccurCondition);
            if (_exist.Count==0&&exists)// && (MonthlyAddFlag != AdvanceOccurCondition || DateTime.Now.Month != LastExecutionMonth)
            {

                Console.WriteLine("添加月计划记录一次");
                autoaddFunction(item, planMethodRule.exam_plan_method_rule_id);
                MonthlyAddFlag = AdvanceOccurCondition;
                LastExecutionMonth = DateTime.Now.Month;
            }
        
        }
        /// <summary>
        /// 周计划执行函数
        /// </summary>
        /// <param name="occur_condition"></param>
        /// <param name="AdvanceOccurCondition"></param>
        /// <param name="item"></param>
        private void weeklyPlanningFunction(string occur_condition, string AdvanceOccurCondition, exam_plan_method_rule planMethodRule)
        {
            exam_plan_method item = _pikachuApp.GetById<exam_plan_method>(planMethodRule.exam_plan_method_id);
            string DigitalWeek =null;
            switch (AdvanceOccurCondition)
            {
                #region 将英文数字转化为数字
                case "Monday":
                    DigitalWeek = "1";
                    break;
                case "Tuesday":
                    DigitalWeek = "2";
                    break;
                case "Wednesday":
                    DigitalWeek = "3";
                    break;
                case "Thursday":
                    DigitalWeek = "4";
                    break;
                case "Friday":
                    DigitalWeek = "5";
                    break;
                case "Saturday":
                    DigitalWeek = "6";
                    break;
                case "Sunday":
                    DigitalWeek = "7";
                    break;
                    #endregion
            }
           string[] occur_condition_split = occur_condition.Split(",");
            //var _exist = _pikachuApp.GetAll<exam_plan_rec>()
            //     .Where(a => a.exam_plan_method_rule_id == planMethodRule.exam_plan_method_rule_id && a.create_time.Date == DateTime.Now.Date).ToList();
            var _exist = _pikachuApp.GetByOneFeildsSql<exam_plan_rec>("exam_plan_method_rule_id", planMethodRule.exam_plan_method_rule_id)
                 .Where(a => a.create_time.Date == DateTime.Now.Date).ToList();
            //判断当前的时间是否是触发条件的一种
            bool exists = ((IList)occur_condition_split).Contains(DigitalWeek);
            if (_exist.Count == 0&&exists)//考虑到每周几单天运行的情况  && (WeeklyAddFlag != DigitalWeek|| DateTime.Now.Day != LastExecutionDate)
            {
                Console.WriteLine("添加周计划记录一次");
                autoaddFunction(item, planMethodRule.exam_plan_method_rule_id);
                WeeklyAddFlag = DigitalWeek;
                LastExecutionDate = DateTime.Now.Day;
               
            }

        }
        /// <summary>
        /// 间隔天数计划执行函数
        /// </summary>
        /// <param name="occur_condition"></param>
        /// <param name="ComparetimeAdvance"></param>
        /// <param name="item"></param>
        private void dailyScheduleFunction(string occur_condition,  DateTime ComparetimeAdvance, exam_plan_method_rule planMethodRule)
        {
            exam_plan_method item = _pikachuApp.GetById<exam_plan_method>(planMethodRule.exam_plan_method_id);
            if ((DateTime.Now - ComparetimeAdvance).Days.ToString() == occur_condition || 
                (DateTime.Now - LastExecutionTime).Days.ToString() == occur_condition)
            {
                // var _exist = _pikachuApp.GetAll<exam_plan_rec>()
                //.Where(a => a.exam_plan_method_rule_id == planMethodRule.exam_plan_method_rule_id && a.create_time.Date == DateTime.Now.Date).ToList();
                var _exist = _pikachuApp.GetByOneFeildsSql<exam_plan_rec>("exam_plan_method_rule_id", planMethodRule.exam_plan_method_rule_id)
                  .Where(a => a.create_time.Date == DateTime.Now.Date).ToList();
                //防止一天添加多次
                if (_exist.Count == 0)//&& LastExecutionTime.Day != DateTime.Now.Day
                {
                    Console.WriteLine("添加间隔天数计划记录一次"); 
                    autoaddFunction(item, planMethodRule.exam_plan_method_rule_id);
                    //DailyAddFlag = AdvanceOccurCondition;
                    //LastExecutionDate = DateTime.Now.Day;
                    LastExecutionTime = DateTime.Now;
                }
            }
            
        }

    }

}



