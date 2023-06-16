/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// 点检项目表
    /// </summary>
    [Table(TableName = "base_examitem", Code = "examitem_code", KeyName = "examitem_id", IsIdentity = false)]
    public class base_examitem
    {
        /// <summary>
        /// 点检ID
        /// </summary>
        [DefaultValue("")]
        public string examitem_id { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        [DefaultValue("")]
        public string examitem_code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        [DefaultValue("")]
        public string examitem_name { get; set; }

        /// <summary>
        /// 点检项目类型ID
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_id { get; set; }

        /// <summary>
        /// 点检项目类型Name
        /// </summary>
        [DefaultValue("")]
        public string examitem_type_name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DefaultValue("")]
        public string description { get; set; }

        /// <summary>
        /// 维保方式类型
        /// "1"点检  "2"保养 "3"维修
        /// </summary>
        [DefaultValue("")]
        public string method_type { get; set; }

        /// <summary>
        /// 维保标准
        /// </summary>
        [DefaultValue("")]
        public string examitem_std { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        [DefaultValue("")]
        public string value_type { get; set; }

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
