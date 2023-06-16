/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// QuartzJobRunner任务  用来处理正在执行的IJob的整个生命周期：它从容器中获取，执行并释放它（在释放范围时）。
    /// </summary>
    [DisallowConcurrentExecution]
    public class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScheduler _scheduler;//创建作业调度池
        private readonly ILogger<QuartzJobRunner> _logger;
        public QuartzJobRunner(IServiceProvider serviceProvider, IScheduler scheduler, ILogger<QuartzJobRunner> logger)
        {
            _serviceProvider = serviceProvider;
            _scheduler = scheduler;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //【创建局部容器】
            #region 非异步，无调度池
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IJobDetail detail = JobBuilder.Create<DemoJob>().WithIdentity("jobsmal", "job").Build();//创建和任务对应的JobDetail实例
                Type jobType = detail.JobType;
                IJob job = scope.ServiceProvider.GetRequiredService(jobType) as IJob;//从局部容器中获取任务实例
                job.Execute(context);//直接执行任务实例
            }
            #endregion

            #region 异步 调度池
            ////【创建局部容器】
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    JobKey jobKey = new JobKey("jobsmail2", "job");//创建JobKey
            //    Task<bool> bo = _scheduler.CheckExists(jobKey);//检查作业调度池中是否已存在同名同组job
            //    if (await bo == false)
            //    {
            //        List<string> a = new List<string> { "a", "b", "c" };
            //        JobDataMap jobDataMap = new JobDataMap();
            //        jobDataMap.Put("myObjData", a);
            //        IJobDetail detail = JobBuilder.Create<DemoJob>().WithIdentity("jobsmail2", "job").UsingJobData(jobDataMap).Build();
            //        var jobType = detail.JobType;
            //        var trigger = TriggerBuilder.Create()
            //                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
            //                        .Build();
            //        await _scheduler.ScheduleJob(detail, trigger);//作业调度建立任务，执行
            //    }
            //}
            #endregion

            return Task.CompletedTask;
        }
    }
}
