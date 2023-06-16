using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Fault
{
    /// <summary>
    /// 事件记录控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Fault", IgnoreApi = false)]
    public class FaultRecordController : ControllerBase
    {
        private readonly FaultRecordApp _app;
        private readonly FaultRecordFlowApp _faultRecordFlowApp;
        private readonly PikachuApp _pikachuApp;
        private readonly IMapper _mapper;
        private readonly IAuth _auth;
        public FaultRecordController(FaultRecordApp app, PikachuApp picachuApp, IMapper mapper, FaultRecordFlowApp faultRecordFlowApp, IAuth auth)
        {
            _app = app;
            _pikachuApp = picachuApp;
            _mapper = mapper;
            _faultRecordFlowApp = faultRecordFlowApp;
            _auth = auth;
        }
        /// <summary>
        /// 上报事件,生成事件记录的同时生成事件记录流程
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> ReportFault(List<FaultReportRequest> obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                //Console.WriteLine("ReportFault事务外层进入");
                bool data = _app.InsertFaultRecord(obj);
                if (data)
                {
                    result.Result = "操作成功";
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
            //Console.WriteLine("ReportFault事务外层出");
            return result;
        }

        /// <summary>
        /// 点击处理事件按钮以后弹出的处理界面
        /// 将选中的流程对应的最新状态返回给前端
        /// </summary>
        /// <param name="dealRecordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<DealRecordResponse> DealFault(DealRecordRequest dealRecordRequest)
        {
        
            Response<DealRecordResponse> result = new Response<DealRecordResponse>();
            try
            {
                DealRecordResponse dealRecordResponse = new();
                _mapper.Map(dealRecordRequest, dealRecordResponse);

                //根据事件记录id找到事件记录
                fault_record fault_Record = _pikachuApp.GetByOneFeildsSql<fault_record>("fault_record_id", dealRecordRequest.fault_record_id).FirstOrDefault();
                if (fault_Record == null)
                {
                    throw new Exception("未能找到当前流程对应的事件记录");
                }

                //找到事件记录对应的流程
                List<fault_record_flow> fault_Record_Flow =
                    _pikachuApp.GetByOneFeildsSql<fault_record_flow>("fault_record_id", fault_Record.fault_record_id);

                base_fault base_Fault = _pikachuApp.GetByOneFeildsSql<base_fault>("fault_name", dealRecordRequest.fault_name).FirstOrDefault();
                fault_record fault_Record1 = _pikachuApp.GetByOneFeildsSql<fault_record>("fault_id", base_Fault.fault_id).FirstOrDefault();
                dealRecordResponse.create_time = fault_Record1.create_time;
                dealRecordResponse.report_people = base_Fault.create_by_name;

                //确认流程
                fault_record_flow flow = fault_Record_Flow.Where(c => c.flow_seq == 1).FirstOrDefault();
                if (flow.is_finish == 0)//未完成确认
                {
                    dealRecordResponse.active = 0;
                }
                else if (flow.is_finish == 1)
                {
                    dealRecordResponse.active = 1;//完成确认
                }
                //找处理流程
                flow = fault_Record_Flow.Where(c => c.flow_seq == 2).FirstOrDefault();
                if (flow?.is_finish == 1)//完成处理
                {
                    dealRecordResponse.active = 2;
                }
                flow = fault_Record_Flow.Where(c => c.flow_seq == 3).FirstOrDefault();
                if (flow?.is_finish == 1)//完成关闭
                {
                    dealRecordResponse.active = 3;
                }

                result.Result = dealRecordResponse;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }



        /// <summary>
        /// 请求已有的异常事件记录,可根据设备名称分类查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> FaultRecordRequest(string equipment_id)
        {
            try
            {
               // Console.WriteLine("FaultRecordRequest事件外层进入");
                Response<PageData> result = new Response<PageData>();
                PageData pd = new();

                try
                {
                    List<FaultRecordResponse> faultRecordResponses = _app.FaultRecordRequest(equipment_id);
                    pd.Data = faultRecordResponses;
                    pd.Total = pd.Data.Count;
                    result.Result = pd;
                }
                catch (Exception ex)
                {
                    result.Code = 500;
                    result.Message = ex.InnerException?.Message ?? ex.Message;
                }
               // Console.WriteLine("FaultRecordRequest事件外层出");
                return result;
            }
            catch(Exception ex)
            {
                //Console.WriteLine($"{ex.Message}");
                Response<PageData> result = new Response<PageData>();
                return result;
            }
           
        }



        /// <summary>
        /// 异常事件统计页面的飘带图
        /// 查询日期范围(year-mouth ~ year-mouth)内上报的异常事件 类型 的 数量统计
        /// </summary>
        /// <param name="StartTime">years-mouth</param>
        /// <param name="EndTime">years-mouth</param>
        /// <returns></returns>
        [HttpPost]
        public Response<FaultRecordStaticsResponse> GetProductsNumberByTimeRibbonChart(string StartTime, string EndTime)
        {
            Response<FaultRecordStaticsResponse> result = new Response<FaultRecordStaticsResponse>();
            try
            {
                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
                {
                    throw new Exception("请选择时间");
                }

                FaultRecordStaticsResponse data = new();

                DateTime Start = (StartTime + " 00:00:00").ToDate();
                DateTime End = (EndTime + " 23:59:59").ToDate();

                string[] SplitArray = EndTime.Split('-');//由于结束的月份天数不好确定，需要判断

                //大月
                string[] BigMouth = { "01", "03", "05", "10", "12" };
                //小月
                string[] SmallMouth = { "04", "06", "07", "08", "09", "11" };
                //平月
                string[] nonleapMouth = { "02" };

                //如果月份是大月
                if (BigMouth.Contains(SplitArray[1]))
                {
                    End = (EndTime + " 23:59:59").ToDate();
                }
                else if (SmallMouth.Contains(SplitArray[1]))
                {
                    End = (EndTime + " 23:59:59").ToDate();
                }
                else if (nonleapMouth.Contains(SplitArray[1]))
                {
                    //闰年
                    if (SplitArray[0].ToInt() % 400 == 0)//能被400整除是闰年
                    {
                        End = (EndTime + " 23:59:59").ToDate();
                    }
                    //能被4整除不能被100整除的是平年
                    else if (SplitArray[0].ToInt() % 4 == 0 && SplitArray[0].ToInt() % 100 != 0)
                    {
                        End = (EndTime + " 23:59:59").ToDate();
                    }
                }

                data = _app.GetProductsNumberByTimeRibbonChartApp(Start, End);
                result.Result = data;
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
