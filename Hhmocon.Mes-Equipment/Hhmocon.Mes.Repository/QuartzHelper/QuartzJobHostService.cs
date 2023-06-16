using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository
{
    public class QuartzJobHostService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<QuartzJobSchedule> _jobSchedules;

        public QuartzJobHostService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<QuartzJobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
        }

        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory= _jobFactory;
            foreach (var jobSchedule in _jobSchedules)
            {
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(job,trigger,cancellationToken);
                jobSchedule.JobStatus = JobStatu.Scheduling;
            }

            await Scheduler.Start(cancellationToken);

            foreach (var jobSchedule in _jobSchedules)
            {
                jobSchedule.JobStatus = JobStatu.Running;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
            foreach (var jobSchedule in _jobSchedules)
            {
                jobSchedule.JobStatus=JobStatu.Stopped;
            }
        }

        private static IJobDetail CreateJob(QuartzJobSchedule schedule)
        {
            var jobType=schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(QuartzJobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}
