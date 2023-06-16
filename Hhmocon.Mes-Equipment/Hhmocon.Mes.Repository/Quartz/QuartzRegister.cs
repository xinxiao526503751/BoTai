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

using Hhmocon.Mes.Application.Quartz;
using Hhmocon.Mes.Repository;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Hhmocon.Mes.Util.Quartz
{
    /// <summary>
    /// Quartz的服务注入
    /// </summary>
    public static class QuartzRegister
    {
        public static void AddJob(this IServiceCollection services)
        {

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();//启用后台任务, QuartzHostedService将IJobFactory、ISchedulerFactory、JobSchedule组合


            //添加job=
            services.AddSingleton<DemoJob>();//在JobRunner统一管理、并与局部容器一起回收
            services.AddSingleton<QuartzJobRunner>();//注册全局任务，对其他任务进行管理

        }
    }
}
