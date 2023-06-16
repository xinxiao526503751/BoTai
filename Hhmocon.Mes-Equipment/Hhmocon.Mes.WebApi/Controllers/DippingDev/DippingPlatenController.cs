using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.DippingDev;
using Hhmocon.Mes.Util;
using hmocon.Mes.Repository.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.DippingDev
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class DippingPlatenController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly DippingPlatenApp _platenApp;
        public DippingPlatenController(PikachuApp picachuApp, DippingPlatenApp platenApp)
        {
            _pikachuApp = picachuApp;
            _platenApp = platenApp;
        }

        [HttpPost]
        public Response<string> Create(dipping_platen_data obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                dipping_platen_data data = _platenApp.Insert(obj);
                if (data != null)
                {
                    result.Result = data.dipping_platen_code;
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
        /// 看板接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<dipping_platen_data> GetDataLast()
        {
            Response<dipping_platen_data> result = new Response<dipping_platen_data>();
            try
            {
                result.Result = _platenApp.GetDataLast();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 前端要求需要list形式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<dipping_platen_data>> GetDataLast2()
        {
            Response<List<dipping_platen_data>> result = new Response<List<dipping_platen_data>>();
            try
            {
                result.Result = _platenApp.GetDataLast2();
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
