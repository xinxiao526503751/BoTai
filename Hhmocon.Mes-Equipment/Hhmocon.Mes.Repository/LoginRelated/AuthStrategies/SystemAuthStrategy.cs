using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository.LoginRelated.AuthStrategies
{
    /// <summary>
    /// 领域服务
    /// <para>超级管理员权限</para>
    /// <para>超级管理员使用guid.empty为ID,可以根据需要修改</para>>
    /// </summary>
    public class SystemAuthStrategy : IAuthStrategy, IDependency
    {
        /// <summary>
        /// 用户
        /// </summary>
        protected readonly sys_user _user;

        private readonly PikachuRepository _pikachuRepository;

        public SystemAuthStrategy(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;
            _user = new sys_user
            {
                user_name = "System",
                user_cn_name = "超级管理员",
                user_id = Guid.Empty.ToString()
            };

        }

        /// <summary>
        /// 登录用户
        /// </summary>
        public sys_user User
        {
            get => _user;
            set //禁止外部设置
=> throw new Exception("超级管理员，禁止设置用户");
        }

        public List<MenuTree> Rights
        {
            get
            {
                List<sys_right> sys_Rights = _pikachuRepository.GetAll<sys_right>().OrderBy(a => a.create_time).ToList();
                sys_Rights.RemoveAll(a => a.right_type == "2");
                List<MenuTree> sysRightNodes = _pikachuRepository.ListElementToMenuNode(sys_Rights);
                List<MenuTree> treeModels = _pikachuRepository.ListToMenuTree(sysRightNodes);
                return treeModels;
            }
        }

        public List<sys_role> Roles => _pikachuRepository.GetAll<sys_role>();

        public List<KeyDescription> GetProperties(string moduleCode)
        {
            throw new NotImplementedException();
        }
    }
}
