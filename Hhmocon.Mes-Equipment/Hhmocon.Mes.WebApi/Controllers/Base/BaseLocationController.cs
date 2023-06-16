using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseLocationController : ControllerBase
    {
        private BaseLocationApp _app;
        private PikachuApp _pikachuApp;
       
        public BaseLocationController(BaseLocationApp app,PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }

        /// <summary>
        /// 得到地点类型分页列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            var result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_location>(req, ref lcount);
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
        /// 更新地点类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_location obj)
        {
            var result = new Response<string>();
            try
            {
                base_location _Location = _pikachuApp.GetById<base_location>(obj.location_id);
                if (_Location != null)//如果能够根据id找到内容
                {
                    obj.location_code = _Location.location_code;//锁死code
                    obj.modified_time = Time.Now;//更新修改时间
                    obj.create_time = _Location.create_time;//锁死创建时间
                }
                else {
                    result.Result = obj.location_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.location_id;

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
        /// 根据地点ID 删除所有关联地点  可批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="check_flag">检查标志位</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids,bool check_flag)
        {
            var result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (check_flag)
                {
                    //查找用到dept_id字段的表下是不是有数据
                    List<string> chartNames = _pikachuApp.GetAllChartNameHavingSameField("dept_id");
                    List<string> chartExistsData = new();
                    foreach (string id in ids)
                    {
                        foreach (string chart in chartNames)
                        {
                            _app.CheckChartIfExistsData(ref chartExistsData, id, chart);
                            if (chartExistsData.Count > 0)
                            {
                                string chars = string.Join(",", chartExistsData.ToArray());
                                string name = _pikachuApp.GetById<sys_dept>(id).dept_name;
                                switch (chars)
                                {
                                    case "base_equipment":
                                        throw new Exception($"有设备正在引用地点:{name}");
                                    case "sys_dept":
                                        throw new Exception($"有部门正在引用地点:{name}");
                                    case "base_location":
                                        throw new Exception($"{name}下存在子地点");
                                    case "plan_process":
                                        throw new Exception($"计划正在引用{name}");
                                    case "base_warehouse":
                                        throw new Exception($"仓库正在引用{name}");
                                }

                            }
                        }
                    }

                    _app.DeleteLocation(ids);
                }
                else
                {
                    _app.DeleteLocation(ids);
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
        /// 新建地点类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_location obj)
        {
            var result = new Response<string>();
            try
            {
                if (obj.start_time > obj.end_time)
                {
                    throw new Exception("启动时间不允许早于结束时间");
                }

                base_location getbycode = _pikachuApp.GetByCode<base_location>(obj.location_code);
                base_location getbyname = _pikachuApp.GetByName<base_location>(obj.location_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", obj.location_name, obj.location_code);
                    return result;
                }
                //新建时根据_type_id自动生成_type_name
                base_location_type _base_Location_type = _pikachuApp.GetById<base_location_type>(obj.location_type_id);

                //如果能够根据新建location的location_type_id找到内容
                if (_base_Location_type != null)
                {
                    obj.location_type_name = _base_Location_type.location_type_name;//锁死name
                }
                else
                {
                    result.Code = 100;
                    result.Message = "新建物料信息时不能填写不存在的地点类型";
                }

                //2021/7/6记录，Location_type_id修改为允许null
                base_location data = _app.InsertBaseLoction(obj);
                if (data != null)
                {
                    result.Result = data.location_id;
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
        /// 根据ID得到地点类型明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_location> GetDetail(string id)
        {
            var result = new Response<base_location>();
            try
            {
                result.Result = _pikachuApp.GetById<base_location>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 返回表中所有数据不分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_location>> GetAll()
        {
            var result = new Response<List<base_location>>();
            try
            {
                result.Result = _pikachuApp.GetRootAndBranch<base_location>(null).OrderByDescending(c=>c.create_time).ToList();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 搜索按钮接口
        /// </summary>
        /// <param name="root_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchLeafs(string root_id, PageReq req)
        {
            var result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                if (req.key != "" && root_id != null)
                {
                    long lcount = 0;
                    List<base_location> base_Locations1 = new();
                    List<base_location> base_Locations2 = _pikachuApp.GetRootAndBranch<base_location>(root_id);
                    foreach (var item in base_Locations2)
                    {
                        base_Locations1.AddRange(_pikachuApp.GetList<base_location>(req, ref lcount).Where(a => a.location_id == item.location_id).ToList());
                    }
                    
                    pd.Data = base_Locations1; 
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {
                    long lcount = 0;
                    pd.Data = _pikachuApp.GetList<base_location>(req, ref lcount);
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (string.IsNullOrEmpty(root_id))
                {
                    long lcount = 0;
                    pd.Data = _pikachuApp.GetList<base_location>(req, ref lcount);
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;
                }

                List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(root_id);

                //分页
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    pd.Data =  base_Locations.Skip((iPage - 1) * iRows).Take(iRows);
                }
                pd.Total = base_Locations.Count;

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
        /// 根据传入的id获取该id数据
        /// 并根据parentname字段得到子数据
        /// </summary>
        /// <param name="root_id">为null时效果同getlist</param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetLeafs(string root_id, PageReq req)
        {
            var result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                if (string.IsNullOrEmpty(root_id))
                {
                    long lcount = 0;
                    pd.Data = _pikachuApp.GetList<base_location>(req, ref lcount);
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;
                }

                List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(root_id).OrderByDescending(g => g.create_time).ToList();

                //分页
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    pd.Data = base_Locations.Skip((iPage - 1) * iRows).Take(iRows);
                }
                pd.Total = base_Locations.Count;

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
