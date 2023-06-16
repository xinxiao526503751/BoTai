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
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]

    public class BaseMaterialController : ControllerBase
    {
        private readonly BaseMaterialApp _app;
        private readonly PikachuApp _pikachuApp;

        public BaseMaterialController(BaseMaterialApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 新建物料信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_material obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_material getbycode = _pikachuApp.GetByCode<base_material>(obj.material_code);
                base_material getbyname = _pikachuApp.GetByName<base_material>(obj.material_name);

                if (getbycode != null)//如果仅能根据code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("编码重复，数据库中已存在有该编码={0}的数据，请重新填写", obj.material_code);
                    return result;
                }

                //新建时根据_type_id自动生成_type_code和_type_name
                base_material_type _base_Material_type = _pikachuApp.GetById<base_material_type>(obj.material_type_id);

                //如果能够根据新建material的material_type_id找到内容
                if (_base_Material_type != null)
                {
                    obj.material_type_code = _base_Material_type.material_type_code;//锁死code
                    obj.material_type_name = _base_Material_type.material_type_name;//锁死name
                }
                else
                {
                    result.Code = 100;
                    result.Message = "新建物料信息时不能填写不存在的物料类型id";
                    return result;
                }

                base_material data = _app.InsertBaseMaterial(obj);
                if (data != null)
                {
                    result.Result = data.material_id;
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
        /// 物料类型下的物料界面的 左上角的 搜索框
        /// 功能:输入code或者name,根据code或者name查询
        /// 选中物料类型时，会传入type_id，不选中时传入的dept_id=null
        /// 
        /// 
        /// 有错  为解决
        /// </summary>
        /// <param name="materialSearchBarRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(MaterialSearchBarRequest materialSearchBarRequest)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData
                {
                    Data = _app.SearchBar(materialSearchBarRequest)
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
        /// 得到物料分页列表数据
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
                pd.Data = _pikachuApp.GetList<base_material>(req, ref lcount);

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
        /// 根据传入的material_type_name查找所有material的属性有该materialTypeId的元素
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetAllBy_materialTypeName(string material_type_name)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();

                long lcount = 0;
                pd.Data = _app.GetBaseMaterial_ByMaterialTypeName(material_type_name, ref lcount);
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
        /// 更新物料信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_material obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_material _Material = _pikachuApp.GetById<base_material>(obj.material_id);
                if (_Material != null)//如果能够根据id找到内容
                {
                    obj.material_code = _Material.material_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Material.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.material_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }


                result.Result = obj.material_id;

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
        /// 根据物料ID 删除物料  可批量删除  假删除
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

                if (!_pikachuApp.DeleteMask<base_material>(ids))
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
        /// 根据ID得到材料类型明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_material> GetDetail(string id)
        {
            Response<base_material> result = new Response<base_material>();
            try
            {
                result.Result = _pikachuApp.GetById<base_material>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据传入的type_id和分页数据找叶
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetLeafs(string type_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            long icount = 0;
            try
            {
                if (req.key != "" && type_id != null)
                {
                    long lcount = 0;
                    pd.Data = _pikachuApp.GetList<base_material>(req, ref lcount).Where(a => a.material_type_id == type_id).OrderByDescending(c=>c.create_time).ToList();
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {
                    long lcount = 0;
                    pd.Data = _pikachuApp.GetList<base_material>(req, ref lcount).OrderByDescending(c => c.create_time).ToList();
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }

                //获取类型树
                List<base_material_type> base_Material_Types = _pikachuApp.GetRootAndBranch<base_material_type>(type_id);
                //获取挂载了类型树的定义
                List<base_material> base_Materials = new();
                foreach (base_material_type temp in base_Material_Types)
                {

                    List<base_material> r = _pikachuApp.GetByOneFeildsSql<base_material>("material_type_id", temp.material_type_id).OrderBy(c => c.create_time).ToList();
                    if (r.Count != 0)
                    {
                        base_Materials.AddRange(r);
                    }

                }

                icount = base_Materials.Count;


                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    base_Materials = base_Materials.Skip((iPage - 1) * iRows).Take(iRows).OrderByDescending(c => c.create_time).ToList();
                }
                pd.Data = base_Materials;
                pd.Total = icount;
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
