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
    /// <summary>
    /// 缺陷类型控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Defective", IgnoreApi = false)]
    public class BaseDefectiveTypeController : ControllerBase
    {
        private readonly BaseDefectiveTypeApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public BaseDefectiveTypeController(BaseDefectiveTypeApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;

        }

        /// <summary>
        /// 新建缺陷定义类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_defective_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_defective_type getbycode = _pikachuApp.GetByCode<base_defective_type>(obj.defective_type_code);
                base_defective_type getbyname = _pikachuApp.GetByOneFeildsSql<base_defective_type>("defective_type_name", obj.defective_type_name).ToList().FirstOrDefault();//_pikachuApp.GetByName<base_defective_type>(obj.defective_type_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.defective_type_name, obj.defective_type_code);
                    return result;
                }

                base_defective_type data = _app.Insert(obj);
                if (data != null)
                {
                    result.Result = data.defective_type_id;
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

                if (!_pikachuApp.DeleteMask<base_defective_type>(ids))
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
        public Response<string> Update(base_defective_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_defective_type _Customer = _pikachuApp.GetById<base_defective_type>(obj.defective_type_id);
                //如果能够根据id找到顾客
                if (_Customer != null)
                {
                    obj.defective_type_code = _Customer.defective_type_code;//锁死code
                    obj.create_time = _Customer.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到顾客要返回错误信息
                    result.Result = obj.defective_type_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.defective_type_id;

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
                pd.Data = _pikachuApp.GetList<base_defective_type>(req, ref lcount);
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
        /// 返回表中所有数据不分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_defective_type>> GetAll()
        {
            Response<List<base_defective_type>> result = new Response<List<base_defective_type>>();
            try
            {
                result.Result = _pikachuApp.GetAll<base_defective_type>();
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
