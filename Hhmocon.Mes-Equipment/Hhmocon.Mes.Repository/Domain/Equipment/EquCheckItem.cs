using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 设备点检项
    /// </summary>
    public class EquCheckItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string PLAN_ID { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EQU_ID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EQU_NAME { get; set; }
        /// <summary>
        /// 点检项目编号
        /// </summary>
        public string CHECK_ITEM_ID { get; set; }
        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string CHECK_ITEM_NAME { get; set; }
        /// <summary>
        /// 点检人员
        /// </summary>
        public string CHECK_PERSON { get; set; }
        /// <summary>
        /// 点检结果
        /// </summary>
        public string CHECK_RESULT { get; set; }
        /// <summary>
        /// 软删除标志
        /// </summary>
        public int  DELETE_MARK { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATED_TIME { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CREATED_BY { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UPDATE_TIME { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string? UPDATE_BY { get; set; }
    }
}
