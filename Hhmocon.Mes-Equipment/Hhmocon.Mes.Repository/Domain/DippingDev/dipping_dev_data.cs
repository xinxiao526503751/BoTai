using Hhmocon.Mes.DataBase;
using System;

namespace hmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 浸胶设备监控数据
    /// </summary>
    [Table(TableName = "dipping_dev_data", KeyName = "dipping_dev_data_id", IsIdentity = false)]
    public class dipping_dev_data
    {

        /////// <summary>
        /////// ID
        /////// </summary>
        //public long dipping_dev_data_id { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string dipping_dev_code { get; set; }

        /// <summary>
        /// 设备运行状态(1: 通讯正常  9：通讯失败)
        /// </summary>
        public int dipping_dev_flag { get; set; }

        /// <summary>
        /// 主速
        /// </summary>
        public int mainspeed { get; set; }

        /// <summary>
        /// 产量
        /// </summary>
        public int output { get; set; }

        /// <summary>
        /// fan1
        /// </summary>
        public int fan1 { get; set; }

        /// <summary>
        /// fan2
        /// </summary>
        public int fan2 { get; set; }

        /// <summary>
        /// fan3
        /// </summary>
        public int fan3 { get; set; }

        /// <summary>
        /// fan4
        /// </summary>
        public int fan4 { get; set; }

        /// <summary>
        /// fan5
        /// </summary>
        public int fan5 { get; set; }

        /// <summary>
        /// fan6
        /// </summary>
        public int fan6 { get; set; }

        /// <summary>
        /// fan7
        /// </summary>
        public int fan7 { get; set; }

        /// <summary>
        /// fan8
        /// </summary>
        public int fan8 { get; set; }

        /// <summary>
        /// fan9
        /// </summary>
        public int fan9 { get; set; }

        /// <summary>
        /// fan10
        /// </summary>
        public int fan10 { get; set; }

        /// <summary>
        /// psfan
        /// </summary>
        public int psfan { get; set; }

        /// <summary>
        /// lqfan1
        /// </summary>
        public int lqfan1 { get; set; }

        /// <summary>
        /// lqfan2
        /// </summary>
        public int lqfan2 { get; set; }

        /// <summary>
        /// fanrun1
        /// </summary>
        public int fanrun1 { get; set; }

        /// <summary>
        /// fanrun2
        /// </summary>
        public int fanrun2 { get; set; }

        /// <summary>
        /// fanrun3
        /// </summary>
        public int fanrun3 { get; set; }

        /// <summary>
        /// fanrun4
        /// </summary>
        public int fanrun4 { get; set; }

        /// <summary>
        /// fanrun5
        /// </summary>
        public int fanrun5 { get; set; }

        /// <summary>
        /// fanrun6
        /// </summary>
        public int fanrun6 { get; set; }

        /// <summary>
        /// fanrun7
        /// </summary>
        public int fanrun7 { get; set; }

        /// <summary>
        /// fanrun8
        /// </summary>
        public int fanrun8 { get; set; }

        /// <summary>
        /// fanrun9
        /// </summary>
        public int fanrun9 { get; set; }

        /// <summary>
        /// fanrun10
        /// </summary>
        public int fanrun10 { get; set; }

        /// <summary>
        /// psfanrun
        /// </summary>
        public int psfanrun { get; set; }

        /// <summary>
        /// lqfanrun1
        /// </summary>
        public int lqfanrun1 { get; set; }

        /// <summary>
        /// lqfanrun2
        /// </summary>
        public int lqfanrun2 { get; set; }

        /// <summary>
        /// tempcontrol1
        /// </summary>
        public int tempcontrol1 { get; set; }

        /// <summary>
        /// tempcontrol2
        /// </summary>
        public int tempcontrol2 { get; set; }

        /// <summary>
        /// tempcontrol3
        /// </summary>
        public int tempcontrol3 { get; set; }

        /// <summary>
        /// tempcontrol4
        /// </summary>
        public int tempcontrol4 { get; set; }

        /// <summary>
        /// tempcontrol5
        /// </summary>
        public int tempcontrol5 { get; set; }

        /// <summary>
        /// tempcontrol6
        /// </summary>
        public int tempcontrol6 { get; set; }

        /// <summary>
        /// tempcontrol7
        /// </summary>
        public int tempcontrol7 { get; set; }

        /// <summary>
        /// tempcontrol8
        /// </summary>
        public int tempcontrol8 { get; set; }

        /// <summary>
        /// tempcontrol9
        /// </summary>
        public int tempcontrol9 { get; set; }

        /// <summary>
        /// tempcontrol10
        /// </summary>
        public int tempcontrol10 { get; set; }

        /// <summary>
        /// datatime
        /// </summary>
        public DateTime datatime { get; set; }
    }

    [Table(TableName = "dipping_dev_data", KeyName = "dipping_dev_data_id", IsIdentity = false)]
    public class dipping_dev_data_pro : dipping_dev_data
    {

        public long dipping_dev_data_id { get; set; }
    }
}
