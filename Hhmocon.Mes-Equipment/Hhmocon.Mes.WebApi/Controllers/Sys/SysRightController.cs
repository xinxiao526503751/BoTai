
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
    /// 权限接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysRightController : ControllerBase
    {
        private readonly SysRightApp _sysRightApp;
        private readonly SysSystemApp _sysSystemApp;
        private readonly PikachuApp _pikachuApp;

        public SysRightController(SysRightApp sysRightApp, SysSystemApp sysSystemApp, PikachuApp pikachuApp)
        {
            _sysRightApp = sysRightApp;
            _sysSystemApp = sysSystemApp;
            _pikachuApp = pikachuApp;
        }

        /// <summary>
        /// 新增权限数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(sys_right obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                //给root_sys_id赋值
                //sys_system Sys = _pikachuApp.GetById<sys_system>(obj.sys_id);
                //obj.root_sys_id = Sys.parent_sys_id;
                if (_sysRightApp.Insert(obj))
                {
                    result.Result = obj.right_id;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "数据写入失败！";
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
        /// 删除权限数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_sysRightApp.DeleteMask<sys_right>(ids))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "操作失败！";
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
        /// 更新权限数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(sys_right obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_right _Right = _pikachuApp.GetById<sys_right>(obj.right_id);
                //如果能够根据id找到
                if (_Right != null)
                {
                    obj.create_time = _Right.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                    result.Result = obj.right_id;


                    if (!_pikachuApp.Update(obj))
                    {
                        //更新失败
                        result.Result = obj.right_id;
                        result.Code = 100;
                        result.Message = "更新失败！";
                    }

                }
                else
                { //找不到要返回错误信息

                    result.Result = obj.right_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
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
        /// 获取系统数据，返回树格式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<sys_right>> GetSysNodes()
        {
            Response<List<sys_right>> result = new Response<List<sys_right>>();
            try
            {
                result.Result = _sysRightApp.GetSysNodes().OrderBy(g => g.create_time).ToList();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 根据系统级ID获取菜单级数据
        /// </summary>
        /// <param name="SysId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetRightBySysId(string SysId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _sysRightApp.GetRightBySysId(SysId, req, ref lcount).OrderBy(g => g.create_time).ToList();
                pd.Total = lcount;
                result.Result = pd;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 搜索按钮接口
        /// </summary>
        /// <param name="SysId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchRightBySysId(string SysId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                //if (req.key != "")
                //{
                //    pd.Data = _pikachuApp.GetList<sys_right>(req, ref lcount).Where(a => a.right_type == "1" && a.parent_right_id == SysId);
                //    pd.Total = lcount;
                //    result.Result = pd;
                //    return result;
                //}
                if (req.key != "" && SysId != null)
                {
                    pd.Data = _pikachuApp.GetList<sys_right>(req, ref lcount).Where(a => a.right_type == "1" && a.parent_right_id == SysId).ToList(); ;
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {
                    pd.Data = _pikachuApp.GetList<sys_right>(req, ref lcount).Where(a => a.right_type == "1").ToList(); ;
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }

                pd.Data = _sysRightApp.GetRightBySysId(SysId, req, ref lcount).OrderBy(g => g.create_time).ToList();
                pd.Total = lcount;
                result.Result = pd;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 根据权限级ID获取按钮级数据
        /// </summary>
        /// <param name="rightId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetButtonByRightId(string rightId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _sysRightApp.GetButtonByRightId(rightId, req, ref lcount).OrderBy(g => g.create_time).ToList();
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 返回树结构用于给角色权限页面菜单栏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<TreeLevel>> ObtainAllNodes()
        {
            Response<List<TreeLevel>> result = new Response<List<TreeLevel>>();
            try
            {
                List<sys_right> sysRights = _pikachuApp.GetAll<sys_right>().OrderBy(g => g.create_time).ToList();

                //List<sys_right> sysSystemNodes = sysRights.Where(a => a.right_type == "0").ToList();
                //List<sys_right> sysRightNodes = sysRights.Where(a => a.right_type == "1").ToList();
                //List<sys_right> sysSubRightNodes = sysRights.Where(a => a.right_type == "2").ToList();

                List<TreeModel> sysRightNodes = _pikachuApp.ListElementToNode(sysRights, false);

                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(sysRightNodes);
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
        /// 废弃
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<MenuTree>> ObtainSysRoleNodes()
        {
            Response<List<MenuTree>> result = new Response<List<MenuTree>>();
            try
            {
                List<sys_right> sysRights = _pikachuApp.GetAll<sys_right>();

                List<sys_right> sysSystemNodes = sysRights.Where(a => a.right_type == "0").ToList();
                List<sys_right> sysRightNodes = sysRights.Where(a => a.right_type == "1").ToList();
                //List<sys_right> sysSubRightNodes = sysRights.Where(a => a.right_type == "2").ToList();

                sysSystemNodes.AddRange(sysRightNodes);
                List<MenuTree> sysRightAllNodes = _pikachuApp.ListElementToMenuNode(sysSystemNodes);

                List<MenuTree> treeModels = _pikachuApp.ListToMenuTree(sysRightAllNodes);
                //List<MenuTree> easies = _pikachuApp.TreeModelToTreeLevel(treeModels);
                result.Result = treeModels;

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
