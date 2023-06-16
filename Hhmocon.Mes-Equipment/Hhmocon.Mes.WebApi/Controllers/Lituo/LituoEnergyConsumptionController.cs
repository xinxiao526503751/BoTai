using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Lituo;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Lituo
{

    /// <summary>
    /// 力拓能耗控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Lituo", IgnoreApi = false)]
    public class LituoEnergyConsumptionController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly LituoEnergyConsumptionApp _lituoEnergyConsumptionApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        public LituoEnergyConsumptionController(PikachuApp pikachuApp, LituoEnergyConsumptionApp lituoEnergyConsumptionApp)
        {
            _pikachuApp = pikachuApp;
            _lituoEnergyConsumptionApp = lituoEnergyConsumptionApp;
        }


        /// <summary>
        /// 录入能耗数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_energy_consumption obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_energy_consumption data = _lituoEnergyConsumptionApp.Insert(obj);
                if (data != null)
                {
                    result.Result = data.energy_consumption_id;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "数据写入失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 录入能耗数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public void CreateVirtual()
        {
            try
            {
                Random rand = new Random();
                //找有没有当天电的数据
                bool existPower = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.AddDays(-1).Date && a.energy_flag == 0).ToList().Count == 0;
                if (existPower)
                {

                    var lastPowerConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 0).ToList().Last();
                    base_energy_consumption powerConsumption = new();
                    powerConsumption = lastPowerConsumption;
                    powerConsumption.consumption_date = DateTime.Now;
                    powerConsumption.create_time = DateTime.Now;
                    powerConsumption.modified_time = DateTime.Now;
                    powerConsumption.power_consumption_1 = lastPowerConsumption.power_consumption_1 + rand.Next(3, 8);
                    powerConsumption.power_consumption_2 = lastPowerConsumption.power_consumption_2 + rand.Next(4, 10);
                    powerConsumption.power_consumption_3 = lastPowerConsumption.power_consumption_3 + rand.Next(6, 10);
                    _lituoEnergyConsumptionApp.Insert(powerConsumption);
                    Console.WriteLine("添加电数据");

                }
                bool existWater = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date && a.energy_flag == 1).ToList().Count == 0;
                if (existWater)
                {
                    base_energy_consumption lastWaterConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 1).ToList().Last();
                    base_energy_consumption waterConsumption = new();
                    waterConsumption = lastWaterConsumption;
                    waterConsumption.consumption_date = DateTime.Now;
                    waterConsumption.create_time = DateTime.Now;
                    waterConsumption.modified_time = DateTime.Now;
                    waterConsumption.water_consumption_1 = lastWaterConsumption.water_consumption_1 + rand.Next(3, 6);
                    waterConsumption.water_consumptio_2 = lastWaterConsumption.water_consumptio_2 + rand.Next(35, 56);
                    _lituoEnergyConsumptionApp.Insert(waterConsumption);
                    Console.WriteLine("添加水数据");
                }

                bool existGas = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date && a.energy_flag == 2).ToList().Count == 0;
                if (existGas)
                {
                    base_energy_consumption lastGasConsumption = _pikachuApp.GetAll<base_energy_consumption>().Where(a => a.consumption_date.Date == DateTime.Now.Date.AddDays(-1) && a.energy_flag == 2).ToList().Last();
                    base_energy_consumption gasConsumption = new();
                    gasConsumption = lastGasConsumption;
                    gasConsumption.consumption_date = DateTime.Now;
                    gasConsumption.create_time = DateTime.Now;
                    gasConsumption.modified_time = DateTime.Now;
                    gasConsumption.natural_gas_energy_consumption = (lastGasConsumption.natural_gas_energy_consumption * 5 - rand.Next(6200, 7000)) / 5;
                    _lituoEnergyConsumptionApp.Insert(gasConsumption);
                    Console.WriteLine("添加气数据");
               }
              
            }
            catch (Exception ex)
            {            
            }
        }

        /// <summary>
        /// 根据时间获取能耗数据
        /// </summary>
        /// <param name="processoutputDate">数据的时间段</param>
        /// <param name="ResourceCategory">0是电源数据，1是水数据，2是天然气数据</param>
        /// <returns></returns>
        [HttpPost]
        public Response<EnergyConsumptionResponse> GetNumOfEnergyConsumption(EnergyConsumptionOutputDate processoutputDate, int ResourceCategory, bool lineChart = false)
        {
            Response<EnergyConsumptionResponse> result = new Response<EnergyConsumptionResponse>();
            try
            {
                if (processoutputDate.start_time > processoutputDate.end_time)
                {
                    throw new Exception("开始时间不能晚于结束时间");
                }

                if (lineChart)
                {
                    result.Result = _lituoEnergyConsumptionApp.GetNumOfEnergyConsumption2(processoutputDate, ResourceCategory);
                }
                else
                {
                    result.Result = _lituoEnergyConsumptionApp.GetNumOfEnergyConsumption(processoutputDate, ResourceCategory);
                }
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
