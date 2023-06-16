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
    public class BaseMaterialTypeController : ControllerBase
    {
        private readonly BaseMaterialTypeApp _app;
        private readonly PikachuApp _pikachuApp;

        public BaseMaterialTypeController(BaseMaterialTypeApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到材料类型分页列表数据
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
                pd.Data = _pikachuApp.GetList<base_material_type>(req, ref lcount);
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
        public Response<string> Update(base_material_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_material_type _Material_Type = _pikachuApp.GetById<base_material_type>(obj.material_type_id);
                if (_Material_Type != null)//如果能够根据id找到内容
                {
                    obj.material_type_code = _Material_Type.material_type_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Material_Type.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.material_type_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.material_type_id;

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
        /// 根据材料类型ID 假删除地点类型  可批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> DeleteMask(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<base_material_type>(ids))
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
        /// 新建材料类型信息,如果类型编码重复则提示不能添加
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_material_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_material_type getbycode = _pikachuApp.GetByCode<base_material_type>(obj.material_type_code);
                base_material_type getbyname = _pikachuApp.GetByName<base_material_type>(obj.material_type_name);

                if (getbycode != null && getbyname != null)//如果能根据id和code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("Name和Code重复，数据库中已存在有该Name={0},Code={1}的数据，请重新填写", obj.material_type_name, obj.material_type_code);
                    return result;
                }
                if (getbycode != null)//如果仅能根据code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("Code重复，数据库中已存在有该Code={0}的数据，请重新填写", obj.material_type_code);
                    return result;
                }
                if (getbyname != null)//如果仅能根据id找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("Name重复，数据库中已存在有该Id={0}的数据，请重新填写", obj.material_type_name);
                    return result;
                }


                //根据传入的material_type_parentname更改material_type_parentid的值
                if (obj.material_type_parentname != null)
                {
                    base_material_type papa = _pikachuApp.GetByName<base_material_type>(obj.material_type_parentname);
                    if (papa != null)
                    {
                        obj.material_type_parentid = papa.material_type_id;
                    }
                }

                base_material_type data = _app.InsertBaseMaterialType(obj);
                if (data != null)
                {
                    result.Result = data.material_type_id;
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
        /// 根据ID得到材料类型明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_material_type> GetDetail(string id)
        {
            Response<base_material_type> result = new Response<base_material_type>();
            try
            {
                result.Result = _pikachuApp.GetById<base_material_type>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取表里所有数据
        /// 丢给前端自己去判断哪个是根节点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_material_type>> GetAll()
        {
            Response<List<base_material_type>> result = new Response<List<base_material_type>>();
            try
            {
                result.Result = _pikachuApp.GetRootAndBranch<base_material_type>(null);
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
