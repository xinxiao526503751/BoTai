using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 客户表
    /// </summary>
    [Table(TableName = "base_customer", KeyName = "customer_id", Code = "customer_code", IsIdentity = false)]
    public class base_customer
    {

        /// <summary>
        /// 客户ID
        /// </summary>
        [DefaultValue("")]
        public string customer_id { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        [DefaultValue("")]
        public string customer_code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [DefaultValue("")]
        public string customer_name { get; set; }

        /// <summary>
        /// 客户简称
        /// </summary>
        [DefaultValue("")]
        public string customer_simple_name { get; set; }

        /// <summary>
        /// 客户电话
        /// </summary>
        [DefaultValue("")]
        public string customer_tel { get; set; }

        /// <summary>
        /// 客户传真
        /// </summary>
        [DefaultValue("")]
        public string customer_fax { get; set; }

        /// <summary>
        /// 客户邮编
        /// </summary>
        [DefaultValue("")]
        public string customer_post { get; set; }

        /// <summary>
        /// 客户邮件
        /// </summary>
        [DefaultValue("")]
        public string customer_email { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        [DefaultValue("")]
        public string customer_address { get; set; }

        /// <summary>
        /// 客户网站
        /// </summary>
        [DefaultValue("")]
        public string customer_web { get; set; }

        /// <summary>
        /// 客户银行
        /// </summary>
        [DefaultValue("")]
        public string customer_bank { get; set; }

        /// <summary>
        /// 银行帐号
        /// </summary>
        [DefaultValue("")]
        public string customer_account { get; set; }

        /// <summary>
        /// 客户备注
        /// </summary>
        public string customer_remark { get; set; }

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
        /// 修改时间
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
