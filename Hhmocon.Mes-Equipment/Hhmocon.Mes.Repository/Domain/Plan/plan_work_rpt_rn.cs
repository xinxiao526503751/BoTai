using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    public class plan_work_rpt_rn : plan_work_rpt
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        [DefaultValue("")]
        public string material_name { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [DefaultValue("")]
        public string material_code { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        [DefaultValue("")]
        public string material_spec { get; set; }

        /// <summary>
        /// 已完成数
        /// </summary>
        [DefaultValue("")]
        public int completed_num { get; set; }
    }
}
