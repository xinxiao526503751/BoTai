using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Fake
{
    /// <summary>
    /// 漂亮接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class LaughController
    {

        public class data
        {
            //设备名称
            public string equipment_name;
            //事件内容
            public string fault;
            //发生时间
            public DateTime occurrenceTime;
            //持续时间
            public int durationTime;
            //状态
            public string state;
            //负责人
            public string personInCharge;
        }

        /// <summary>
        /// 漂亮接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<data>> BeautifulApi()
        {
            Response<List<data>> result = new Response<List<data>>();
            try
            {
                List<data> datas = new();
                data data1 = new()
                {
                    equipment_name = "底纸产线",
                    fault = "启动时振动",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(07, 45, 20))),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data2 = new()
                {
                    equipment_name = "面纸产线",
                    fault = "润滑油过量",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(07, 55, 12))),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data3 = new()
                {
                    equipment_name = "雕刻机",
                    fault = "声音异常",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(08, 20, 35))).ToDate(),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data4 = new()
                {
                    equipment_name = "面纸产线",
                    fault = "风机振动剧烈",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(08, 35, 03))).ToDate(),
                    state = "未处理",
                    personInCharge = "管理员"
                };
                data data5 = new()
                {
                    equipment_name = "磨边机",
                    fault = "磨轮不转动",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(09, 07, 34))).ToDate(),
                    state = "未处理",
                    personInCharge = "管理员"
                };
                data data6 = new()
                {
                    equipment_name = "热压产线",
                    fault = "润滑油外漏",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(09, 29, 56))).ToDate(),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data7 = new()
                {
                    equipment_name = "下料机",
                    fault = "显示报警",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(09, 40, 20))).ToDate(),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data8 = new()
                {
                    equipment_name = "面纸产线",
                    fault = "减速机杂声",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(10, 49, 41))).ToDate(),
                    state = "未处理",
                    personInCharge = "管理员"
                };
                data data9 = new()
                {
                    equipment_name = "雕刻机",
                    fault = "通信异常",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(13, 02, 39))).ToDate(),
                    state = "未处理",
                    personInCharge = "管理员"
                };
                data data10 = new()
                {
                    equipment_name = "底纸产线",
                    fault = "电机电流过大",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(13, 56, 01))).ToDate(),
                    state = "未处理",
                    personInCharge = "管理员"
                };
                data data11 = new()
                {
                    equipment_name = "下料机",
                    fault = "输送轮故障",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(13, 56, 01))).ToDate(),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                data data12 = new()
                {
                    equipment_name = "热压产线",
                    fault = "轴承升温过高",
                    occurrenceTime = (Time.Now.Date.Add(new TimeSpan(15, 32, 17))).ToDate(),
                    state = "已处理",
                    personInCharge = "管理员"
                };
                datas.Add(data1);
                datas.Add(data2);
                datas.Add(data3);
                datas.Add(data4);
                datas.Add(data5);
                datas.Add(data6);
                datas.Add(data7);
                datas.Add(data8);
                datas.Add(data9);
                datas.Add(data10);
                datas.Add(data11);
                datas.Add(data12);
                List<data> returnData = new();
                foreach (data data in datas)
                {
                    if (data.occurrenceTime < Time.Now)
                    {
                        data.durationTime = (data.occurrenceTime - Time.Now).ToInt() / 1000;
                        returnData.Add(data);
                    }
                }
                result.Result = returnData;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

    }
}
