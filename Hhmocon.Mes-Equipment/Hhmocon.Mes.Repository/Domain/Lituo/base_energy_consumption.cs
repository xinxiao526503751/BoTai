using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 能耗记录表
    /// </summary>
    [Table(TableName = "base_energy_consumption", KeyName = "energy_consumption_id", IsIdentity = false)]
    public class base_energy_consumption
    {

        /// <summary>
        /// 能耗数据ID
        /// </summary>
        public string energy_consumption_id { get; set; }

        /// <summary>
        /// 压机电能耗
        /// </summary>
        public int power_consumption_1 { get; set; }

        /// <summary>
        /// 产线1电能耗
        /// </summary>
        public int power_consumption_2 { get; set; }

        /// <summary>
        /// 产线2电能耗
        /// </summary>
        public int power_consumption_3 { get; set; }

        /// <summary>
        /// 冷却水能耗
        /// </summary>
        public int water_consumption_1 { get; set; }

        /// <summary>
        /// 加热水能耗
        /// </summary>
        public int water_consumptio_2 { get; set; }

        /// <summary>
        /// 天然气能耗
        /// </summary>
        public int natural_gas_energy_consumption { get; set; }

        /// <summary>
        /// 能耗日期
        /// </summary>
        public DateTime consumption_date { get; set; }

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

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modified_by_name { get; set; }

        /// <summary>
        /// 能源类型
        /// </summary>
        public int energy_flag { get; set; }
    }
}
