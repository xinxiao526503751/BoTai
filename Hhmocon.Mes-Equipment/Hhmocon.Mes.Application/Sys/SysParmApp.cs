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

using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 单位App
    /// </summary>
    public class SysParmApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public SysParmApp(PikachuRepository pikachuRepository, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增单位
        /// </summary>
        /// <returns></returns>
        public bool Insert(sys_parm obj)
        {
            //查重
            List<sys_parm> sys_Parms = _pikachuRepository.GetByOneFeildsSql<sys_parm>("parm_name", obj.parm_name);
            if (sys_Parms.Count != 0)
            {
                throw new Exception($"{obj.parm_name}是已经存在的单位名称");
            }

            //取ID
            obj.parm_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);

            return (_pikachuRepository.Insert(obj));
        }

    }
}
