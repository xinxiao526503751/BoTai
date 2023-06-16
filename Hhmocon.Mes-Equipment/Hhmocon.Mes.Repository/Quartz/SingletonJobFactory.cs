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

using Quartz;
using Quartz.Spi;
using System;

namespace Hhmocon.Mes.Util.Quartz
{
    /// <summary>
    /// 一个自定义的作业工厂。用来从DI容器中获取job实例
    /// </summary>
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 无论请求哪个作业，始终返回QuartzJobRunner任务实例，利用QuartzJobRunner这个任务来同一处理所有其他任务的生命周期
        /// </summary>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            //return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            //return _serviceProvider.GetRequiredService<QuartzJobRunner>();
            IJobDetail jobDetail = bundle.JobDetail;
            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }

        /// <summary>
        /// 调度程序尝试销毁工厂创建的作业的地方
        /// 由于我们使用根容器IServiceProvider来管理IJob对象,根容器的生命周期是全局的，我们无法创建对作业处理所需的具有局部范围的容器
        /// 
        /// 这是很重要的信息，此类实现方法进队单例或瞬态IJob对象是安全的。对若想回收局部实例，必须在局部容器中进行工作，结束时对局部容器进行回收。
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            // var disposable = job as IDisposable;
            // disposable?.Dispose();
        }
    }
}
