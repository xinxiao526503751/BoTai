using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 生产任务主表
    /// </summary>
    [Table(TableName = "lituo_production_task_main", KeyName = "DHDH", IsIdentity = false)]
    public class lituo_production_task_main
    {
        /// <summary>
        /// [订货单号]
        /// </summary>
        public string DHDH { get; set; }

        /// <summary>
        /// [订货日期]
        /// </summary>
        public DateTime DHRQ { get; set; }

        /// <summary>
        /// [订货备注]
        /// </summary>
        public string DHBZ { get; set; }

        /// <summary>
        /// [订单颜色备注]
        /// </summary>
        public string DDYSBZ { get; set; }

        /// <summary>
        /// [撤改单反馈]
        /// </summary>
        public string CGDFK { get; set; }

        /// <summary>
        /// [拆单人员]
        /// </summary>
        public string CDRY { get; set; }

        /// <summary>
        /// [拆单附件]
        /// </summary>
        public string CDFJ { get; set; }

        /// <summary>
        /// [部门改单标识码]
        /// </summary>
        public string BMGDBSM { get; set; }

        /// <summary>
        /// [补单原单单号]
        /// </summary>
        public string BDYDDH { get; set; }

        /// <summary>
        /// [安装图附件]
        /// </summary>
        public string AZTFJ { get; set; }

        /// <summary>
        /// [改单描述]
        /// </summary>
        public string GDMS { get; set; }

        /// <summary>
        /// [改单时间]
        /// </summary>
        public DateTime GDSJ { get; set; }

        /// <summary>
        /// [交货工期]
        /// </summary>
        public DateTime JHGQ { get; set; }

        /// <summary>
        /// [派工单号]
        /// </summary>
        public string PGDH { get; set; }

        /// <summary>
        /// [派工日期]
        /// </summary>
        public DateTime PGRQ { get; set; }

        /// <summary>
        /// [派工时间]
        /// </summary>
        public DateTime PGSJ { get; set; }

        /// <summary>
        /// [生产包数]
        /// </summary>
        public int SCBS { get; set; }

        /// <summary>
        /// [生产备注]
        /// </summary>
        public string SCBZ { get; set; }

        /// <summary>
        /// [生产部号]
        /// </summary>
        public string SCBH { get; set; }

        /// <summary>
        /// [生产部门]
        /// </summary>
        public string SCBM { get; set; }

        /// <summary>
        /// [生产金额]
        /// </summary>
        public int SCJE { get; set; }

        /// <summary>
        /// [生产区域]
        /// </summary>
        public string SCQY { get; set; }

        /// <summary>
        /// [生产状态]
        /// </summary>
        public string SCZT { get; set; }

        /// <summary>
        /// [是否补单]
        /// </summary>
        public string SFBD { get; set; }

        /// <summary>
        /// [是否改单]
        /// </summary>
        public string SFGD { get; set; }

        /// <summary>
        /// [是否开票]
        /// </summary>
        public string SFKP { get; set; }

        /// <summary>
        /// [完工日期]
        /// </summary>
        public DateTime WGRQ { get; set; }

        /// <summary>
        /// [销售部号]
        /// </summary>
        public string XSBH { get; set; }

        /// <summary>
        /// [销售部门]
        /// </summary>
        public string XSBM { get; set; }

        /// <summary>
        /// [销售工号]
        /// </summary>
        public string XSGH { get; set; }

        /// <summary>
        /// [销售人员]
        /// </summary>
        public string XSRY { get; set; }

        /// <summary>
        /// [销售手机]
        /// </summary>
        public string XSSJ { get; set; }

        /// <summary>
        /// [运输方式]
        /// </summary>
        public string YSFS { get; set; }

        /// <summary>
        /// [制图附件]
        /// </summary>
        public string ZTFJ { get; set; }

        /// <summary>
        /// 工序1完工标志
        /// </summary>
        public int process1_finish_flag { get; set; }

        /// <summary>
        /// 工序2完工标志
        /// </summary>
        public int process2_finish_flag { get; set; }

        /// <summary>
        /// 工序3完工标志
        /// </summary>
        public int process3_finish_flag { get; set; }

        /// <summary>
        /// 工序4完工标志
        /// </summary>
        public int process4_finish_flag { get; set; }

        /// <summary>
        /// 工序5完工标志
        /// </summary>
        public int process5_finish_flag { get; set; }

        /// <summary>
        /// 订单完工标志
        /// </summary>
        public int order_finish_flag { get; set; }

        /// <summary>
        /// 工序1完工时间
        /// </summary>
        public DateTime process1_finish_time { get; set; }

        /// <summary>
        /// 工序2完工时间
        /// </summary>
        public DateTime process2_finish_time { get; set; }

        /// <summary>
        /// 工序3完工时间
        /// </summary>
        public DateTime process3_finish_time { get; set; }

        /// <summary>
        /// 工序4完工时间
        /// </summary>
        public DateTime process4_finish_time { get; set; }

        /// <summary>
        /// 工序5完工时间
        /// </summary>
        public DateTime process5_finish_time { get; set; }

        /// <summary>
        /// 订单完工时间
        /// </summary>
        public DateTime order_finish_time { get; set; }


        /// <summary>
        /// 工序1报工人ID
        /// </summary>
        public string process1_finish_user { get; set; }

        /// <summary>
        /// 工序2报工人ID
        /// </summary>
        public string process2_finish_user { get; set; }

        /// <summary>
        /// 工序3报工人ID
        /// </summary>
        public string process3_finish_user { get; set; }

        /// <summary>
        /// 工序4报工人ID
        /// </summary>
        public string process4_finish_user { get; set; }

        /// <summary>
        /// 工序5 报工人ID
        /// </summary>
        public string process5_finish_user { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 发货标志
        /// </summary>
        public int send_flag { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public string send_user { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime send_time { get; set; }

        //订单所在库位
        public string kwcode { get; set; }

        public string kw_enter_user { get; set; }

        public DateTime kw_enter_time { get; set; }
        /// <summary>
        /// 订单所在货架
        /// </summary>
        public string hjcode { get; set; }

        public string hj_enter_user { get; set; }

        public DateTime hj_enter_time { get; set; }
    }
    public class lituo_production_task_main_rep:lituo_production_task_main
    {

        /// <summary>
        /// 子订单加工数量统计
        /// </summary>
        public int SL { get; set; }
      

    }
  
}
