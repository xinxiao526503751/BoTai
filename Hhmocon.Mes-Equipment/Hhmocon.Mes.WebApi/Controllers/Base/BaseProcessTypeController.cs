using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseProcessTypeController : ControllerBase
    {
        private readonly BaseProcessTypeApp _app;
        private readonly PikachuApp _pikachuApp;
        public BaseProcessTypeController(BaseProcessTypeApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }



        /// <summary>
        /// 得到工序类型分页列表数据
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
                pd.Data = _pikachuApp.GetList<base_process_type>(req, ref lcount);
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
        /// 更新工序类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_process_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process_type _Process_type = _pikachuApp.GetById<base_process_type>(obj.process_type_id);
                if (_Process_type != null)//如果能够根据id找到内容
                {
                    obj.process_type_code = _Process_type.process_type_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Process_type.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.process_type_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.process_type_id;

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
        /// 根据ID 删除工序类型  可批量删除
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

                if (!_pikachuApp.DeleteMask<base_process_type>(ids))
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
        /// 新建工序类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_process_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process_type getbycode = _pikachuApp.GetByCode<base_process_type>(obj.process_type_code);
                base_process_type getbyname = _pikachuApp.GetByName<base_process_type>(obj.process_type_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，数据库中已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.process_type_name, obj.process_type_code);
                    return result;
                }


                //根据传入的process_type_parentname更改process_type_parentId的值
                if (obj.process_type_parentname != null)
                {
                    base_process_type papa = _pikachuApp.GetByName<base_process_type>(obj.process_type_parentname);
                    if (papa != null)
                    {
                        obj.process_type_parentid = papa.process_type_id;
                    }
                }

                base_process_type data = _app.InsertBaseProcessType(obj);
                if (data != null)
                {
                    result.Result = data.process_type_id;
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
        /// 根据ID得到工序类型明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_process_type> GetDetail(string id)
        {
            Response<base_process_type> result = new Response<base_process_type>();
            try
            {
                result.Result = _pikachuApp.GetById<base_process_type>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取所有parentname is null的工序类型及子类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_process_type>> GetAll()
        {
            Response<List<base_process_type>> result = new Response<List<base_process_type>>();
            try
            {
                result.Result = _pikachuApp.GetRootAndBranch<base_process_type>(null);
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
