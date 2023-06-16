using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 工艺路线规则表
    /// </summary>
    [Table(TableName = "base_process_route", KeyName = "process_route_id", Code = "process_route_code", IsIdentity = false)]
    public class base_process_route
    {

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string process_route_id { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string process_route_code { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string process_route_name { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// 是否主工艺路线
        /// </summary>
        public int is_master { get; set; }

        /// <summary>
        /// 状态（new:新建；audit:审核；stop:停用）
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string check_by { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime check_time { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        public string check_comment { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
