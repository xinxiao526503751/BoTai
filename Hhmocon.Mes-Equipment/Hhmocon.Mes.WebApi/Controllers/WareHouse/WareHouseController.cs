using AutoMapper;
using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Response;
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
    /// 仓库定义控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WareHouse", IgnoreApi = false)]
    public class WareHouseController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly WareHouseApp _WareHouseApp;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="WareHouseApp"></param>
        public WareHouseController(PikachuApp pikachuApp, WareHouseApp WareHouseApp, IMapper mapper)
        {
            _pikachuApp = pikachuApp;
            _WareHouseApp = WareHouseApp;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增仓库定义
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(base_warehouse obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_WareHouseApp.Insert(obj))
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
        /// 删除仓库定义，如果底下有库位或者库存，需要给提示
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
                    throw new Exception("未选中仓库");
                }

                if (check_flag)
                {
                    //查找用到warehouse_id字段的表下是不是有数据（查库位表就可以了）
                    List<string> chartNames = _pikachuApp.GetAllChartNameHavingSameField("warehouse_id");
                    List<string> chartExistsData = new();
                    foreach (string id in ids)
                    {
                        foreach (string chart in chartNames)
                        {
                            _WareHouseApp.CheckChartIfExistsData(ref chartExistsData, id, chart);
                            if (chartExistsData.Count > 0)
                            {
                                string chars = string.Join(",", chartExistsData.ToArray());

                                switch (chars)
                                {
                                    case "base_warehouse_loc":
                                        base_warehouse base_Warehouse = _pikachuApp.GetById<base_warehouse>(id);
                                        if (base_Warehouse != null)
                                        {
                                            string name = base_Warehouse.warehouse_name;
                                            throw new Exception($"仓库{name}存在库位");
                                        }
                                        break;

                                }

                            }
                        }
                    }

                    _WareHouseApp.deleteWareHouse(ids);
                }
                else
                {
                    _WareHouseApp.deleteWareHouse(ids);
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
        /// 更新仓库定义
        /// </summary>
        /// <param name="obj">仓库定义对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_warehouse obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_warehouse base_Warehouse = _pikachuApp.GetById<base_warehouse>(obj.warehouse_id);

                if (base_Warehouse != null)
                {
                    obj.warehouse_code = base_Warehouse.warehouse_code;//锁死code
                    obj.create_time = base_Warehouse.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.warehouse_type_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.warehouse_id;

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
        /// 仓库定义列表数据
        /// </summary>
        /// <param name="req">
        /// 参数</param>
        /// <returns>BaseWareHouseResponse</returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                List<base_warehouse> base_Warehouses = _pikachuApp.GetList<base_warehouse>(req, ref lcount);
                List<BaseWareHouseResponse> baseWareHouseResponses = new();
                foreach (base_warehouse temp in base_Warehouses)
                {
                    BaseWareHouseResponse temp_baseWareHouseResponse = new();
                    _mapper.Map(temp, temp_baseWareHouseResponse);
                    base_location base_Location = _pikachuApp.GetById<base_location>(temp.location_id);
                    if (base_Location == null)
                    {
                        throw new Exception($"地点id：{temp.location_id}找不到对应数据，检查数据库");
                    }
                    temp_baseWareHouseResponse.location_name = base_Location.location_name;
                    base_warehouse_type base_Warehouse_Type = _pikachuApp.GetById<base_warehouse_type>(temp.warehouse_type_id);
                    if (base_Warehouse_Type == null)
                    {
                        throw new Exception("出现无效的仓库类型，请检查输入或刷新页面重试");
                    }
                    temp_baseWareHouseResponse.warehouse_type_name = base_Warehouse_Type.warehouse_type_name;
                    baseWareHouseResponses.Add(temp_baseWareHouseResponse);
                }
                pd.Data = baseWareHouseResponses;
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
        /// 地点树,null获取所有地点树，id获取id为根的地点树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> LocationTree(string location_id = null)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<TreeEasy> easies = _WareHouseApp.LocationTree(location_id);
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
        /// 根据传入的地点Id获取地点下的仓库,传Null时获取所有仓库
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetWareHouseByLocationId(string location_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<base_warehouse> easies = _WareHouseApp.GetWareHouseByLocationId(location_id);
                List<BaseWareHouseResponse> baseWareHouseResponses = new();
                foreach (base_warehouse temp in easies)
                {
                    BaseWareHouseResponse temp_baseWareHouseResponse = new();
                    _mapper.Map(temp, temp_baseWareHouseResponse);
                    temp_baseWareHouseResponse.location_name = _pikachuApp.GetById<base_location>(temp.location_id).location_name;
                    temp_baseWareHouseResponse.warehouse_type_name = _pikachuApp.GetById<base_warehouse_type>(temp.warehouse_type_id).warehouse_type_name;
                    baseWareHouseResponses.Add(temp_baseWareHouseResponse);
                }

                //分页
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    pd.Data = baseWareHouseResponses.Skip((iPage - 1) * iRows).Take(iRows);
                }

                pd.Total = baseWareHouseResponses.Count;
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
        /// 仓库定义页面的搜索框，地点-仓库定义
        /// </summary>
        /// <param name="req"></param>
        /// <param name="location_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(PageReq req, string location_id)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();

                List<base_warehouse> base_Warehouses = _WareHouseApp.SearchBar(req, location_id);
                List<BaseWareHouseResponse> baseWareHouseResponses = new();
                foreach (base_warehouse temp in base_Warehouses)
                {
                    BaseWareHouseResponse temp_baseWareHouseResponse = new();
                    _mapper.Map(temp, temp_baseWareHouseResponse);
                    temp_baseWareHouseResponse.location_name = _pikachuApp.GetById<base_location>(temp.location_id).location_name;
                    temp_baseWareHouseResponse.warehouse_type_name = _pikachuApp.GetById<base_warehouse_type>(temp.warehouse_type_id).warehouse_type_name;
                    baseWareHouseResponses.Add(temp_baseWareHouseResponse);
                }
                pd.Data = baseWareHouseResponses;
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
