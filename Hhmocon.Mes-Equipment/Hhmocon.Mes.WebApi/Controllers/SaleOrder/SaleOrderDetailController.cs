using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.SaleOrder
{
    /// <summary>
    /// 物料订单控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "SaleOrder", IgnoreApi = false)]
    [ApiController]
    public class SaleOrderDetailController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly SaleOrderDetailApp _saleOrderDetailApp;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="picachuApp"></param>
        /// <param name="saleOrderApp"></param>
        public SaleOrderDetailController(PikachuApp picachuApp, SaleOrderDetailApp saleOrderDetailApp)
        {
            _pikachuApp = picachuApp;
            _saleOrderDetailApp = saleOrderDetailApp;
        }


        /// <summary>
        /// 新增物料订单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(sale_order_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_saleOrderDetailApp.Insert(obj))
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
        /// 删除物料订单
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

                if (!_pikachuApp.DeleteMask<sale_order_detail>(ids))
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
        public Response<string> Update(sale_order_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sale_order_detail sale_Order = _pikachuApp.GetById<sale_order_detail>(obj.sale_order_detail_id);

                if (sale_Order != null)
                {
                    obj.sale_order_detail_code = sale_Order.sale_order_detail_code;//锁死code
                    obj.create_time = sale_Order.create_time;//锁死创建时间
                    obj.start_time = sale_Order.start_time;
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.sale_order_detail_id;
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
        /// 根据订单id获取订单详情
        /// </summary>
        /// <param name="req"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetListByOrderId(PageReq req, string orderId)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _saleOrderDetailApp.GetListByOrderId(req, ref lcount, orderId)?.OrderByDescending(a => a.create_time).ToList();
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
