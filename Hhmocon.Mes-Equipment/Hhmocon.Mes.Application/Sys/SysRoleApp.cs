using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application
{
    public class SysRoleApp
    {
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public SysRoleApp(ISysRoleRepository sysRoleRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sysRoleRepository = sysRoleRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }
        public bool Insert(sys_role data)
        {
            //查重
            List<sys_role> getByName = _sysRoleRepository.GetByName(data.role_name);
            if (getByName.Count != 0)
            {
                //return false;
                throw new Exception("该角色名称已存在");
            }
            //取ID
            data.role_id = CommonHelper.GetNextGUID();
            data.modified_time = DateTime.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);
            if (_pikachuRepository.Insert(data))
            {
                return true;
            }
            return false;
        }
    }
}
