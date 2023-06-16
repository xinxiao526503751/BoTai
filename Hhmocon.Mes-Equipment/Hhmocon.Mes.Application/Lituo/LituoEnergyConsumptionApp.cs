using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Lituo
{
    public class LituoEnergyConsumptionApp
    {

        private PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public LituoEnergyConsumptionApp(PikachuRepository pikachuRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }
        public base_energy_consumption Insert(base_energy_consumption data)
        {
          
            data.energy_consumption_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            //data.create_by = _auth.GetUserAccount(null);
            //data.create_by_name = _auth.GetUserName(null);
            //data.modified_by = _auth.GetUserAccount(null);
            //data.modified_by_name = _auth.GetUserName(null);
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
          

        }

        public EnergyConsumptionResponse GetNumOfEnergyConsumption(EnergyConsumptionOutputDate outputDate, int ResourceCategory)
        {
            List<base_energy_consumption> aLLConsumption = _pikachuRepository.GetAll<base_energy_consumption>().Where(a=>a.energy_flag== ResourceCategory).ToList();
            EnergyConsumptionResponse enrgyConsumption = new();
            if (ResourceCategory == 0)
            {
                int totalClass1 = 0;
                int totalClass2 = 0;
                int totalClass3 = 0;
                enrgyConsumption.ConsumptionOrNum_1 = new();
                enrgyConsumption.ConsumptionOrNum_2 = new();
                enrgyConsumption.ConsumptionOrNum_3 = new();
                enrgyConsumption.dateString = new();
                enrgyConsumption.total_day = new();
                enrgyConsumption.dateString.Add("能源种类");
                enrgyConsumption.ConsumptionOrNum_1.Add("压机电能耗（/kW·h）");
                enrgyConsumption.ConsumptionOrNum_2.Add("面纸电能耗（/kW·h）");
                enrgyConsumption.ConsumptionOrNum_3.Add("底纸电能耗（/kW·h）");
                enrgyConsumption.total_day.Add("当天总计");
                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a => a.create_time).LastOrDefault();
                    //int lastpowerConsumption = base_Energy.power_consumption; 
                    //int lastWaterConsumption = base_Energy.water_consumption;
                    //int lastGasConsumption = base_Energy.natural_gas_energy_consumption;
                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1=0;
                        int consumption_2=0;
                        int consumption_3=0;
                        if (last_Engery != null)
                        {
                            consumption_1 = Math.Abs(last_Engery.power_consumption_1 - base_Energy.power_consumption_1)*117;
                            consumption_2 = Math.Abs(last_Engery.power_consumption_2 - base_Energy.power_consumption_2)*119;
                            consumption_3 = Math.Abs(last_Engery.power_consumption_3 - base_Energy.power_consumption_3)*121;
                            totalClass1 += consumption_1;
                        }
                        else
                        {
                            consumption_1 = Math.Abs(base_Energy.power_consumption_1);
                            consumption_2 = Math.Abs(base_Energy.power_consumption_2);
                            consumption_3 = Math.Abs(base_Energy.power_consumption_3);
                        }
                            totalClass1 += consumption_1;
                        totalClass2 += consumption_2;
                        totalClass3 += consumption_3;
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.ConsumptionOrNum_2.Add((consumption_2).ToString());
                        enrgyConsumption.ConsumptionOrNum_3.Add((consumption_3).ToString());
                        enrgyConsumption.total_day.Add((consumption_1 + consumption_2 + consumption_3).ToString());

                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }

                } 
                        enrgyConsumption.ConsumptionOrNum_1.Add((totalClass1.ToString()));
                        enrgyConsumption.ConsumptionOrNum_2.Add((totalClass2.ToString()));
                        enrgyConsumption.ConsumptionOrNum_3.Add((totalClass3.ToString()));
                        enrgyConsumption.dateString.Add("能源总计");
                        enrgyConsumption.total_day.Add((totalClass1 + totalClass2+ totalClass2).ToString());
            }
                if (ResourceCategory == 1)
                {
                int totalClass1 = 0;
                int totalClass2 = 0;
                enrgyConsumption.ConsumptionOrNum_1 = new();
                enrgyConsumption.ConsumptionOrNum_2 = new();
                enrgyConsumption.dateString = new();
                enrgyConsumption.total_day = new();
                enrgyConsumption.dateString.Add("能源种类");
                enrgyConsumption.ConsumptionOrNum_1.Add("冷却水消耗（/m³）");
                enrgyConsumption.ConsumptionOrNum_2.Add("加热水消耗（/m³)");
                enrgyConsumption.total_day.Add("当天总计");
                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a=>a.create_time).LastOrDefault();
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();
                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1 = 0;
                        int consumption_2 = 0;
                        if (last_Engery != null )
                        {
                            consumption_1 = Math.Abs(last_Engery.water_consumption_1 - base_Energy.water_consumption_1);
                            consumption_2 = Math.Abs(last_Engery.water_consumptio_2 - base_Energy.water_consumptio_2);

                            totalClass1 += consumption_1;
                        }
                        else
                        {
                            consumption_1 = Math.Abs(base_Energy.water_consumption_1);
                            consumption_2 = Math.Abs(base_Energy.water_consumptio_2);
                        }
                        totalClass1 += consumption_1;
                        totalClass2 += consumption_2;
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.ConsumptionOrNum_2.Add((consumption_2).ToString());
                        enrgyConsumption.total_day.Add((consumption_1 + consumption_2).ToString());

                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }

                }
                enrgyConsumption.ConsumptionOrNum_1.Add((totalClass1.ToString()));
                enrgyConsumption.ConsumptionOrNum_2.Add((totalClass2.ToString()));
                enrgyConsumption.dateString.Add("能源总计");
                enrgyConsumption.total_day.Add((totalClass1 + totalClass2).ToString());
                return enrgyConsumption;
            }

            if (ResourceCategory == 2)
            {
                int totalClass = 0;
                enrgyConsumption.ConsumptionOrNum_1 = new();             
                enrgyConsumption.dateString = new();
                enrgyConsumption.total_day = new();
                enrgyConsumption.dateString.Add("能源种类");
                enrgyConsumption.ConsumptionOrNum_1.Add("天然气消耗（/m³）");
                enrgyConsumption.total_day.Add("当天总计");

                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a => a.create_time).LastOrDefault();
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();

                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1 = 0;
                        //if (last_Engery != null && enrgyConsumption.ConsumptionOrNum_1.Count == 1)
                        //{
                        //   consumption_1 = Math.Abs(last_Engery.natural_gas_energy_consumption - base_Energy.natural_gas_energy_consumption);
                        //}
                        //else
                        //{
                        //    consumption_1 = Math.Abs(base_Energy.natural_gas_energy_consumption - enrgyConsumption.ConsumptionOrNum_1.LastOrDefault().ToInt());
                        //}
                        //
                        if (last_Engery != null)
                            consumption_1 = Math.Abs(last_Engery.natural_gas_energy_consumption - base_Energy.natural_gas_energy_consumption);
                        else
                        {
                            consumption_1 = Math.Abs(base_Energy.natural_gas_energy_consumption);
                        }
                        totalClass += consumption_1;
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.total_day.Add(consumption_1.ToString());
                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }

                }
                enrgyConsumption.ConsumptionOrNum_1.Add((totalClass.ToString()));
                enrgyConsumption.dateString.Add("能源总计");
                enrgyConsumption.total_day.Add(totalClass.ToString());
                return enrgyConsumption;
            }
            return enrgyConsumption;
        }

        public EnergyConsumptionResponse GetNumOfEnergyConsumption2(EnergyConsumptionOutputDate outputDate, int ResourceCategory)
        {
            List<base_energy_consumption> aLLConsumption = _pikachuRepository.GetAll<base_energy_consumption>().Where(a => a.energy_flag == ResourceCategory).ToList();
            EnergyConsumptionResponse enrgyConsumption = new();
            if (ResourceCategory == 0)
            {
                enrgyConsumption.ConsumptionOrNum_1 = new();
                enrgyConsumption.ConsumptionOrNum_2 = new();
                enrgyConsumption.ConsumptionOrNum_3 = new();
                enrgyConsumption.dateString = new();

                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {

                 
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a => a.create_time).LastOrDefault();
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();
                    //int lastpowerConsumption = base_Energy.power_consumption; 
                    //int lastWaterConsumption = base_Energy.water_consumption;
                    //int lastGasConsumption = base_Energy.natural_gas_energy_consumption;
                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1 = 0;
                        int consumption_2 = 0;
                        int consumption_3 = 0;
                        if (last_Engery != null )
                        {
                            consumption_1 = Math.Abs(last_Engery.power_consumption_1 - base_Energy.power_consumption_1)*117;
                            consumption_2 = Math.Abs(last_Engery.power_consumption_2 - base_Energy.power_consumption_2)*119;
                            consumption_3 = Math.Abs(last_Engery.power_consumption_3 - base_Energy.power_consumption_3)*121;
                        }
                        else
                        {
                            consumption_1 = Math.Abs(base_Energy.power_consumption_1 )*117;
                            consumption_2 = Math.Abs(base_Energy.power_consumption_2 )*119;
                            consumption_3 = Math.Abs(base_Energy.power_consumption_3 )*121;
                        }

                      
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.ConsumptionOrNum_2.Add((consumption_2).ToString());
                        enrgyConsumption.ConsumptionOrNum_3.Add((consumption_3).ToString());

                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }

                }
            }
            if (ResourceCategory == 1)
            {
                enrgyConsumption.ConsumptionOrNum_1 = new();
                enrgyConsumption.ConsumptionOrNum_2 = new();
                enrgyConsumption.dateString = new();
                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a => a.create_time).LastOrDefault();
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();

                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1 = 0;
                        int consumption_2 = 0;
                        if (last_Engery != null )
                        {
                            consumption_1 = Math.Abs(last_Engery.water_consumption_1 - base_Energy.water_consumption_1);
                            consumption_2 = Math.Abs(last_Engery.water_consumptio_2 - base_Energy.water_consumptio_2);
                        }
                        else
                        {

                            consumption_1 = Math.Abs(base_Energy.water_consumption_1);
                            consumption_2 = Math.Abs(base_Energy.water_consumptio_2);
                        }
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.ConsumptionOrNum_2.Add((consumption_2).ToString());

                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }

                }
                return enrgyConsumption;
            }

            if (ResourceCategory == 2)
            {
                enrgyConsumption.ConsumptionOrNum_1 = new();
                enrgyConsumption.dateString = new();

                for (DateTime date = outputDate.start_time.Date; date <= outputDate.end_time.Date; date = date.AddDays(1))
                {
                    base_energy_consumption base_Energy = aLLConsumption.Where(a => a.consumption_date.Date == date.Date).OrderBy(a => a.create_time).LastOrDefault();
                    base_energy_consumption last_Engery = aLLConsumption.Where(a => a.consumption_date.Date < date.Date).OrderBy(a => a.consumption_date).LastOrDefault();

                    ////先筛选当天三种能源的消耗
                    //if (base_Energy != null)
                    //{
                    //    int consumption_1 = Math.Abs(base_Energy.natural_gas_energy_consumption - enrgyConsumption.ConsumptionOrNum_1.LastOrDefault().ToInt());
                    //    enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                    //    enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    //}

                    //先筛选当天三种能源的消耗
                    if (base_Energy != null)
                    {
                        int consumption_1 = 0;
                        //if (last_Engery != null && enrgyConsumption.ConsumptionOrNum_1.Count == 0)
                        //{
                        //    consumption_1 = Math.Abs(last_Engery.natural_gas_energy_consumption - base_Energy.natural_gas_energy_consumption);
                        //}
                        //else
                        //{
                        //    consumption_1 = Math.Abs(base_Energy.natural_gas_energy_consumption - enrgyConsumption.ConsumptionOrNum_1.LastOrDefault().ToInt());
                        //}
                        if (last_Engery != null )
                            consumption_1 = Math.Abs(last_Engery.natural_gas_energy_consumption - base_Energy.natural_gas_energy_consumption);
                        else
                        {
                            consumption_1 = Math.Abs(base_Energy.natural_gas_energy_consumption);
                        }
                        enrgyConsumption.ConsumptionOrNum_1.Add((consumption_1).ToString());
                        enrgyConsumption.dateString.Add(date.Date.ToString("yyyy-MM-dd"));
                    }
                }
                return enrgyConsumption;
            }
            return enrgyConsumption;
        }


    }
}
