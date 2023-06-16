using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application
{
    public class SysRoleRightApp
    {
        private readonly ISysRoleRightRepository _sysRoleRightRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public SysRoleRightApp(ISysRoleRightRepository sysRoleRightRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sysRoleRightRepository = sysRoleRightRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新建权限角色关联表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertRR(sys_role_right data)
        {
            //取ID
            data.role_right_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
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

        /// <summary>
        /// 判断传入的权限并赋值
        /// </summary>
        /// <param name="treeLevels"></param>
        /// <returns></returns>
        public List<sys_role_right> JudgmentAndEvaluation(string[] rightId, string roleId)
        {
            List<sys_role_right> sys_Role_Rights = new();
            foreach (string temp in rightId)
            {
                sys_role_right sysRoleRight = new();
                sysRoleRight.role_id = roleId;
                sysRoleRight.right_id = temp;//赋系统id
                sys_Role_Rights.Add(sysRoleRight);
            }
            return sys_Role_Rights;
        }

        public List<TreeModel> JudgeChecked(List<sys_role_right> sys_Role_Rights, List<TreeModel> treeModels)
        {
            foreach (sys_role_right sysRoleRightItem in sys_Role_Rights)
            {
                foreach (TreeModel sysRightNodesItem in treeModels)
                {
                    if (sysRightNodesItem.id == sysRoleRightItem.right_id)
                    {
                        sysRightNodesItem.checkstate = 2;
                        break;
                    }

                    //sysRightNodesItem.checkstate = sysRightNodesItem.id == sysRoleRightItem.right_id ? 2 : 0;

                }

            }
            //sys_Role_Rights.Where(a => treeModels.Contains(a));
            return treeModels;

        }


        public List<sys_role_right> GetByRoleId(string roleId)
        {
            List<sys_role_right> data = _sysRoleRightRepository.GetByRoleId(roleId);
            return data;

        }
    }
}
