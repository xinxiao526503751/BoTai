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
    /// 单位定义控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysParmController
    {
        private readonly SysParmApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// 单位控制器
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pikachuApp"></param>
        public SysParmController(SysParmApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 新增单位
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(sys_parm obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.Insert(obj))
                {
                    result.Result = obj.parm_id;
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
        /// 删除单位
        /// </summary>
        /// <param name="ids">单位id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<sys_parm>(ids))
                {
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
        /// 更新单位信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [HttpPost]
        public Response<string> Update(sys_parm obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_parm base_Warehouse_Type = _pikachuApp.GetById<sys_parm>(obj.parm_id);

                if (base_Warehouse_Type != null)
                {
                    obj.parm_code = base_Warehouse_Type.parm_code;//锁死code
                    obj.create_time = base_Warehouse_Type.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.parm_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.parm_id;

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
        /// 得到单位列表数据
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
                List<sys_parm> sys_Parms = _pikachuApp.GetList<sys_parm>(req, ref lcount);
                pd.Data = sys_Parms;
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
        /// 通过参数类型名称获取参数类型下的参数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ParamTypeName"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetParamByParamType(PageReq req, string ParamTypeName)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                string type_id = _pikachuApp.GetByOneFeildsSql<sys_parm_type>("parm_type_name", ParamTypeName).FirstOrDefault()?.parm_type_id;
                if (type_id == null)
                {
                    throw new Exception($"无效的参数类型名称{ParamTypeName}");
                }

                List<sys_parm> sys_Parms = _pikachuApp.GetAll<sys_parm>().Where(c => c.parm_type_id == type_id).ToList();
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    sys_Parms = sys_Parms.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = sys_Parms;
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
