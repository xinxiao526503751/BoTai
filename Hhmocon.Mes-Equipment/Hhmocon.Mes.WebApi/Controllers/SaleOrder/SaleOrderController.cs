using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.SaleOrder
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "SaleOrder", IgnoreApi = false)]
    [ApiController]
    public class SaleOrderController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly SaleOrderApp _saleOrderApp;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="picachuApp"></param>
        /// <param name="saleOrderApp"></param>
        public SaleOrderController(PikachuApp picachuApp, SaleOrderApp saleOrderApp)
        {
            _pikachuApp = picachuApp;
            _saleOrderApp = saleOrderApp;
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Create(sale_order obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_saleOrderApp.Insert(obj))
                {
                    result.Result = obj.sale_order_id;
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
        /// 删除订单
        /// </summary>
        /// <param name="ids">仓库定义id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<sale_order>(ids))
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
        /// 更新订单
        /// </summary>
        /// <param name="obj">仓库定义对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(sale_order obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sale_order sale_Order = _pikachuApp.GetById<sale_order>(obj.sale_order_id);

                if (sale_Order != null)
                {
                    obj.sale_order_code = sale_Order.sale_order_code;//锁死code
                    obj.create_time = sale_Order.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.sale_order_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.sale_order_id;

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
        /// 得到列表列表数据
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
                pd.Data = _saleOrderApp.GetSaleOrderList(req, ref lcount);
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
