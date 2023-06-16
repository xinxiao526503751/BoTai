


using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 计划表
    /// </summary>
    [Table(TableName = "plan_process", KeyName = "plan_process_id", Code = "plan_process_code", IsIdentity = false)]
    public class plan_process
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int sort_num { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 计划数
        /// </summary>
        public int plan_num { get; set; }

        /// <summary>
        /// 计划ID
        /// </summary>
        public string plan_process_id { get; set; }

        /// <summary>
        /// 计划编码
        /// </summary>
        public string plan_process_code { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 地点编号
        /// </summary>
        public string location_id { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }
    }

    public class plan_process_rep : plan_process
    {
        /// <summary>
        /// 工序Name
        /// </summary>
        public string process_name { get; set; }
        /// <summary>
        /// 物料NAME
        /// </summary>
        public string material_name { get; set; }
        /// <summary>
        /// 物料code
        /// </summary>
        public string material_code { get; set; }
    }
}
