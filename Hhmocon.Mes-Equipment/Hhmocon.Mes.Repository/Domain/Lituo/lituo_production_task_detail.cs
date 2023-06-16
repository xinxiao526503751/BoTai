using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 生产任务子表
    /// </summary>
    [Table(TableName = "lituo_production_task_detail", KeyName = "ID", IsIdentity = false)]
    public class lituo_production_task_detail
    {

        /// <summary>
        /// id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// [订货单号]
        /// </summary>
        public string DHDH { get; set; }

        /// <summary>
        /// [产品备注]
        /// </summary>
        public string CPBZ { get; set; }

        /// <summary>
        /// [材质]
        /// </summary>
        public string CZ { get; set; }

        /// <summary>
        /// [产品编号]
        /// </summary>
        public string CPBH { get; set; }

        /// <summary>
        /// [产品长度]
        /// </summary>
        public int CPCD { get; set; }

        /// <summary>
        /// [产品单价]
        /// </summary>
        public int CPDJ { get; set; }

        /// <summary>
        /// [产品高度]
        /// </summary>
        public int CPGD { get; set; }

        /// <summary>
        /// [产品规格]
        /// </summary>
        public string CPGG { get; set; }

        /// <summary>
        /// [产品金额]
        /// </summary>
        public int CPJE { get; set; }

        /// <summary>
        /// [产品宽度]
        /// </summary>
        public int CPKD { get; set; }

        /// <summary>
        /// [产品类别]
        /// </summary>
        public string CPLB { get; set; }

        /// <summary>
        /// [产品名称]
        /// </summary>
        public string CPMC { get; set; }

        /// <summary>
        /// [产品属性]
        /// </summary>
        public string CPSX { get; set; }

        /// <summary>
        /// [单位]
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// [调整单价]
        /// </summary>
        public int TZDJ { get; set; }

        /// <summary>
        /// [订货数量]
        /// </summary>
        public int DHSL { get; set; }

        /// <summary>
        /// [订货序号]
        /// </summary>
        public string DHXH { get; set; }

        /// <summary>
        /// [房间标注]
        /// </summary>
        public string FJBZ { get; set; }

        /// <summary>
        /// [基础价格]
        /// </summary>
        public int JCJG { get; set; }

        /// <summary>
        /// [款式型号]
        /// </summary>
        public string KSXH { get; set; }

        /// <summary>
        /// [配项价格]
        /// </summary>
        public int PXJG { get; set; }

        /// <summary>
        /// [品牌]
        /// </summary>
        public string PP { get; set; }

        /// <summary>
        /// [颜色]
        /// </summary>
        public string YS { get; set; }

        /// <summary>
        /// [派工日期]
        /// </summary>
        public DateTime PGRQ { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }
    }
}
