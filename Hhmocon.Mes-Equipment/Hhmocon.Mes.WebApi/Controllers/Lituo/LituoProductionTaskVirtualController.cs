using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Lituo;
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
    /// 工序虚拟数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Lituo", IgnoreApi = false)]
    public class LituoProductionTasVirtualController
    {
        private readonly PikachuApp _pikachuApp;
        private readonly LituoProductionTaskVirtualApp _lituoProductionTaskVirtualApp;


        public LituoProductionTasVirtualController(PikachuApp pikachuApp, LituoProductionTaskVirtualApp lituoProductionTaskVirtualApp)
        {
            _pikachuApp = pikachuApp;
            _lituoProductionTaskVirtualApp = lituoProductionTaskVirtualApp;
        }
        /// <summary>
        /// 录入随机数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(lituo_production_task_virtual obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                lituo_production_task_virtual data = _lituoProductionTaskVirtualApp.Insert(obj);
                if (data != null)
                {
                    result.Result = data.process_data_id;
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

    }
}
