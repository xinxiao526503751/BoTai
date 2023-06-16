using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;

namespace Hhmocon.Mes.Repository
{
    public static class QuartzJobService
    {
        public static void AddQuartzJobService(this IServiceCollection services)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IJobFactory,QuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory,StdSchedulerFactory>();
            services.AddSingleton<QuartzMiddleJob>();

            services.AddSingleton<MyJobs>();

            services.AddSingleton(
                new QuartzJobSchedule(typeof(MyJobs),"0 0 12 * * ?")//每天中午12点执行一次 * * * * * *表示时刻执行
                );

            services.AddHostedService<QuartzJobHostService>();
        }
    }
}
