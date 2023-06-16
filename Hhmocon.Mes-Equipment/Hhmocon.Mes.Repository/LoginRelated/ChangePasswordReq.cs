namespace Hhmocon.Mes.Repository.LoginRelated
{
    public class ChangePasswordReq
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }
    }
}
