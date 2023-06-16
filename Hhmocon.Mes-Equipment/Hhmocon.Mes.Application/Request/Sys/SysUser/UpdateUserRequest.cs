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

using System.Collections.Generic;

namespace Hhmocon.Mes.Application.Request
{
    /// <summary>
    /// 修改用户请求
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// 用户角色Id
        /// </summary>
        public List<string> roles;

        /// <summary>
        /// 用户id
        /// </summary>
        public string user_id;

        /// <summary>
        /// 用户密码
        /// </summary>
        public string user_passwd { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string user_name { get; set; }

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
        /// 用户描述
        /// </summary>
        public string user_desc { get; set; }

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
        /// 停用和启用状态
        /// </summary>
        public string is_lock { get; set; }
    }
}
