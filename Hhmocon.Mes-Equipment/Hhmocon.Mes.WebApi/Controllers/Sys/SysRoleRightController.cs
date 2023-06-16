using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 角色权限控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysRoleRightController : ControllerBase
    {

        private readonly PikachuApp _pikachuApp;
        private readonly SysRoleRightApp _sysRoleRightApp;
        public SysRoleRightController(SysRoleRightApp sysRoleRightApp, PikachuApp pikachuApp)
        {
            _pikachuApp = pikachuApp;
            _sysRoleRightApp = sysRoleRightApp;
        }
        /// <summary>
        /// 给角色赋权限
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]

        public Response<string> GiveRolePermissions(string roleId, string[] rightId)
        {
            List<sys_role_right> obj = new();
            Response<string> result = new Response<string>();
            List<string> rightIdCopy = rightId.ToList();
            //string[] rightIdDele = null;
            try
            {
                List<sys_role_right> oldSysRoleRights = _sysRoleRightApp.GetByRoleId(roleId);
                IEnumerable<string> oldRightId = from n in oldSysRoleRights select n.right_id;
                foreach (string a in rightId)
                {
                    if (oldRightId.Contains(a))//原来存在的权限
                    {
                        rightIdCopy.Remove(a);
                    }
                }
                foreach (string b in oldRightId)
                {

                    if (!rightId.Contains(b))//要删除的权限
                    {
                        //rightIdDele.ToList().Add(b);
                        IEnumerable<string> sysRoleRightsDel = from n in oldSysRoleRights
                                                               where n.right_id == b
                                                               select n.role_right_id;

                        _pikachuApp.DeleteMask<sys_role_right>(sysRoleRightsDel.ToArray());//删除废弃的权限
                    }
                }
                obj = _sysRoleRightApp.JudgmentAndEvaluation(rightIdCopy.ToArray(), roleId);//赋值

                foreach (sys_role_right item in obj)
                {
                    if (_sysRoleRightApp.InsertRR(item))
                    {
                        result.Result = item.role_right_id;
                    }
                    else
                    {
                        //更新失败
                        result.Code = 100;
                        result.Message = "数据写入失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据角色返回树结构用于给角色权限页面菜单栏
        /// 默认不选定任何按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<TreeLevel>> ObtainNodesByRole(string roleId)
        {
            Response<List<TreeLevel>> result = new Response<List<TreeLevel>>();
            try
            {
                List<sys_role_right> sysRoleRight = _sysRoleRightApp.GetByRoleId(roleId);

                List<sys_right> sysRights = _pikachuApp.GetAll<sys_right>();

                List<TreeModel> sysRightNodes = _pikachuApp.ListElementToNode(sysRights, false);

                List<TreeModel> models = _sysRoleRightApp.JudgeChecked(sysRoleRight, sysRightNodes);
                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(models);
                List<TreeLevel> easies = _pikachuApp.TreeModelToTreeLevel(treeModels);

                result.Result = easies;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 通过RoleId返回rightId数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> ObtainSysRoleByRoleId(string roleId)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                List<sys_role_right> sysRoleRight = _sysRoleRightApp.GetByRoleId(roleId);
                IEnumerable<string> sysRightsId = from n in sysRoleRight select n.right_id;
                result.Result = sysRightsId.ToArray();

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;

        }
        /// <summary>
        /// 根据角色ID获取左边菜单栏
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<TreeLevel>> ObtainSysRoleNodesByRoleId(string roleId)
        {
            Response<List<TreeLevel>> result = new Response<List<TreeLevel>>();
            try
            {
                List<sys_role_right> sysRoleRight = _sysRoleRightApp.GetByRoleId(roleId);
                IEnumerable<string> sysRightsId = from n in sysRoleRight select n.right_id;
                List<sys_right> sysRights = _pikachuApp.GetAllByIds<sys_right>(sysRightsId.ToArray());
                int removeNumber = sysRights.RemoveAll(a => a.right_type == "2");


                List<TreeModel> sysRightNodes = _pikachuApp.ListElementToNode(sysRights, false);

                List<TreeModel> models = _sysRoleRightApp.JudgeChecked(sysRoleRight, sysRightNodes);
                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(models);
                List<TreeLevel> easies = _pikachuApp.TreeModelToTreeLevel(treeModels);

                result.Result = easies;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;

        }
    }
}
