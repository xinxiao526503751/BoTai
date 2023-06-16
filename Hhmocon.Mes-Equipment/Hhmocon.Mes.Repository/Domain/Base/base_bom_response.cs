using hmocon.Mes.Repository.Domain;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    public class base_bom_response : base_bom
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [DefaultValue("")]
        public string material_code { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [DefaultValue("")]
        public string material_name { get; set; }
        /// <summary>
        /// 物料规格
        /// </summary>
        [DefaultValue("")]
        public string material_spec { get; set; }
    }
}
