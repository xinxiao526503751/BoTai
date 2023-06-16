using System;
using System.ComponentModel.DataAnnotations;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// 登录请求类
    /// </summary>
    public class PassportLoginRequest
    {
        /// <summary>
        /// 账户名
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        [Required]
        public string AppKey { get; set; }

        /// <summary>
        /// 去空操作
        /// </summary>
        public void Trim()
        {
            if (string.IsNullOrEmpty(Account))
            {
                throw new Exception("用户名不能为空");
            }
            if (string.IsNullOrEmpty(Password))
            {
                throw new Exception("密码不能为空");
            }
            Account = Account.Trim();
            Password = Password.Trim();

            //如果appkey非空  就去掉两端的空格
            if (!string.IsNullOrEmpty(AppKey))
            {
                AppKey = AppKey.Trim();//.Trim()方法去掉字符串两端的空格
            }
        }
    }
}
