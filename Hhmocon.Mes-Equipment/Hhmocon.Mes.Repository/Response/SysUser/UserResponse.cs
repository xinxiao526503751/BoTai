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

using Hhmocon.Mes.Repository.Domain;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Response
{
    /// <summary>
    /// 用户Response类型
    /// </summary>
    public class UserResponse : sys_user
    {
        /// <summary>
        /// 部门名
        /// </summary>
        public string dept_name;

        /// <summary>
        /// 角色名
        /// </summary>
        public List<string> role_name;

        /// <summary>
        /// 角色id
        /// </summary>
        public List<string> role_id;
    }
}
