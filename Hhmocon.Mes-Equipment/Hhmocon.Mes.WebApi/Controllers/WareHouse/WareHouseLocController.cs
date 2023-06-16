using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.WareHouse
{
    /// <summary>
    /// 仓库位置控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WareHouse", IgnoreApi = false)]
    public class WareHouseLocController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly WareHouseLocApp _WareHouseLocApp;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="WareHouseLocApp"></param>
        /// <param name="mapper"></param>
        public WareHouseLocController(PikachuApp pikachuApp, WareHouseLocApp WareHouseLocApp, IMapper mapper)
        {
            _pikachuApp = pikachuApp;
            _WareHouseLocApp = WareHouseLocApp;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增仓库库位
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(WareHouseLocRequest obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_WareHouseLocApp.Insert(obj))
                {
                    result.Result = obj.warehouse_id;
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
        /// 删除仓库库位，如果库位下有库存，需要给提示
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="check_flag">
        /// true代表删前检查引用，
        /// false代表直接删掉。</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids, bool check_flag = true)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;
                if (ids.Length == 0 || ids == null)
                {
                    throw new Exception("未选中库位");
                }

                if (check_flag)
                {
                    //查找用到warehouse_loc_id字段的表下是不是有数据
                    List<string> chartNames = _pikachuApp.GetAllChartNameHavingSameField("warehouse_loc_id");
                    List<string> chartExistsData = new();
                    foreach (string id in ids)
                    {
                        foreach (string chart in chartNames)
                        {
                            _WareHouseLocApp.CheckChartIfExistsData(ref chartExistsData, id, chart);
                            if (chartExistsData.Count > 0)
                            {
                                string chars = string.Join(",", chartExistsData.ToArray());
                                switch (chars)
                                {
                                    case "base_stock":
                                        List<base_stock> base_Stocks = _pikachuApp.GetByOneFeildsSql<base_stock>("warehouse_loc_id", id);
                                        if (base_Stocks.Count != 0)
                                        {
                                            throw new Exception($"操作的库位下存在库存，操作失败");
                                        }
                                        break;

                                }

                            }
                        }
                    }

                    _WareHouseLocApp.deleteWarehouseLoc(ids);
                }
                else
                {
                    _WareHouseLocApp.deleteWarehouseLoc(ids);
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
        /// 更新库位定义
        /// </summary>
        /// <param name="obj">仓库定义对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_warehouse_loc obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_warehouse_loc base_Warehouse = _pikachuApp.GetById<base_warehouse_loc>(obj.warehouse_loc_id);

                if (base_Warehouse != null)
                {
                    obj.warehouse_loc_code = base_Warehouse.warehouse_loc_code;//锁死code
                    obj.create_time = base_Warehouse.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.warehouse_loc_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.warehouse_loc_id;

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
        /// 得到库位列表数据
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                List<BaseWareHouseLocResponse> baseWareHouseLocResponses = new();
                PageData pd = new PageData();
                long lcount = 0;
                List<base_warehouse_loc> base_Warehouses_loc = _pikachuApp.GetList<base_warehouse_loc>(req, ref lcount);
                foreach (base_warehouse_loc temp in base_Warehouses_loc)
                {
                   
                    BaseWareHouseLocResponse temp_baseWareHouseLocResponse = new();
                    _mapper.Map(temp, temp_baseWareHouseLocResponse);
                    temp_baseWareHouseLocResponse.warehouse_name = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id)?.warehouse_name;
      
                    baseWareHouseLocResponses.Add(temp_baseWareHouseLocResponse);
                }
                pd.Data = baseWareHouseLocResponses;
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


        ///// <summary>
        ///// 对库位进行出入库操作
        ///// </summary>
        ///// <param name="WareHouseRecs"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Response<string> IoWareHouseOperationForLoc(List<warehouse_io_rec> WareHouseRecs)
        //{
        //    var result = new Response<string>();
        //    try
        //    {
        //        //_WareHouseIoRecApp.IoWareHouseOperation(WareHouseRecs);
        //        result.Result = "操作成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Code = 500;
        //        result.Message = ex.InnerException?.Message ?? ex.Message;
        //    }
        //    return result;
        //}


    }
}
