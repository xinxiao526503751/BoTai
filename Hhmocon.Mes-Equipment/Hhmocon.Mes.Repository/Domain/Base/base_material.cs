using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    [Table(TableName = "base_material", Code = "material_code", KeyName = "material_id", IsIdentity = false)]
    public class base_material
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        [DefaultValue("")]
        public string material_id { get; set; }

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
        /// 物料全称
        /// </summary>
        [DefaultValue("")]
        public string material_fullname { get; set; }

        /// <summary>
        /// 物料英文名称
        /// </summary>
        [DefaultValue("")]
        public string material_en_name { get; set; }

        /// <summary>
        /// 物料简称
        /// </summary>
        [DefaultValue("")]
        public string material_short_name { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        [DefaultValue("")]
        public string material_spec { get; set; }

        /// <summary>
        /// 物料型号
        /// </summary>
        [DefaultValue("")]
        public string material_model { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        [DefaultValue("")]
        public string material_unit { get; set; }

        /// <summary>
        /// 制造方式
        /// 1、自制件 2、采购件 3、外协件 9、虚拟件
        /// </summary>
        public int production_mode { get; set; }

        /// <summary>
        /// 物料颜色
        /// </summary>
        [DefaultValue("")]
        public string material_color { get; set; }

        /// <summary>
        /// 物料类型id
        /// </summary>
        [DefaultValue("")]
        public string material_type_id { get; set; }

        /// <summary>
        /// 物料类型code
        /// </summary>
        [DefaultValue("自动根据物料类型id生成")]
        public string material_type_code { get; set; }

        /// <summary>
        /// 物料类型name
        /// </summary>
        [DefaultValue("")]
        public string material_type_name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DefaultValue("")]
        public string description { get; set; }

        /// <summary>
        /// 物料重量
        /// </summary>
        [DefaultValue("")]
        public string material_weight { get; set; }


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
        [DefaultValue("")]
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DefaultValue("")]
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [DefaultValue("")]
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [DefaultValue("")]
        public string modified_by_name { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [DefaultValue("")]
        public string brand { get; set; }

        /// <summary>
        /// 款式型号
        /// </summary>
        [DefaultValue("")]
        public string style_model { get; set; }
    }
}
