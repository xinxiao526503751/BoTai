using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.LoginRelated;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysUserController
    {
        private readonly SysUserApp _app;
        private readonly PikachuApp _pikachuApp;
        private readonly SysUserRoleApp _sysUserRoleApp;
        private readonly IMapper _mapper;

        /// <summary>
        /// 用户控制器构造函数
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pikachuApp"></param>
        public SysUserController(SysUserApp app, PikachuApp pikachuApp, SysUserRoleApp sysUserRoleApp, IMapper mapper)
        {
            _app = app;
            _pikachuApp = pikachuApp;
            _sysUserRoleApp = sysUserRoleApp;
            _mapper = mapper;
        }

        /// <summary>
        /// 新建用户信息
        /// </summary>
        /// <param name="obj">用户对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(CreateUserRequest obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.Insert(obj))
                {
                    result.Result = obj.user_cn_name;
                }
                else
                {
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
        /// 删除用户信息
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;
                if (ids.Length == 0)
                {
                    throw new Exception("请选择要删除的对象");
                }
                List<string> idss = new(ids);
                _app.Delete(idss);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="updateUserRequest">用户对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(UpdateUserRequest updateUserRequest)
        {
            Response<string> result = new Response<string>();
            try
            {
                _app.Update(updateUserRequest);


            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 部门下的用户界面的 左上角的 搜索框
        /// 功能:用户输入name,根据name或ch_name查询
        /// 选中部门时，会传入dept_id，不选中时传入的dept_id=null
        /// </summary>
        /// <param name="sysUserSearchBar"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(SysUserSearchBarRequest sysUserSearchBar)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData
                {
                    Data = _app.SearchBar(sysUserSearchBar)
                };
                pd.Total = pd.Data.Count;
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
        /// 得到用户列表数据
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                List<sys_user> sys_Users = _pikachuApp.GetList<sys_user>(req, ref lcount);
                List<UserResponse> userResponses = new();
                foreach (sys_user temp in sys_Users)
                {
                    UserResponse userResponse = new();
                    userResponse = _mapper.Map<UserResponse>(temp);
                    userResponse.dept_name = _pikachuApp.GetById<sys_dept>(temp.dept_id).dept_name;
                    userResponse.role_name = new();
                    userResponse.role_id = new();
                    List<sys_user_role> d = _pikachuApp.GetByOneFeildsSql<sys_user_role>("user_id", temp.user_id);
                    foreach (sys_user_role sys_user_role in d)
                    {
                        sys_role role_temp = _pikachuApp.GetById<sys_role>(sys_user_role.role_id);
                        if (role_temp != null)
                        {
                            userResponse.role_id.Add(sys_user_role.role_id);
                            userResponse.role_name.Add(role_temp.role_name);
                        }
                    }
                    userResponses.Add(userResponse);
                }

                pd.Data = userResponses;
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
        ///  根据ID得到用户明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<sys_user> GetDetail(string id)
        {
            Response<sys_user> result = new Response<sys_user>();
            try
            {
                result.Result = _pikachuApp.GetById<sys_user>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> ChangePassword(ChangePasswordReq request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.ChangePassword(request) != null)
                {
                    //更新失败
                    result.Code = 200;
                    result.Message = "操作成功！";
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
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> ResetPassword(ChangePasswordReq request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.ResetPassword(request) != null)
                {
                    //更新失败
                    result.Code = 200;
                    result.Message = "操作成功！";
                }
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
