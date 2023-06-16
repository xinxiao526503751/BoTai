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

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    /// <summary>
    /// 基础设备分类接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Equipment", IgnoreApi = false)]
    public class BaseEquipmentClassController
    {
        private readonly BaseEquipmentClassApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public BaseEquipmentClassController(BaseEquipmentClassApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }
        /// <summary>
        /// 新建设备分类信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_equipment_class obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_equipment_class getbycode = _pikachuApp.GetByCode<base_equipment_class>(obj.equipment_class_code);
                base_equipment_class getbyname = _pikachuApp.GetByName<base_equipment_class>(obj.equipment_class_name);

                if (getbycode != null && getbyname != null)//如果能根据name和code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称和编码重复，数据库中已存在有该名称={0},编码={1}的数据，请重新填写", obj.equipment_class_name, obj.equipment_class_code);
                    return result;
                }
                if (getbycode != null)//如果仅能根据code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("编码重复，数据库中已存在有该编码={0}的数据，请重新填写", obj.equipment_class_code);
                    return result;
                }
                if (getbyname != null)//如果仅能根据id找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("Id重复，数据库中已存在有该Id={0}的数据，请重新填写", obj.equipment_class_name);
                    return result;
                }

                base_equipment_class data = _app.InsertEquipment(obj);
                if (data != null)
                {
                    result.Result = data.equipment_class_id;
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
        /// 删除设备分类信息
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

                if (!_pikachuApp.DeleteMask<base_equipment_class>(ids))
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
        /// 更新设备分类信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_equipment_class obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_equipment_class _Equipment = _pikachuApp.GetById<base_equipment_class>(obj.equipment_class_id);

                if (_Equipment != null)
                {
                    obj.equipment_class_code = _Equipment.equipment_class_code;//锁死code
                    obj.create_time = _Equipment.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                {
                    result.Result = obj.equipment_class_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }

                result.Result = obj.equipment_class_id;

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
        /// 获取设备分类分页数据
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
                pd.Data = _pikachuApp.GetList<base_equipment_class>(req, ref lcount);
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
        /// 根据id得到设备分类明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_equipment_class> GetDetail(string id)
        {
            Response<base_equipment_class> result = new Response<base_equipment_class>();
            try
            {
                result.Result = _pikachuApp.GetById<base_equipment_class>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 页面层级结构左边，获取_parent_id = null的所有数据，以及他们的枝
        /// 效果等同于GetAll,获取表里所有数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_equipment_class>> GetAll()
        {
            Response<List<base_equipment_class>> result = new Response<List<base_equipment_class>>();
            try
            {

                result.Result = _pikachuApp.GetRootAndBranch<base_equipment_class>(null);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 页面右边，传入设备分类id，得到该设备分类及子设备分类
        /// 统一：根据定义找定义参数命名root_id
        ///       根据类型找定义参数命名type_id
        ///        int iPage = req.page;
        ///        int iRows = req.rows;
        ///        //分页只用到这俩参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetLeafs(string root_id, PageReq req)
        {
            Response<PageData> result = new();
            PageData pd = new();
            try
            {
                List<base_equipment_class> result_temp = _pikachuApp.GetRootAndBranch<base_equipment_class>(root_id);
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    result_temp = result_temp.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = result_temp;
                pd.Total = result_temp.Count;
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
        /// 设备分类页面的搜索框，找设备分类
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(EquipmentClassSearchBarRequest equipmentSearchBarRequest)
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
    }
}
