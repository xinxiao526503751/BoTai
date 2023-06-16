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

using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// Job调度中间对象。一个简单的DTO，包含配置作业所需的各类参数，用来更方便的配置作业
    /// </summary>
    public class JobSchedule
    {

        //调度表有两个对象(IJOB,Cropression)
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression = cronExpression ?? throw new ArgumentNullException(nameof(cronExpression));
        }
        /// <summary>
        /// Job类型
        /// </summary>
        public Type JobType { get; private set; }
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; private set; }
        /// <summary>
        /// Job状态
        /// </summary>
        public JobStatus JobStatu { get; set; } = JobStatus.Init;


    }

    /// <summary>
    /// Job运行状态
    /// </summary>
    public enum JobStatus : byte
    {
        [Description("初始化")]
        Init = 0,
        [Description("运行中")]
        Running = 1,
        [Description("调度中")]
        Scheduling = 2,
        [Description("已停止")]
        Stopped = 3
    }
}
