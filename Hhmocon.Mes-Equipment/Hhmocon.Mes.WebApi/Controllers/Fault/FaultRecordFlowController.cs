using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Fault
{
    /// <summary>
    /// 事件记录流程控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Fault", IgnoreApi = false)]
    public class FaultRecordFlowController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly FaultRecordFlowApp _faultRecordFlowApp;
        private readonly IAuth _auth;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="faultRecordFlowApp"></param>
        /// <param name="auth"></param>
        public FaultRecordFlowController(PikachuApp pikachuApp, FaultRecordFlowApp faultRecordFlowApp, IAuth auth)
        {
            _pikachuApp = pikachuApp;
            _faultRecordFlowApp = faultRecordFlowApp;
            _auth = auth;
        }

        /// <summary>
        /// 根据时间记录id获取已经执行的流程的
        /// 执行人和执行时间
        /// </summary>
        /// <param name="fault_record_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<FaultDealPeopleAndDealTime> GetAlreadyExistsFlowByFaultRecordId(string fault_record_id)
        {
            Response<FaultDealPeopleAndDealTime> result = new Response<FaultDealPeopleAndDealTime>();
            try
            {
                FaultDealPeopleAndDealTime data = _faultRecordFlowApp.GetAlreadyExistsFlowByFaultRecordId(fault_record_id);
                result.Result = data;
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 处理异常流程
        /// </summary>
        /// <param name="fault_record_id">异常记录id</param>
        /// <param name="operationCode">0确认 1处理 2关闭</param>
        /// <param name="flow_info">信息</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> DealFaultFlow(DealFaultFlowRequest dealFaultFlowRequest)
        {
            Response<string> result = new Response<string>();
            try
            {
                bool re = _faultRecordFlowApp.DealFaultFlow(dealFaultFlowRequest.fault_record_id, dealFaultFlowRequest.operationCode, dealFaultFlowRequest.flow_info);
                if (re)
                {
                    result.Result = "操作成功";
                }
                else
                {
                    result.Result = "操作失败";
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
