
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 按钮权限接口(已废弃)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysSubRightController : ControllerBase
    {
        private readonly SysSubRightApp _sysSubRightApp;
        private readonly PikachuApp _pikachuApp;

        public SysSubRightController(SysSubRightApp sysSubRightApp, PikachuApp pikachuApp)
        {
            _sysSubRightApp = sysSubRightApp;
            _pikachuApp = pikachuApp;
        }

        /// <summary>
        /// 新增按钮权限数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(sys_sub_right obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_sysSubRightApp.Insert(obj))
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
        /// 删除按钮权限数据
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

                if (!_pikachuApp.DeleteMask<sys_sub_right>(ids))
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
        /// 更新按钮权限数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(sys_sub_right obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_sub_right _Right = _pikachuApp.GetById<sys_sub_right>(obj.right_id);
                //如果能够根据id找到
                if (_Right != null)
                {
                    obj.create_time = _Right.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;

                    if (!_pikachuApp.Update(obj))
                    {
                        //更新失败
                        result.Result = obj.right_id;
                        result.Code = 100;
                        result.Message = "更新失败！";
                    }
                }
                else
                {
                    //找不到要返回错误信息

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
        /// 根据上级权限ID获取按钮权限数据
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetByParentId(string ParentId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _sysSubRightApp.GetBySysId(ParentId, req, ref lcount);
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

    }
}
