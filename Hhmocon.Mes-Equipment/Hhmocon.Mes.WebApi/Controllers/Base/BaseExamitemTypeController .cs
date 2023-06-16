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
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class BaseExamitemTypeController : ControllerBase
    {
        private readonly BaseExamitemTypeApp _app;
        private readonly PikachuApp _pikachuApp;
        public BaseExamitemTypeController(BaseExamitemTypeApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到点检项目类型分页列表数据/维修/保养
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
                pd.Data = _pikachuApp.GetList<base_examitem_type>(req, ref lcount);
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
        /// 更新点检项目类型信息/维修/保养
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_examitem_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_examitem_type _Process = _pikachuApp.GetById<base_examitem_type>(obj.examitem_type_id);
                if (_Process != null)//如果能够根据id找到内容
                {
                    obj.examitem_type_code = _Process.examitem_type_code;//锁死code
                    obj.modified_time = DateTime.Now;        //更新修改时间
                    obj.create_time = _Process.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.examitem_type_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.examitem_type_id;

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
        /// 根据ID删除点检项目，可批量删除（假删除）点检/维修/保养
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

                if (!_pikachuApp.DeleteMask<base_examitem_type>(ids))
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
        /// 新建点检项目类型信息/维修/保养
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_examitem_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                List<base_examitem_type> getbycodes = _pikachuApp.GetByOneFeildsSql<base_examitem_type>("examitem_type_code", obj.examitem_type_code);
                base_examitem_type getbycode = getbycodes.Where(c => c.method_type == obj.method_type).FirstOrDefault();
                List<base_examitem_type> getbynames = _pikachuApp.GetByOneFeildsSql<base_examitem_type>("examitem_type_name", obj.examitem_type_name);
                base_examitem_type getbyname = getbynames.Where(c => c.method_type == obj.method_type).FirstOrDefault();

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.examitem_type_name, obj.examitem_type_code);
                    return result;
                }


                base_examitem_type data = _app.InsertExamitemType(obj);
                if (data != null)
                {
                    result.Result = data.examitem_type_id;
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
        /// 根据ID得到点检项目类型明细信息/维修/保养
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_examitem_type> GetDetail(string id)
        {
            Response<base_examitem_type> result = new Response<base_examitem_type>();
            try
            {
                result.Result = _pikachuApp.GetById<base_examitem_type>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method_type">"1" "2" "3"</param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_examitem_type>> GetAll(string method_type)
        {
            Response<List<base_examitem_type>> result = new Response<List<base_examitem_type>>();
            try
            {
                result.Result = _pikachuApp.GetRootAndBranch<base_examitem_type>(null).Where(c => c.method_type == method_type).ToList();
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
