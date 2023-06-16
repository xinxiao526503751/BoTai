using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Sys
{
    /// <summary>
    /// 单位类型控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Sys", IgnoreApi = false)]
    [ApiController]
    public class SysParmTypeController
    {
        private readonly SysParmTypeApp _app;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// 单位类型控制器
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pikachuApp"></param>
        public SysParmTypeController(SysParmTypeApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }

        /// <summary>
        /// 新增单位类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>warehouse_type_id</returns>
        [HttpPost]
        public Response<string> Insert(sys_parm_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_app.Insert(obj))
                {
                    result.Result = obj.parm_type_id;
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
        /// 删除单位类型
        /// </summary>
        /// <param name="ids">单位类型id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids, bool check_flag)
        {
            Response<string[]> result = new Response<string[]>();
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
                                result.Code = 200;
                                result.Message = $"{chars}正在引用{name}";
                                return result;
                            }
                        }
                    }
                }

                else
                {
                    _app.DeleteParmType(ids);

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
        /// 更新单位类型
        /// </summary>
        /// <param name="obj">单位类型对象</param>
        /// <returns></returns>

        [HttpPost]
        public Response<string> Update(sys_parm_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                sys_parm_type base_Warehouse_Type = _pikachuApp.GetById<sys_parm_type>(obj.parm_type_id);

                if (base_Warehouse_Type != null)
                {
                    obj.create_time = base_Warehouse_Type.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.parm_type_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                result.Result = obj.parm_type_id;

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
        /// 得到单位类型列表
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
                pd.Data = _pikachuApp.GetList<sys_parm_type>(req, ref lcount);
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
        /// 获取参数类型树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SysParmTypeTree()
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<TreeEasy> easies = _app.SysParmTree();
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

    }
}
