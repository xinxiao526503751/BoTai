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

using Hhmocon.Mes.Repository.Repository.Sys.SysUserDept;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 用户部门App
    /// </summary>
    public class SysUserDeptApp
    {
        private readonly ISysUserDeptRepository _sysUserDeptRepository;
        public SysUserDeptApp(ISysUserDeptRepository sysUserDeptRepository)
        {
            _sysUserDeptRepository = sysUserDeptRepository;
        }


    }
}
