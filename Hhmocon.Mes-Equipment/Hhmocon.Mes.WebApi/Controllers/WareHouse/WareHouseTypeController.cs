using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.WareHouse
{
    /// <summary>
    /// 仓库类型控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WareHouse", IgnoreApi = false)]
    public class WareHouseTypeController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly WareHouseTypeApp _WareHouseTypeApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="WareHouseTypeApp"></param>
        public WareHouseTypeController(PikachuApp pikachuApp, WareHouseTypeApp WareHouseTypeApp)
        {
            _pikachuApp = pikachuApp;
            _WareHouseTypeApp = WareHouseTypeApp;
        }

        /// <summary>
        /// 新增仓库类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(base_warehouse_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_WareHouseTypeApp.Insert(obj))
                {
                    result.Result = obj.warehouse_type_id;
                }
                else
                {
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
        /// 删除仓库类型信息
        /// </summary>
        /// <param name="ids">仓库类型id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                foreach (string id in ids)
                {
                    //检擦仓库中是否有使用仓库类型
                    List<base_warehouse> base_Warehouses = _pikachuApp.GetByOneFeildsSql<base_warehouse>("warehouse_type_id", id);
                    if (base_Warehouses.Count != 0)
                    {
                        throw new Exception("有仓库正在使用准备删除的仓库类型，删除失败");
                    }

                }

                if (!_pikachuApp.DeleteMask<base_warehouse_type>(ids))
                {
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
        /// 更新出仓库类型信息
        /// </summary>
        /// <param name="obj">仓库类型对象</param>
        /// <returns></returns>

        [HttpPost]
        public Response<string> Update(base_warehouse_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_warehouse_type base_Warehouse_Type = _pikachuApp.GetById<base_warehouse_type>(obj.warehouse_type_id);
                //如果能够根据id找到出入库类型信息
                if (base_Warehouse_Type != null)
                {
                    obj.warehouse_type_code = base_Warehouse_Type.warehouse_type_code;//锁死code
                    obj.create_time = base_Warehouse_Type.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.warehouse_type_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.warehouse_type_id;

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
        /// 得到出入库列表数据
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
                pd.Data = _pikachuApp.GetList<base_warehouse_type>(req, ref lcount);
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
    }
}
