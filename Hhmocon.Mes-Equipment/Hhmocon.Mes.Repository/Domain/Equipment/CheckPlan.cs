using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 点检计划
    /// </summary>
    public class CheckPlan
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 计划等级
        /// </summary>
        public string PLAN_LEVEL { get; set; }
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
        /// 点检方法
        /// </summary>
        public string CHECK_METHOD { get; set; }
        /// <summary>
        /// 点检频率
        /// </summary>
        public string CHECK_FREQUENCY { get; set; }
        /// <summary>
        /// 是否手动，默认为否
        /// </summary>
        public string IS_MANUAL { get; set; }
        /// <summary>
        /// 是否停机，默认为是
        /// </summary>
        public string IS_STOP { get; set; }
        /// <summary>
        /// 提前期
        /// </summary>
        public int LEAD_TIME { get; set; }
        /// <summary>
        /// 偏置期单位
        /// </summary>
        public string LEAD_TIME_UNIT { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string DES { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string START_TIME { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string END_TIME { get; set; }
        /// <summary>
        /// 软删除标志
        /// </summary>
        public int DELETE_MARK { get; set; }
        /// <summary>
        /// 是否点检
        /// </summary>
        public string IS_CHECK { get; set; }
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
