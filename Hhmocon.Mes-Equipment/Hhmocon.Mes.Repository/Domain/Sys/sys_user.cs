using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 用户表
    /// </summary>
    [Table(TableName = "sys_user", KeyName = "user_id", IsIdentity = false)]
    public class sys_user
    {

        /// <summary>
        /// 用户id
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// 用户名称(登录时使用)
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string user_passwd { get; set; }

        /// <summary>
        /// 用户中文名称
        /// </summary>
        public string user_cn_name { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string empno { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string dept_id { get; set; }

        /// <summary>
        /// 岗位编号
        /// </summary>
        public string position_id { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime last_login_time { get; set; }

        /// <summary>
        /// 用户描述
        /// </summary>
        public string user_desc { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public int is_lock { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string user_tel { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string user_mobile { get; set; }

        /// <summary>
        /// 用户QQ
        /// </summary>
        public string user_qq { get; set; }

        /// <summary>
        /// 用户Webchat
        /// </summary>
        public string user_webchat { get; set; }

        /// <summary>
        /// 用户邮件
        /// </summary>
        public string user_email { get; set; }

        /// <summary>
        /// 用户类型(0普通用户,1操作工)
        /// </summary>
        public int user_type { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string user_sex { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string user_original { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string user_address { get; set; }

        /// <summary>
        /// 文化程度
        /// </summary>
        public string user_edu { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public string user_intime { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }



        /// <summary>
        /// 用户报工ID
        /// </summary>
        public string user_report_process { get; set; }
    }
}
