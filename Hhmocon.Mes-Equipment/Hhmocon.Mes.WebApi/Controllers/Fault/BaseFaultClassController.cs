using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Fault
{
    /// <summary>
    /// 事件类型控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Fault", IgnoreApi = false)]
    public class BaseFaultClassController : ControllerBase
    {
        private readonly BaseFaultClassApp _app;
        private readonly PikachuApp _pikachuApp;

        public BaseFaultClassController(BaseFaultClassApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;

        }
        /// <summary>
        /// 新建事件类型定义
        /// </summary>
        /// <param name="obj">新增的对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_fault_class obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault_class getbycode = _pikachuApp.GetByCode<base_fault_class>(obj.fault_class_code);
                base_fault_class getbyname = _pikachuApp.GetByName<base_fault_class>(obj.fault_class_name);

                if (getbycode != null || getbyname != null)
                {
                    result.Code = 100;
                    result.Message = "code或name重复";
                    return result;
                }

                base_fault_class data = _app.InsertBaseFaultClass(obj);

                if (data != null)
                {
                    result.Result = data.fault_class_id;
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
        /// 删除事件类型定义
        /// </summary>
        /// <param name="ids">删除的id，支持多选</param>
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
        /// 修改事件类型定义
        /// </summary>
        /// <param name="obj">更改后的对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_fault_class obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault_class _Base_Fault_Class = _pikachuApp.GetById<base_fault_class>(obj.fault_class_id);
                if (_Base_Fault_Class != null)
                {
                    obj.fault_class_code = _Base_Fault_Class.fault_class_code;//锁死code
                    obj.create_time = _Base_Fault_Class.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                {
                    result.Result = obj.fault_class_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.fault_class_id;

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
        /// 得到事件类型列表数据
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
                pd.Data = _pikachuApp.GetList<base_fault_class>(req, ref lcount);
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
        /// 得到事件类型详细
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_fault_class> GetDetail(string id)
        {
            Response<base_fault_class> result = new Response<base_fault_class>();
            try
            {
                result.Result = _pikachuApp.GetById<base_fault_class>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }



        /// <summary>
        /// 以Tree的形式获取表里所有数据
        /// </summary>
        /// <returns>List[TreeModel]形式</returns>
        [HttpPost]
        public Response<List<TreeModel>> GetAll()
        {
            Response<List<TreeModel>> result = new Response<List<TreeModel>>();
            try
            {
                List<base_fault_class> datas = _pikachuApp.GetAll<base_fault_class>();
                List<TreeModel> treeEasies = _app.ListElementToNode(datas);
                treeEasies = _pikachuApp.ListToTreeModel(treeEasies);
                result.Result = treeEasies;
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
