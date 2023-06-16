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

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]

    public class BaseLocationTypeController : ControllerBase
    {
        private readonly BaseLocationTypeApp _app;
        private readonly PikachuApp _pikachuApp;

        public BaseLocationTypeController(BaseLocationTypeApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到地点类型分页列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_location_type>(req, ref lcount);
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
        /// 更新地点类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_location_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_location_type _Location_type = _pikachuApp.GetById<base_location_type>(obj.location_type_id);
                if (_Location_type != null)//如果能够根据id找到内容
                {
                    obj.location_type_code = _Location_type.location_type_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Location_type.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.location_type_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.location_type_id;

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
        /// 根据地点类型IDs 删除地点类型和所有有关联的地点  可批量删除
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

                bool temp_result_ = _pikachuApp.DeleteMask<base_location_type>(ids);

                if (!temp_result_)
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
        /// 新建地点类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_location_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_location_type getbycode = _pikachuApp.GetByCode<base_location_type>(obj.location_type_code);
                base_location_type getbyname = _pikachuApp.GetByName<base_location_type>(obj.location_type_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.location_type_name, obj.location_type_code);
                    return result;
                }

                base_location_type data = _app.InsertBaseLoctionType(obj);
                if (data != null)
                {
                    result.Result = data.location_type_id;
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
        /// 根据ID得到地点类型明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_location_type> GetDetail(string id)
        {
            Response<base_location_type> result = new Response<base_location_type>();
            try
            {
                result.Result = _pikachuApp.GetById<base_location_type>(id);
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
        public Response<List<base_location_type>> GetAll()
        {
            Response<List<base_location_type>> result = new Response<List<base_location_type>>();
            try
            {
                result.Result = _pikachuApp.GetRootAndBranch<base_location_type>(null);
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
