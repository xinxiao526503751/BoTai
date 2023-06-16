using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 系统App
    /// </summary>
    public class SysSystemApp
    {
        private readonly ISysSystemRepository _sysSystemRepository;
        private readonly PikachuRepository _pikachuRepository;
        public SysSystemApp(ISysSystemRepository sysSystemRepository, PikachuRepository pikachuRepository)
        {
            _sysSystemRepository = sysSystemRepository;
            _pikachuRepository = pikachuRepository;
        }

        /// <summary>
        /// 新增系统函数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert(sys_system data)
        {
            //查重
            List<sys_system> getByName = _sysSystemRepository.GetByName(data.sys_name);
            if (getByName.Count != 0)
            {
                return false;
            }
            //取ID
            data.sys_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            if (_pikachuRepository.Insert(data))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 遍历所有根节点获取数据
        /// </summary>
        /// <returns></returns>
        public List<sys_system> GetAllNodes()
        {
            //先获取父节点
            List<sys_system> rootNotes = _pikachuRepository.GetAllByParentId<sys_system>(null);
            List<sys_system> allNotes = rootNotes;
            if (rootNotes.Count != 0)
            {
                ///遍历所有根节点
                for (int i = rootNotes.Count - 1; i >= 0; i--)
                {
                    //搜索该根节点的子节点
                    List<sys_system> childrens = GetChildrens(rootNotes[i].sys_id);
                    //加到返回列表
                    allNotes.AddRange(childrens);
                }
            }
            return allNotes;
        }
        /// <summary>
        /// 递归搜索所有子节点
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<sys_system> GetChildrens(string ParentId)
        {
            List<sys_system> childrens = _pikachuRepository.GetAllByParentId<sys_system>(ParentId);
            if (childrens.Count != 0)
            {
                for (int i = childrens.Count - 1; i >= 0; i--)
                {
                    //搜索该节点的所有子节点
                    List<sys_system> childrenOfchildrens = GetChildrens(childrens[i].sys_id);
                    childrens.AddRange(childrenOfchildrens);
                }
            }
            return childrens;
        }

    }
}
