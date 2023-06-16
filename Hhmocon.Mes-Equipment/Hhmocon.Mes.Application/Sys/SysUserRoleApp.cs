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

using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    public class SysUserRoleApp
    {
        private readonly SysUserRoleRepository _sysUserRoleRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        public SysUserRoleApp(SysUserRoleRepository sysUserRoleRepository, PikachuApp pikachuApp, PikachuRepository pikachuRepository)
        {
            _sysUserRoleRepository = sysUserRoleRepository;
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
        }



        /// <summary>
        /// 根据user_id在关联表中找到user_role
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<sys_role> GetRolesByUserId(string user_id)
        {
            List<sys_user_role> sys_User_Roles = new();
            List<sys_role> sys_Roles = new();
            sys_User_Roles = _pikachuApp.GetAll<sys_user_role>().Where(c => c.user_id == user_id).ToList();
            foreach (sys_user_role temp in sys_User_Roles)
            {
                sys_role sys_Role = _pikachuApp.GetById<sys_role>(temp.role_id);
                sys_Roles.Add(sys_Role);
            }
            return sys_Roles;
        }
    }
}
