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
    /// Response类
    /// </summary>
    public class GetUserByDeptIdResponse : sys_user
    {
        /// <summary>
        /// 用户的部门名称
        /// </summary>
        public string dept_name;

        /// <summary>
        /// 用户的角色,有多个，整合成string返回给前端
        /// </summary>
        public List<string> role_names;

        /// <summary>
        /// 整合成string的rolenames
        /// </summary>
        public string role_name_string;
    }
}
