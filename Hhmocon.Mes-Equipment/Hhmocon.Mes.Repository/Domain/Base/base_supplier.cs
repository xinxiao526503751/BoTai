using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 供应商表
    /// </summary>
    [Table(TableName = "base_supplier", Code = "supplier_code", KeyName = "supplier_id", IsIdentity = false)]
    public class base_supplier
    {

        /// <summary>
        /// 供应商ID
        /// </summary>
        [DefaultValue("")]
        public string supplier_id { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        [DefaultValue("")]
        public string supplier_code { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DefaultValue("")]
        public string supplier_name { get; set; }

        /// <summary>
        /// 供应商简称
        /// </summary>
        [DefaultValue("")]
        public string supplier_simple_name { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        [DefaultValue("")]
        public string supplier_tel { get; set; }

        /// <summary>
        /// 供应商传真
        /// </summary>
        [DefaultValue("")]
        public string supplier_fax { get; set; }

        /// <summary>
        /// 供应商邮编
        /// </summary>
        [DefaultValue("")]
        public string supplier_post { get; set; }

        /// <summary>
        /// 供应商邮件
        /// </summary>
        [DefaultValue("")]
        public string supplier_email { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        [DefaultValue("")]
        public string supplier_address { get; set; }

        /// <summary>
        /// 供应商网站
        /// </summary>
        [DefaultValue("")]
        public string supplier_web { get; set; }

        /// <summary>
        /// 供应商银行
        /// </summary>
        [DefaultValue("")]
        public string supplier_bank { get; set; }

        /// <summary>
        /// 银行帐号
        /// </summary>
        [DefaultValue("")]
        public string supplier_account { get; set; }

        /// <summary>
        /// 供应商备注
        /// </summary>
        [DefaultValue("")]
        public string supplier_remark { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DefaultValue("")]
        public string contact_name { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DefaultValue("")]
        public string contact_tel { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        [DefaultValue("")]
        public string contact_mobile { get; set; }

        /// <summary>
        /// 联系人QQ
        /// </summary>
        [DefaultValue("")]
        public string contact_qq { get; set; }

        /// <summary>
        /// 联系人邮件
        /// </summary>
        [DefaultValue("")]
        public string contact_email { get; set; }

        /// <summary>
        /// 联系人微信
        /// </summary>
        [DefaultValue("")]
        public string contact_weixin { get; set; }

        /// <summary>
        /// 联系人生日
        /// </summary>
        [DefaultValue("")]
        public string contact_birthday { get; set; }

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
    }
}
