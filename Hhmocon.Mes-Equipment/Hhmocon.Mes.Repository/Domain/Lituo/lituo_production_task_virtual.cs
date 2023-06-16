using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 工序产量虚拟数据表
    /// </summary>
    [Table(TableName="lituo_production_task_virtual",KeyName= "process_data_id", IsIdentity = false)]
    public class lituo_production_task_virtual
    {

        /// <summary>
        /// 
        /// </summary>
        public DateTime datetime { get; set; }

        /// <summary>
        /// 工序数据ID
        /// </summary>
        public string process_data_id { get; set; }

        /// <summary>
        /// 压板
        /// </summary>
        public int platening_process { get; set; }

        /// <summary>
        /// 打包
        /// </summary>
        public int packaging_process { get; set; }

        /// <summary>
        /// 雕刻
        /// </summary>
        public int engraving_process { get; set; }

        /// <summary>
        /// 下料
        /// </summary>
        public int unloading_process { get; set; }

        /// <summary>
        /// 贴边
        /// </summary>
        public int welting_process { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_by_name { get; set; }
    }
}
