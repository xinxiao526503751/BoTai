using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository.LoginRelated.AuthStrategies
{
    /// <summary>
    /// 普通用户授权策略
    /// </summary>
    public class NormalAuthStrateg : IAuthStrategy, IDependency
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        protected sys_user _user;

        private readonly PikachuRepository _pikachuRepository;

        public NormalAuthStrateg(PikachuRepository pikachuRepository)
        {
            _pikachuRepository = pikachuRepository;

        }

        /// <summary>
        /// 获取权限菜单
        /// </summary>
        public List<MenuTree> Rights
        {
            get
            {
                List<sys_user_role> sys_User_Roles = _pikachuRepository.GetAll<sys_user_role>().Where(a => a.user_id == _user.user_id).ToList();
                IEnumerable<string> roleId = from n in sys_User_Roles select n.role_id;
                List<sys_role_right> sys_role_right = _pikachuRepository.GetAll<sys_role_right>().Where(a => roleId.Contains(a.role_id)
                ).ToList();
                IEnumerable<string> rightId = from n in sys_role_right select n.right_id;

                List<sys_right> sys_Rights = _pikachuRepository.GetAllByIds<sys_right>(rightId.ToArray()).OrderBy(a => a.create_time).ToList();
                sys_Rights.RemoveAll(a => a.right_type == "2");
                List<MenuTree> sysRightNodes = _pikachuRepository.ListElementToMenuNode(sys_Rights);
                List<MenuTree> treeModels = _pikachuRepository.ListToMenuTree(sysRightNodes);
                return treeModels;
            }
        }


        public List<sys_role> Roles
        {
            get
            {
                List<sys_user_role> sys_User_Roles = _pikachuRepository.GetAll<sys_user_role>().Where(a => a.user_id == _user.user_id).ToList();
                IEnumerable<string> roleId = from n in sys_User_Roles select n.role_id;

                return _pikachuRepository.GetAllByIds<sys_role>(roleId.ToArray());
            }
        }//之后要删除相同角色

        public sys_user User
        {
            //get { return _user; }
            get => _pikachuRepository.GetAll<sys_user>().Where(a => a.user_name == _user.user_name).FirstOrDefault();
            //get { return _pikachuRepository.GetAll<sys_user>().FirstOrDefault(); }
            set => _user = value;
        }

        public List<KeyDescription> GetProperties(string moduleCode)
        {
            throw new NotImplementedException();
        }
    }
}
