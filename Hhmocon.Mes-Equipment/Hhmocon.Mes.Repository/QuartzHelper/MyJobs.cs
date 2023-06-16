using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.CommunicationUtil;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// IJob实例.
    /// 用进行事件通知配置
    /// </summary>
    [DisallowConcurrentExecution]
    public class MyJobs : IJob
    {
        private readonly ILogger<MyJobs> _logger;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IFaultRecordFlowRepository _faultRecordFlowRepository;

        public MyJobs(ILogger<MyJobs> logger, PikachuRepository pikachuRepository, FaultRecordFlowRepository faultRecordFlowRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pikachuRepository = pikachuRepository;
            _faultRecordFlowRepository = faultRecordFlowRepository;
        }


        /// <summary>
        /// 重写IJob接口的Execute方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    Console.WriteLine("开始执行");
                    //需要被通知的用户的id
                    List<string> WechatNoticeUserId = new();
                    //根据所有未完成确认流程 且 未完成事件通知 找到涉及的事件id
                    List<string> fault_ids = _faultRecordFlowRepository.GetRecordsByFlowSeq1(conn, transaction).ToList();
                    foreach (string id in fault_ids)
                    {
                        //找到和事件对应的通知配置
                        List<base_fault_noticecfg> base_Fault_Noticecfgs = _pikachuRepository.GetByOneFeildsSql<base_fault_noticecfg>("fault_id", id, tran: transaction, dbConnection: conn);
                        List<string> notice_people = new();
                        foreach (base_fault_noticecfg cfg in base_Fault_Noticecfgs)
                        {
                            //找到通知配置对应的人员
                            WechatNoticeUserId.Add(cfg.user_id);
                        }
                    }

                    //根据未完成确认流程 且 未完成事件通知 找到涉及的流程
                    List<fault_record_flow> fault_Record_Flows = _faultRecordFlowRepository.GetFlowsByFlowSeq1(conn, transaction);

                    //遍历所有需要通知的流程
                    foreach (fault_record_flow temp in fault_Record_Flows)
                    {
                        //找到流程的fault_id对应的通知配置
                        //获取所有通知配置
                        List<base_fault_noticecfg> base_Fault_NoticecfgsAall = _pikachuRepository.GetByOneFeildsSql<base_fault_noticecfg>("fault_id", temp.fault_id, tran: transaction, dbConnection: conn);
                        //遍历通知配置
                        foreach (base_fault_noticecfg base_Fault_Noticecfg in base_Fault_NoticecfgsAall)
                        {
                            DateTime dateTime = Time.Now;
                            //当前时间与流程创建时间的差值
                            TimeSpan timeSpan = Time.Now - temp.create_time;
                            switch (base_Fault_Noticecfg.notice_level)//根据不同通知等级判断是否达到执行时间段
                            {
                                //通知等级为0 即时通知
                                case 0:
                                    {
                                        sys_user sys_User = _pikachuRepository.GetByOneFeildsSql<sys_user>("user_id", base_Fault_Noticecfg.user_id, tran: transaction, dbConnection: conn).FirstOrDefault();
                                        if (sys_User == null)
                                        {
                                            throw new Exception($"未能找到用户，id:{sys_User.user_webchat}");
                                        }
                                        WechatNoticeUserId.Add(sys_User.user_webchat);
                                    }
                                    break;

                                //通知等级为1  半小时后通知
                                case 1:
                                    {//只检查当天的内容
                                        if (base_Fault_Noticecfg.create_time.Date == dateTime.Date)//按理说所有数据都应该是当天的，因为通知最迟延时一个半小时
                                        {
                                            //数据库要求至少能一分钟检查一次
                                            if (timeSpan.Minutes > 30 && timeSpan.Minutes < 31)
                                            {
                                                switch (base_Fault_Noticecfg.notice_type)//通知类型
                                                {
                                                    case "webchat":
                                                        sys_user sys_User = _pikachuRepository.GetByOneFeildsSql<sys_user>("user_id", base_Fault_Noticecfg.user_id, tran: transaction, dbConnection: conn).FirstOrDefault();
                                                        if (sys_User == null)
                                                        {
                                                            throw new Exception($"未能找到用户，id:{base_Fault_Noticecfg.user_id}");
                                                        }
                                                        WechatNoticeUserId.Add(sys_User.user_webchat);
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                    break;

                                //通知等级为2,一小时后通知
                                case 2:
                                    {
                                        if (base_Fault_Noticecfg.create_time.Date == dateTime.Date)//只检查当天的内容，按理说所有数据都应该是当天的，因为通知最迟延时一个半小时
                                        {
                                            //数据库要求至少能一分钟检查一次
                                            if (timeSpan.Minutes > 60 && timeSpan.Minutes < 61)
                                            {
                                                switch (base_Fault_Noticecfg.notice_type)//通知类型
                                                {
                                                    case "webchat":
                                                        sys_user sys_User = _pikachuRepository.GetByOneFeildsSql<sys_user>("user_id", base_Fault_Noticecfg.user_id, tran: transaction, dbConnection: conn).FirstOrDefault();
                                                        if (sys_User == null)
                                                        {
                                                            throw new Exception($"未能找到用户，id:{base_Fault_Noticecfg.user_id}");
                                                        }
                                                        WechatNoticeUserId.Add(sys_User.user_webchat);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;

                                //通知等级为3,一个半小时后通知
                                case 3:
                                    {
                                        if (base_Fault_Noticecfg.create_time.Date == dateTime.Date)//只检查当天的内容，按理说所有数据都应该是当天的，因为通知最迟延时一个半小时
                                        {
                                            //数据库要求至少能一分钟检查一次
                                            if (timeSpan.Minutes > 90 && timeSpan.Minutes < 91)
                                            {
                                                switch (base_Fault_Noticecfg.notice_type)//通知类型
                                                {
                                                    case "webchat":
                                                        sys_user sys_User = _pikachuRepository.GetByOneFeildsSql<sys_user>("user_id", base_Fault_Noticecfg.user_id, tran: transaction, dbConnection: conn).FirstOrDefault();
                                                        if (sys_User == null)
                                                        {
                                                            throw new Exception($"未能找到用户，id:{base_Fault_Noticecfg.user_id}");
                                                        }
                                                        WechatNoticeUserId.Add(sys_User.user_webchat);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            //通知操作完成以后要把seq=1的流程的通知标志位置1，代表通知过
                            temp.notice_flag = 1;

                            _logger.LogInformation($"通知操作完成!");
                            conn.Update(temp, tran: transaction);
                        }
                    }
                    string AccessToken = WeCharEnterpriseUtil.GetAccessToken();
                    if (WechatNoticeUserId.Count != 0)
                    {
                        WeCharEnterpriseUtil.PostMail(AccessToken, WechatNoticeUserId, message: $"异常报告");
                    }
                    transaction.Commit();
                    return Task.CompletedTask;//结束线程
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    _logger.LogError(exception.Message);
                    throw new Exception(exception.Message);
                }

            }
        }
    }
}
