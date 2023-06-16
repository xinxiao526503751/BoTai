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
using Hhmocon.Mes.Repository;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Quartz
{
    /// <summary>
    /// 后台任务实现类，继承IHsotedService后台任务接口
    /// 对ISchedulerFactory，IJobFactory，JobSchedule进行组合
    /// </summary>
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;//调度工厂,用来创建调度器Scheduler
        private readonly IJobFactory _jobFactory;            //自定义的作业工厂，用来从根容器中获取作业。这里获取的是QuartzJobRunner
        private readonly IEnumerable<JobSchedule> _jobSchedule;//创建实例时可以向容器注入多个JobSchedule类型，会自动创建IEnumerable<T>类型

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<JobSchedule> jobSchedule)//IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedule = jobSchedule;
            _jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }//IScheduler实例,作业调度池

        /// <summary>
        /// StartAsync在应用程序启动时被调用，是配置Quartz的地方。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //【通过调度工厂ISchedulerFactory创建调度实例IScheduler】
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);//**调度工厂ISchedulerFactory用来为IScheduler提供句柄，也就是IScheduler实例

            //【将自定义的作业工厂JobFactory提供给IScheduler实例，作业工厂负责从容器中获取IJob服务】
            Scheduler.JobFactory = _jobFactory;//**自定义的实现的作业工厂IJobFactory用来为IScheduler从根容器获取实例

            foreach (JobSchedule jobSchedule in _jobSchedule)//遍历容器创建的调度DTO
            {
                IJobDetail job = CreateJob(jobSchedule);//根据DTO的句柄创建IJobDetail实例
                ITrigger trigger = CreateTrigger(jobSchedule);//根据DTO的句柄创建ITrigger实例

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);//根据IJobDetail和ITrigger，并提供[取消令牌]，正式执行调度任务【注意和JobRunner中的JobDetail.Execute区别】  调度任务是有时间和触发设备的，任务是直接跑线程走代码
            }

            await Scheduler.Start(cancellationToken);//通过IScherduler实例启动线程，调度任务被正式启动
        }

        /// <summary>
        /// 当应用程序关闭时，框架将调用StopAsync()，此时能够调用Scheduler.Shutdown()来安全地关闭调度程序进程。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);//通过Scheduler实例关闭线程
        }

        /// <summary>
        /// 根据CronExpresion创建触发条件
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
            .Create()
            .WithIdentity($"{schedule.JobType.FullName}.trigger")
            .WithCronSchedule(schedule.CronExpression)
            .WithDescription(schedule.CronExpression)
            .Build();
        }

        /// <summary>
        /// 根据schedule的句柄创建job
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private IJobDetail CreateJob(JobSchedule schedule)
        {

            Type jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }
    }
}
