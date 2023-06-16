using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Fault
{
    /// <summary>
    /// 事件定义控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Fault", IgnoreApi = false)]
    public class BaseFaultController : ControllerBase
    {
        private readonly BaseFaultApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public BaseFaultController(BaseFaultApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;

        }
        /// <summary>
        /// 新建事件定义
        /// </summary>
        /// <param name="obj">新增的对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_fault obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault getbycode = _pikachuApp.GetByCode<base_fault>(obj.fault_code);
                base_fault getbyname = _pikachuApp.GetByName<base_fault>(obj.fault_name);

                if (getbycode != null || getbyname != null)
                {
                    result.Code = 100;
                    result.Message = "code或name重复";
                    return result;
                }

                base_fault data = _app.InsertBaseFault(obj);
                if (data != null)
                {
                    result.Result = data.fault_id;
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
        /// 删除事件定义
        /// </summary>
        /// <param name="ids">删除的对象ids</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                _app.Delete(ids);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 修改事件定义
        /// </summary>
        /// <param name="obj">修改后的对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_fault obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault _Base_Fault = _pikachuApp.GetById<base_fault>(obj.fault_id);
                if (_Base_Fault != null)
                {
                    obj.fault_code = _Base_Fault.fault_code;//锁死code
                    obj.create_time = _Base_Fault.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                {
                    result.Result = obj.fault_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.fault_id;

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
        /// 得到事件列表数据
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
                pd.Data = _pikachuApp.GetList<base_fault>(req, ref lcount);
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
        /// 事件定义页面的搜索框
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(FaultSearchBarRequest equipmentSearchBarRequest)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData
                {
                    Data = _app.SearchBar(equipmentSearchBarRequest)
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
        /// 得到事件详细
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_fault> GetDetail(string id)
        {
            Response<base_fault> result = new Response<base_fault>();
            try
            {
                result.Result = _pikachuApp.GetById<base_fault>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 通过事件类型id获取挂载的事件
        /// </summary>
        /// <param name="fault_class_id">事件类型id</param>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetFaultByFaultClassId(string fault_class_id, PageReq req)
        {

            PageData pd = new PageData();
            long lcount = 0;

            Response<PageData> result = new Response<PageData>();
            try
            {
                if (fault_class_id == null)
                {
                    pd.Data = _pikachuApp.GetList<base_fault>(req, ref lcount);
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;
                }

                List<base_fault> data = _app.GetFaultByFaultClassId(fault_class_id);
                //分页
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    pd.Data = data.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                    pd.Total = pd.Data.Count;
                }

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
