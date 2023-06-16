using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.DippingDev;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Util;
using hmocon.Mes.Repository.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.DippingDev
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class DippingDevController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly DippingDevApp _devApp;
        public DippingDevController(PikachuApp picachuApp, DippingDevApp devApp)
        {
            _pikachuApp = picachuApp;
            _devApp = devApp;
        }
        [HttpPost]
        public Response<string> Create(dipping_dev_data obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                dipping_dev_data data = _devApp.Insert(obj);
                if (data != null)
                {
                    result.Result = data.dipping_dev_code;
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


        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _devApp.GetList<dipping_dev_data_pro>(req, ref lcount);
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        public class returnData
        {
            public string equipment_Name;
            public string value_Name;
            public string value;
            public string collectionTime;
        }

        /// <summary>
        /// 变量查询页面
        /// </summary>
        /// <param name="dippingQueryRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetDippingDev(DippingQueryRequest dippingQueryRequest)
        {

            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new();
                List<returnData> rr = new();
                if (dippingQueryRequest.VariableCode == null || dippingQueryRequest.EquipmentName == null)
                {
                    throw new Exception("请填写设备名称和变量名称");
                }


                if (dippingQueryRequest.EquipmentName != "压板")
                {

                    List<dipping_dev_data> dipping_Dev_Datas = _devApp.GetDippingDevs(dippingQueryRequest).OrderByDescending(c => c.datatime).ToList();
                    pd.Total = dipping_Dev_Datas.Count;


                    List<dipping_dev_data> dd = dipping_Dev_Datas?.Skip((dippingQueryRequest.iPage - 1) * dippingQueryRequest.iRows).Take(dippingQueryRequest.iRows).ToList();

                    foreach (dipping_dev_data d in dd)
                    {
                        returnData a = new();
                        a.equipment_Name = dippingQueryRequest.EquipmentName;
                        a.value_Name = dippingQueryRequest.VariableCode;
                        a.collectionTime = d.datatime.ToString();
                        a.value = d.GetType().GetProperty(dippingQueryRequest.VariableCode).GetValue(d).ToString();
                        rr.Add(a);
                    }
                }
                else
                {

                    List<dipping_platen_data> dipping_Platen_Datas = _devApp.GetPlatenDevs(dippingQueryRequest).OrderByDescending(c => c.datetime).ToList();
                    pd.Total = dipping_Platen_Datas.Count;


                    List<dipping_platen_data> dd = dipping_Platen_Datas?.Skip((dippingQueryRequest.iPage - 1) * dippingQueryRequest.iRows).Take(dippingQueryRequest.iRows).ToList();

                    foreach (dipping_platen_data d in dd)
                    {
                        returnData a = new();
                        a.equipment_Name = dippingQueryRequest.EquipmentName;
                        a.value_Name = dippingQueryRequest.VariableCode;
                        a.collectionTime = d.datetime.ToString();
                        a.value = d.GetType().GetProperty(dippingQueryRequest.VariableCode).GetValue(d).ToString();
                        rr.Add(a);
                    }
                }

                //分页
                pd.Data = rr;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取最底纸面纸最新的一条数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<dipping_dev_data>> GetDippingDataLast(string dippingDevCode)
        {
            Response<List<dipping_dev_data>> result = new Response<List<dipping_dev_data>>();
            try
            {
                result.Result = _devApp.GetDippinggDataLast(dippingDevCode);
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
