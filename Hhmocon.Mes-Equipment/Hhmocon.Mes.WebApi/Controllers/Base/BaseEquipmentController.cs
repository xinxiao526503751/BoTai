using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Equipment", IgnoreApi = false)]
    public class BaseEquipmentController : ControllerBase
    {
        private readonly BaseEquipmentApp _app;
        private readonly PikachuApp _pikachuApp;
        private readonly ILogger<BaseEquipmentController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        /// <param name="logger"></param>
        public BaseEquipmentController(BaseEquipmentApp app, PikachuApp picachuApp, ILogger<BaseEquipmentController> logger)
        {
            _app = app;
            _pikachuApp = picachuApp;
            _logger = logger;
        }

        /// <summary>
        /// 新建设备信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_equipment obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_equipment getbycode = _pikachuApp.GetByCode<base_equipment>(obj.equipment_code);
                base_equipment getbyname = _pikachuApp.GetByName<base_equipment>(obj.equipment_name);
                //根据location_id补充location_name
                base_location getbylocationid = _pikachuApp.GetById<base_location>(obj.location_id);
                if (getbylocationid != null)
                {
                    obj.location_name = getbylocationid.location_name;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "location_id不存在！";
                    return result;
                }
                //根据equipment_type_id补充equipment_type_name
                base_equipment_type getbyequipmenttypeid = _pikachuApp.GetById<base_equipment_type>(obj.equipment_type_id);
                if (getbyequipmenttypeid != null)
                {
                    obj.equipment_type_name = getbyequipmenttypeid.equipment_type_name;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "equipment_type_id不存在！";
                    return result;
                }

                //根据process_id补充process_name
                base_process getbyprocessid = _pikachuApp.GetById<base_process>(obj.process_id);
                if (getbyprocessid != null)
                {
                    obj.process_name = getbyprocessid.process_name;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "process_id不存在！";
                    return result;
                }

                if (getbycode != null && getbyname != null)//如果能根据name和code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称和编码重复，数据库中已存在有该名称={0},编码={1}的数据，请重新填写", obj.equipment_name, obj.equipment_code);
                    return result;
                }
                else if (getbycode != null)//如果仅能根据code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("编码重复，数据库中已存在有该编码={0}的数据，请重新填写", obj.equipment_code);
                    return result;
                }
                else if (getbyname != null)//如果仅能根据id找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称重复，数据库中已存在有该名称={0}的数据，请重新填写", obj.equipment_name);
                    return result;
                }

                base_equipment data = _app.InsertEquipment(obj);
                if (data != null)
                {
                    result.Result = data.equipment_id;
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
        /// 删除设备同时，删除关联表数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="check_flag">默认true.会对id进行检查，如果有</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids, bool check_flag = true)
        {
            //前端不给且后端不设置的话默认是false
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;
                if (ids.Length == 0 || ids == null)
                {
                    throw new Exception("未选中设备");
                }
                if (check_flag)
                {
                    //查找用到dept_id字段的表下是不是有数据
                    List<string> chartNames = _pikachuApp.GetAllChartNameHavingSameField("equipment_id");
                    List<string> chartExistsData = new();
                    foreach (string id in ids)
                    {
                        foreach (string chart in chartNames)
                        {
                            _app.CheckChartIfExistsData(ref chartExistsData, id, chart);
                            if (chartExistsData.Count > 0)
                            {
                                string name = _pikachuApp.GetById<base_equipment>(id).equipment_name;
                                string charts = string.Join(",", chartExistsData.ToArray());
                                switch (charts)
                                {
                                    //case "base_equipment":
                                    //    throw new Exception($"设备正在引用{name}");
                                    case "base_equipment_sub":
                                        throw new Exception($"设备{name}存在子设备");
                                    case "plan_work":
                                        throw new Exception($"设备{name}存在计划工单引用");
                                    case "plan_work_rpt":
                                        throw new Exception($"设备{name}存在生产报工引用");
                                    case "fault_record":
                                        throw new Exception($"设备{name}存在事件记录引用");
                                    case "exam_equipment_item":
                                        throw new Exception($"设备{name}存在设备点检项引用");
                                    case "exam_plan_method_equipment":
                                        throw new Exception($"设备{name}存在点检计划引用");

                                    case "exam_plan_method_item":
                                        throw new Exception($"设备{name}存在点检计划-点检项目关联引用");
                                    case "exam_plan_rec":
                                        throw new Exception($"设备{name}存在设备点检保养计划关联引用");

                                }
                            }
                        }
                    }

                    _app.Delete(ids);
                }

                else
                {
                    _app.Delete(ids);
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex.Message);
            }

            return result;
        }


        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_equipment obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                result.Result = obj.equipment_id;

                if (!_app.Update(obj))
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
        /// 设备定义页面的搜索框
        /// </summary>
        /// <param name="equipmentSearchBarRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchBar(EquipmentSearchBarRequest equipmentSearchBarRequest)
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



        /// <summary>
        /// 获取设备分页数据
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
                pd.Data = _pikachuApp.GetList<base_equipment>(req, ref lcount);
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
        /// 获取设备不分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetAll()
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData
                {
                    Data = _pikachuApp.GetAll<base_equipment>()
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
        /// 根据id得到设备明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_equipment> GetDetail(string id)
        {
            Response<base_equipment> result = new Response<base_equipment>();
            try
            {
                result.Result = _pikachuApp.GetById<base_equipment>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        ///  根据地点id获取地点下挂载的设备
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetEquipmentByLocationId(string location_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            int count = 0;
            List<base_equipment> equipments = new();
            try
            {
                //根据传入的location_id找到location树
                List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(location_id);
                //遍历location树，根据location_id找到节点下的设备
                foreach (base_location temp in base_Locations)
                {
                    List<base_equipment> equipment_temp = _app.GetByLocationId(temp.location_id).OrderBy(a => a.equipment_name).ToList();
                    if (equipment_temp != null)
                    {
                        //将找到的设备添进列表
                        equipments.AddRange(equipment_temp);
                    }
                }

                count = equipments.Count;

                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页

                    equipments = equipments.Skip((iPage - 1) * iRows).Take(iRows).OrderBy(c => c.equipment_code).ToList();

                }
                pd.Data = equipments;
                pd.Total = count;
                result.Result = pd;
                return result;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
                return result;
            }
        }

        [HttpPost]
        public Response<PageData> SearchLeafs(string location_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            long lcount = 0;
            List<base_equipment> base_Equipment = new();
            try
            {
                if (req.key != "" && location_id != null)
                {
                    List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(location_id);
                    foreach (base_location item in base_Locations)
                    {
                        base_Equipment.AddRange(_pikachuApp.GetList<base_equipment>(req, ref lcount).Where(a => a.location_id == item.location_id).ToList());
                    }
                    pd.Data = base_Equipment;
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {
                    pd.Data = _pikachuApp.GetList<base_equipment>(req, ref lcount);
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
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
