using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseSuppliersController : ControllerBase
    {
        private readonly BaseSupplierApp _app;
        private readonly PikachuApp _pikachuApp;
        public BaseSuppliersController(BaseSupplierApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到供应商列表数据
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
                pd.Data = _pikachuApp.GetList<base_supplier>(req, ref lcount);
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
        /// 更新供应商信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_supplier obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_supplier _Supplier = _pikachuApp.GetById<base_supplier>(obj.supplier_id);
                if (_Supplier != null)//如果能够根据id找到内容
                {
                    obj.supplier_code = _Supplier.supplier_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Supplier.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.supplier_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.supplier_id;

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
        /// 根据用户ID 删除供应商数据  可批量删除
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
                if (!_pikachuApp.DeleteMask<base_supplier>(ids))
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
        /// 新建供应商信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_supplier obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_supplier getbycode = _pikachuApp.GetByCode<base_supplier>(obj.supplier_code);
                base_supplier getbyname = _pikachuApp.GetByName<base_supplier>(obj.supplier_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.supplier_name, obj.supplier_code);
                    return result;
                }

                base_supplier data = _app.InserSuppliers(obj);
                if (data != null)
                {
                    result.Result = data.supplier_id;
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
        ///  根据ID得到供应商明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_supplier> GetDetail(string id)
        {
            Response<base_supplier> result = new Response<base_supplier>();
            try
            {
                result.Result = _pikachuApp.GetById<base_supplier>(id);
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
