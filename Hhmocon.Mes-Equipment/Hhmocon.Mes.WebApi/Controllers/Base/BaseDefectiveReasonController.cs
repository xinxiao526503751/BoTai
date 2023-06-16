using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Defective", IgnoreApi = false)]
    public class BaseDefectiveReasonController : ControllerBase
    {
        private readonly BaseDefectiveReasonApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public BaseDefectiveReasonController(BaseDefectiveReasonApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;

        }

        /// <summary>
        /// 新建缺陷定义信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_defective_reason obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_defective_reason getbycode = _pikachuApp.GetByCode<base_defective_reason>(obj.defective_reason_code);
                base_defective_reason getbyname = _pikachuApp.GetByOneFeildsSql<base_defective_reason>("defective_reason_name", obj.defective_reason_name).ToList().FirstOrDefault();

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.defective_reason_name, obj.defective_reason_code);
                    return result;
                }

                base_defective_reason data = _app.Insert(obj);
                if (data != null)
                {
                    result.Result = data.defective_reason_id;
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
        /// 根据ID 删除数据  可批量删除
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

                if (!_pikachuApp.DeleteMask<base_defective_reason>(ids))
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
        /// 更新信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_defective_reason obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_defective_reason _Reason = _pikachuApp.GetById<base_defective_reason>(obj.defective_reason_id);
                //如果能够根据id找
                if (_Reason != null)
                {
                    obj.defective_reason_code = _Reason.defective_reason_code;//锁死code
                    obj.create_time = _Reason.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到顾客要返回错误信息
                    result.Result = obj.defective_reason_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.defective_reason_id;

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
        /// 根据不合格类型获取定义
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetReasonByType(string typeId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                //pd.Data = _pikachuApp.GetList<base_defective_reason>();
                List<base_defective_reason> data = string.IsNullOrEmpty(typeId)
                   ? _pikachuApp.GetAll<base_defective_reason>().OrderBy(a => a.defective_reason_code).ToList()
                   : _pikachuApp.GetByOneFeildsSql<base_defective_reason>("defective_type_id", typeId).OrderBy(a => a.defective_reason_code).ToList();
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    pd.Data = data?.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Total = data != null ? data.Count : 0;
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
        /// 得到列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_defective_reason>(req, ref lcount);
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

        [HttpPost]
        public Response<List<base_defective_reason>> GetAll()
        {
            Response<List<base_defective_reason>> result = new Response<List<base_defective_reason>>();
            try
            {
                result.Result = _pikachuApp.GetAll<base_defective_reason>();

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
