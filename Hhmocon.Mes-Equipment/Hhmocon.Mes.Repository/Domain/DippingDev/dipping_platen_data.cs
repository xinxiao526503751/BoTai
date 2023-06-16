using Hhmocon.Mes.DataBase;
using System;

namespace hmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 压板设备监控数据
    /// </summary>
    [Table(TableName = "dipping_platen_data", KeyName = "dipping_platen_data_id", IsIdentity = false)]
    public class dipping_platen_data
    {

        ///// <summary>
        ///// ID
        ///// </summary>
        //public long dipping_platen_data_id { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string dipping_platen_code { get; set; }

        /// <summary>
        /// 设备运行状态(1: 通讯正常  9：通讯失败)
        /// </summary>
        public int dipping_platen_flag { get; set; }

        /// <summary>
        /// 成品数
        /// </summary>
        public int output { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int layers { get; set; }

        /// <summary>
        /// 4层平板温度
        /// </summary>
        public double temp4 { get; set; }

        /// <summary>
        /// 10层平板温度
        /// </summary>
        public double temp10 { get; set; }

        /// <summary>
        /// 19层平板温度
        /// </summary>
        public double temp19 { get; set; }

        /// <summary>
        /// 24层平板温度
        /// </summary>
        public double temp24 { get; set; }

        /// <summary>
        /// 压机压力
        /// </summary>
        public double pressure { get; set; }

        /// <summary>
        /// 真空压力
        /// </summary>
        public double vpressure { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public string temprow1 { get; set; }

        /// <summary>
        /// temprow2
        /// </summary>
        public string temprow2 { get; set; }

        /// <summary>
        /// temprow3
        /// </summary>
        public string temprow3 { get; set; }

        /// <summary>
        /// temprow4
        /// </summary>
        public string temprow4 { get; set; }

        /// <summary>
        /// temprow5
        /// </summary>
        public string temprow5 { get; set; }

        /// <summary>
        /// 数据采集时间
        /// </summary>
        public DateTime datetime { get; set; }
    }
}
