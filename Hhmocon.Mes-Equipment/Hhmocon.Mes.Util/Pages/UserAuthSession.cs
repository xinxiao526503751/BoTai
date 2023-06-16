using System;

namespace Hhmocon.Mes.Util
{
    [Serializable]
    public class UserAuthSession
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 系统的Key 用户标识是什么系统留待以后扩展用。
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 所属的院区ID
        /// </summary>
        public long OrgID { get; set; }

        /// <summary>
        /// 所属的院区Name
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 当前工作的子部门
        /// </summary>
        public long SubOrgID { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  登录的IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Session的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}