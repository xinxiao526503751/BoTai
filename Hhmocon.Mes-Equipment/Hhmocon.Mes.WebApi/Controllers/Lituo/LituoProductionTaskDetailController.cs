using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers
{
    /// <summary>
    /// 力拓生产任务明细表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Lituo", IgnoreApi = false)]
    public class LituoProductionTaskDetailController : ControllerBase
    {

        private readonly PikachuApp _pikachuApp;
        private readonly LituoProductionTaskDetailApp _lituoProductionTaskDetailApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        public LituoProductionTaskDetailController(PikachuApp pikachuApp, LituoProductionTaskDetailApp lituoProductionTaskDetailApp)
        {
            _pikachuApp = pikachuApp;
            _lituoProductionTaskDetailApp = lituoProductionTaskDetailApp;
        }

        /// <summary>
        /// 获取质量原因类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<string>> GetAll()
        {
            Response<List<string>> result = new Response<List<string>>();
            try
            {
                List<lituo_production_task_detail> dataList = _pikachuApp.GetAll<lituo_production_task_detail>();
                List<string> KSXH_List = (from data in dataList
                                          select data.KSXH).Distinct().ToList();

                result.Result = KSXH_List;
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
