using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository
{
    public class QuartzJobSchedule
    {
        public QuartzJobSchedule(Type jobType,string cronExpression)
        {
            this.JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression= cronExpression??throw new ArgumentNullException(nameof(cronExpression));
        }

        public Type JobType { get; set; }
        public string CronExpression { get; set; }

        public JobStatu JobStatus { get; set; } = JobStatu.Init;
    }

    public enum JobStatu : byte
    {
        [Description("Initialization")]
        Init = 0,
        [Description("Running")]
        Running = 1,
        [Description("Scheduling")]
        Scheduling = 2,
        [Description("Stopped")]
        Stopped = 3,
    }
}
