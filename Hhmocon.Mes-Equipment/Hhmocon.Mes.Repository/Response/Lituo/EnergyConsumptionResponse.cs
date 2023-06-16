using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Response
{
    public class EnergyConsumptionResponse
    {
        public List<string> dateString;

        public List<string> ConsumptionOrNum_1;

        public List<string> ConsumptionOrNum_2;

        public List<string> ConsumptionOrNum_3;

        public List<string> total_day;
    }
    public class EnergyConsumptionOutputDate
    {

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
    }
}
