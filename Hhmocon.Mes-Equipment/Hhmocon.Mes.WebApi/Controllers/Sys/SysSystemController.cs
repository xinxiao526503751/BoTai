using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers
{
    /// <summary>
    /// 系统接口(已废弃)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]

    public class SysSystemController : ControllerBase
    {
        private readonly SysSystemApp _app;
        private readonly PikachuApp _pikachuApp;
        private readonly SysRightApp _sysRightApp;
        public SysSystemController(SysSystemApp app, SysRightApp sysRightApp, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
            _sysRightApp = sysRightApp;
        }

        /// <summary>
        /// 新增系统数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(sys_system obj)
        {
            Response<string> result = new Response<string>();
            try
            {

                if (_app.Insert(obj))
                {
                    result.Result = obj.sys_id;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "数据name重复，写入失败！";
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
        /// 删除系统数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                foreach (string id in ids)
                {
                    long count = 0;
                    List<sys_system> sys_Systems = _app.GetChildrens(id);
                    // List<sys_right> sys_right = _sysRightApp.GetBySysId(id,null, ref count);
                    if (sys_Systems.Count != 0)
                    {
                        result.Code = 100;
                        result.Message = "该级系统下有子系统未删除";
                    }
                    if (count != 0)
                    {
                        result.Code = 100;
                        result.Message = "该级系统下有权限未删除";
                    }
                    else
                    {
                        result.Result = ids;

                        if (!_pikachuApp.DeleteMask<sys_system>(ids))
                        {
                            //更新失败
                            result.Code = 100;
                            result.Message = "操作失败！";
                        }
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
        /// 更新系统数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(sys_system obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_system _System = _pikachuApp.GetById<sys_system>(obj.sys_id);
                //如果能够根据id找到
                if (_System != null)
                {
                    obj.create_time = _System.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.sys_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }

                result.Result = obj.sys_id;

                if (!_pikachuApp.Update(obj))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "更新失败！";
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
        /// 遍历获取所有根节点及下面的子节点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<sys_system>> ObtainAllNodes()
        {
            Response<List<sys_system>> result = new Response<List<sys_system>>();
            try
            {
                result.Result = _app.GetAllNodes();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 返回树结构用于页面左边Sys加Right
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<MenuTree>> ObtainSysRigNodes()
        {
            Response<List<MenuTree>> result = new Response<List<MenuTree>>();
            try
            {
                List<sys_right> sysRights = _pikachuApp.GetAll<sys_right>();
                List<sys_system> sysSystems = _pikachuApp.GetAll<sys_system>();

                List<MenuTree> sysRightNodes = _pikachuApp.ListElementToMenuNode(sysRights);
                List<MenuTree> sysSystemNodes = _pikachuApp.ListElementToMenuNode(sysSystems);
                sysRightNodes.AddRange(sysSystemNodes);
                List<MenuTree> treeModels = _pikachuApp.ListToMenuTree(sysRightNodes);
                result.Result = treeModels;
                //result.Result = _sysRightApp.GetAllNodes();

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

