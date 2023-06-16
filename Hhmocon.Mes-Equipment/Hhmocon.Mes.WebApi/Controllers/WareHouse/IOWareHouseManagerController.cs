using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Application.Response.WareHouse;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.WareHouse
{
    /// <summary>
    /// 出入库管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WareHouse", IgnoreApi = false)]
    public class IOWareHouseManagerController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly WareHouseApp _WareHouseApp;
        private readonly WareHouseIoRecApp _WareHouseIoRecApp;
        private readonly IMapper _mapper;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="WareHouseApp"></param>
        public IOWareHouseManagerController(PikachuApp pikachuApp, WareHouseApp WareHouseApp, WareHouseIoRecApp WareHouseIoRecApp, IMapper mapper)
        {
            _pikachuApp = pikachuApp;
            _WareHouseApp = WareHouseApp;
            _WareHouseIoRecApp = WareHouseIoRecApp;
            _mapper = mapper;
        }

        /// <summary>
        /// 出入库管页面，左边的地点-仓库-库存树 地点-仓库-库存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> LocationWareHouseTree()
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<TreeEasy> easies = _WareHouseApp.LocationWareHouseTree();
                pd.Data = easies;
                pd.Total = easies.Count;
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
        /// 得到出入库记录数据
        /// </summary>
        /// <param name="req">
        /// 参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;

                List<warehouse_io_rec> base_Warehouses_rec = _pikachuApp.GetList<warehouse_io_rec>(req, ref lcount);
                List<WareHouceRecResponse> temp_baseWareHouseRecResponses = new();
                foreach (warehouse_io_rec temp in base_Warehouses_rec)
                {

                    //新增
                    WareHouceRecResponse temp_baseWareHouseRecResponse = new();
                    base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id);
                    base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(temp.warehouse_loc_id);
                    _mapper.Map(temp, temp_baseWareHouseRecResponse);
                    //temp_baseWareHouseRecResponse.warehouse_code = temp_base_Warehouse.warehouse_code;
                    //temp_baseWareHouseRecResponse.warehouse_name = temp_base_Warehouse.warehouse_name;
                    //temp_baseWareHouseRecResponse.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                    //temp_baseWareHouseRecResponse.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                    temp_baseWareHouseRecResponses.Add(temp_baseWareHouseRecResponse);
                }
                pd.Data = temp_baseWareHouseRecResponses;
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
        /// 获取库存列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetStockList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;

                List<base_stock> stocks = _pikachuApp.GetList<base_stock>(req, ref lcount);

                pd.Data = stocks;
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
        /// 根据地点id或仓库id或者库位id获取库存数据.ID给null的时候，type值的影响将被忽略，函数成为getlist
        /// </summary>
        /// <param name="id">地点id或者仓库id或者库位Id</param>
        /// <param name="type">location   warehouse   warehouse_loc</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetStockBy(string id, string type, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                if (id == null)
                {
                    PageData pd1 = new PageData();
                    List<base_stock> a = _pikachuApp.GetAll<base_stock>();
                    long lcoun = 0;
                    List<StockResponse> temp_StockResponseResponses1 = new();
                    foreach (base_stock temp in a)
                    {
                        StockResponse temp_StockResponseResponse = new();
                        base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id);
                        if (temp_base_Warehouse == null)
                        {
                            throw new Exception($"库存{temp.stock_id}数据中出现无效的仓库{temp.warehouse_id}");
                        }

                        base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(temp.warehouse_loc_id);
                        temp_StockResponseResponse.warehouse_id = temp_base_Warehouse.warehouse_id;
                        temp_StockResponseResponse.warehouse_loc_id = temp_base_Warehouse_loc.warehouse_loc_id;
                        temp_StockResponseResponse.warehouse_code = temp_base_Warehouse.warehouse_code;
                        temp_StockResponseResponse.warehouse_name = temp_base_Warehouse.warehouse_name;
                        temp_StockResponseResponse.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                        temp_StockResponseResponse.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                        temp_StockResponseResponse.material_id = temp.material_id;
                        temp_StockResponseResponse.material_name = temp.material_name;
                        temp_StockResponseResponse.material_code = temp.material_code;
                        temp_StockResponseResponse.qty = temp.qty;
                        temp_StockResponseResponse.create_time = temp.create_time;
                        temp_StockResponseResponse.create_by_name = temp.create_by_name;
                        temp_StockResponseResponse.stock_id = temp.stock_id;
                        temp_StockResponseResponses1.Add(temp_StockResponseResponse);
                    }



                    lcoun = temp_StockResponseResponses1.Count();
                    if (req != null)
                    {
                        int iPage = req.page;
                        int iRows = req.rows;
                        //分页
                        temp_StockResponseResponses1 = temp_StockResponseResponses1.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                    }
                    pd1.Data = temp_StockResponseResponses1;
                    pd1.Total = temp_StockResponseResponses1.Count;
                    result.Result = pd1;
                    return result;
                }

                PageData pd = new PageData();
                long lcount = 0;
                List<base_stock> base_Stocks = new();
                switch (type)
                {
                    case "location":
                        List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(id);
                        List<base_warehouse> base_Warehouses = new();
                        foreach (base_location base_Location in base_Locations)
                        {
                            base_Warehouses = _pikachuApp.GetByOneFeildsSql<base_warehouse>("location_id", base_Location.location_id);
                            foreach (base_warehouse base_Warehouse in base_Warehouses)
                            {
                                List<base_stock> base_stocks = _pikachuApp.GetByOneFeildsSql<base_stock>("warehouse_id", base_Warehouse.warehouse_id);
                                base_Stocks.AddRange(base_stocks);
                            }
                        }

                        break;
                    case "warehouse":
                        base_Stocks = _pikachuApp.GetByOneFeildsSql<base_stock>("warehouse_id", id);
                        break;
                    case "warehouse_loc":
                        base_Stocks = _pikachuApp.GetByOneFeildsSql<base_stock>("warehouse_loc_id", id);
                        break;

                }

                List<StockResponse> temp_StockResponses = new();
                foreach (base_stock temp in base_Stocks)
                {
                    StockResponse temp_StockResponse = new();
                    base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id);
                    base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(temp.warehouse_loc_id);
                    temp_StockResponse.warehouse_id = temp_base_Warehouse.warehouse_id;
                    temp_StockResponse.warehouse_loc_id = temp_base_Warehouse_loc.warehouse_loc_id;
                    temp_StockResponse.warehouse_code = temp_base_Warehouse.warehouse_code;
                    temp_StockResponse.warehouse_name = temp_base_Warehouse.warehouse_name;
                    temp_StockResponse.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                    temp_StockResponse.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                    temp_StockResponse.material_id = temp.material_id;
                    temp_StockResponse.material_name = temp.material_name;
                    temp_StockResponse.material_code = temp.material_code;
                    temp_StockResponse.qty = temp.qty;
                    temp_StockResponse.create_time = temp.create_time;
                    temp_StockResponse.create_by_name = temp.create_by_name;
                    temp_StockResponse.stock_id = temp.stock_id;
                    temp_StockResponses.Add(temp_StockResponse);
                }

                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    temp_StockResponses = temp_StockResponses.OrderByDescending(c => c.create_time).Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = temp_StockResponses;
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
        /// 根据地点id或仓库id获取出入库记录，注意要分页(还没做，用到该函数再做)
        /// </summary>
        /// <param name="id">地点id或者仓库id</param>
        /// <param name="IOopration">出入库操作 0入库 1出库 3都要 默认值3</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetIoWareHouseRec(string id, int IOopration = 3)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;

                List<warehouse_io_rec> base_Warehouses_rec = _WareHouseIoRecApp.GetIoWareHouseRec(id, IOopration);
                List<WareHouceRecResponse> temp_baseWareHouseRecResponses = new();
                foreach (warehouse_io_rec temp in base_Warehouses_rec)
                {
                    WareHouceRecResponse temp_baseWareHouseRecResponse = new();
                    base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id);
                    base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(temp.warehouse_loc_id);
                    _mapper.Map(temp, temp_baseWareHouseRecResponse);
                    temp_baseWareHouseRecResponse.warehouse_code = temp_base_Warehouse.warehouse_code;
                    temp_baseWareHouseRecResponse.warehouse_name = temp_base_Warehouse.warehouse_name;
                    temp_baseWareHouseRecResponse.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                    temp_baseWareHouseRecResponse.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                    temp_baseWareHouseRecResponses.Add(temp_baseWareHouseRecResponse);
                }
                pd.Data = temp_baseWareHouseRecResponses;
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
        /// 入库操作时对库位数量进行检验
        /// </summary>
        /// <param name="WareHouseRecs"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> CheckIoWareHouseOperation(List<warehouse_io_rec> WareHouseRecs)
        {
            Response<bool> result = new Response<bool>();
            try
            {
                bool re = _WareHouseIoRecApp.CheckIoWareHouseOperation(WareHouseRecs);
                result.Result = re;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 出入库操作,可以多条记录同时出入库
        /// </summary>
        /// <param name="WareHouseRecs">仓库id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> IoWareHouseOperation(List<warehouse_io_rec> WareHouseRecs)
        {
            Response<string> result = new Response<string>();
            try
            {
                _WareHouseIoRecApp.IoWareHouseOperation(WareHouseRecs);
                result.Result = "操作成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 调拨操作
        /// </summary>
        /// <param name="MoveRequests"></param>
        /// <param name="rec_id">选中的记录id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> MoveOperation(List<MoveRequest> MoveRequests, string rec_id)
        {
            Response<string> result = new Response<string>();
            try
            {
                _WareHouseIoRecApp.Move(MoveRequests, rec_id);
                result.Result = "操作成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 盘点操作
        /// </summary>
        /// <param name="stock_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CheckOperation(string stock_id, int num)
        {
            Response<string> result = new Response<string>();
            try
            {
                _WareHouseIoRecApp.CheckOperation(stock_id, num);
                result.Result = "操作成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据库位id返回库位的safa_num和max_num
        /// List<int>
        /// 第一个数是safa
        /// 第二个是max
        /// </summary>
        /// <param name="warehouse_loc_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<int>> GetNumOfWareLoc(string warehouse_loc_id)
        {
            Response<List<int>> result = new Response<List<int>>();
            try
            {
                result.Result = _WareHouseIoRecApp.GetNumOfWareLoc(warehouse_loc_id);
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
