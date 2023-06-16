using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 权限事务
    /// </summary>
    public class SysRightApp
    {
        private readonly ISysRightRepository _sysRightRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public SysRightApp(ISysRightRepository sysRightRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _sysRightRepository = sysRightRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public bool Insert(sys_right data)
        {
            //查重
            List<sys_right> getByName = _sysRightRepository.GetByName(data.right_name);

            List<sys_right> broData = _pikachuRepository.GetAllByParentId<sys_right>(data.parent_right_id);//不允许系统下有子系统还添加权限
            if (getByName.Count != 0)
            {
                //return false;
                throw new Exception("该权限已存在");
            }
            if (broData.Where(a => a.right_type != data.right_type).ToList().Count != 0)
            {
                throw new Exception("该权限存在下级权限，不允许添加同级");
            }
            //取ID
            data.right_id = CommonHelper.GetNextGUID();
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


        public bool DeleteMask<sys_right>(string[] ids)
        {
            foreach (string id in ids)
            {
                List<sys_right> sys_Rights = _pikachuRepository.GetAllByParentId<sys_right>(id);
                if (sys_Rights.Count != 0)
                {
                    return false;
                }
            }
            return _pikachuRepository.Delete_Mask<sys_right>(ids);
        }
        /// <summary>
        /// 根据系统级id获取权限级数据
        /// </summary>
        /// <param name="SysId"></param>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<sys_right> GetRightBySysId(string SysId, PageReq req, ref long icount)
        {

            List<sys_right> data = _sysRightRepository.GetByParentRightId(SysId);
            List<sys_right> rightData = data.Where(a => a.right_type == "1").ToList();
            icount = rightData.Count;
            //分页
            if (req != null)
            {
                string strKey = req.key;
                int iPage = req.page;
                int iRows = req.rows;
                string strSort = req.sort;
                string strOrder = req.order;
                string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
                string ordStr = string.Empty;
                rightData = rightData.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            return rightData;
        }

        /// <summary>
        /// 根据权限级id获取按钮级数据
        /// </summary>
        /// <param name="RightId"></param>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<sys_right> GetButtonByRightId(string RightId, PageReq req, ref long icount)
        {

            List<sys_right> data = _sysRightRepository.GetByParentRightId(RightId);
            List<sys_right> rightData = data.Where(a => a.right_type == "2").ToList();
            icount = rightData.Count;
            //分页
            if (req != null)
            {
                string strKey = req.key;
                int iPage = req.page;
                int iRows = req.rows;
                string strSort = req.sort;
                string strOrder = req.order;
                string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
                string ordStr = string.Empty;
                rightData = rightData.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            return rightData;
        }

        /// <summary>
        /// 遍历所有根节点获取数据
        /// </summary>
        /// <returns></returns>
        public List<sys_right> GetSysNodes()
        {
            //先获取父节点
            List<sys_right> rootNotes = _pikachuRepository.GetAllByParentId<sys_right>(null);
            List<sys_right> allNotes = rootNotes;
            if (rootNotes.Count != 0)
            {
                ///遍历所有根节点
                for (int i = rootNotes.Count - 1; i >= 0; i--)
                {
                    //搜索该根节点的子节点
                    List<sys_right> childrens = GetSysChildrens(rootNotes[i].right_id);
                    //加到返回列表
                    allNotes.AddRange(childrens);
                }
            }
            return allNotes;
        }
        /// <summary>
        /// 递归搜索所有系统子节点
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<sys_right> GetSysChildrens(string ParentId)
        {
            List<sys_right> childrens = _pikachuRepository.GetAllByParentId<sys_right>(ParentId)
                .Where(a => a.right_type == "0").ToList(); //筛选系统数据
            if (childrens.Count != 0)
            {
                for (int i = childrens.Count - 1; i >= 0; i--)
                {
                    //搜索该节点的所有子节点
                    List<sys_right> childrenOfchildrens = GetSysChildrens(childrens[i].right_id);
                    childrens.AddRange(childrenOfchildrens);
                }

            }
            return childrens;
        }
    }
}

