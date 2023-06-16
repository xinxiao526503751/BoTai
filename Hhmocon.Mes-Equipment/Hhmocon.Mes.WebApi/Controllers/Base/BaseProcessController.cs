using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseProcessController : ControllerBase
    {
        private readonly BaseProcessApp _app;
        private readonly PikachuApp _pikachuApp;
        public BaseProcessController(BaseProcessApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到工序分页列表数据
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
                pd.Data = _pikachuApp.GetList<base_process>(req, ref lcount);
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
        /// 更新工序信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_process obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process _Process = _pikachuApp.GetById<base_process>(obj.process_id);
                if (_Process != null)//如果能够根据id找到内容
                {
                    obj.process_code = _Process.process_code;//锁死code
                    obj.modified_time = DateTime.Now;        //更新修改时间
                    obj.create_time = _Process.create_time;//锁死创建时间
                }
                else
                {
                    result.Result = obj.process_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                }

                result.Result = obj.process_id;

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
        /// 根据ID 删除工序  可批量删除
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

                if (!_pikachuApp.DeleteMask<base_process>(ids))
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
        /// 新建工序信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_process obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_process getbycode = _pikachuApp.GetByCode<base_process>(obj.process_code);
                base_process getbyname = _pikachuApp.GetByName<base_process>(obj.process_name);

                if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("name或Code重复，数据库中已存在有该name={0}或Code={1}的数据，请检查并重新填写", obj.process_name, obj.process_code);
                    return result;
                }

                ////新建时根据_type_id自动生成_type_code和_type_name
                //base_process_type _base_process_type = _pikachuApp.GetById<base_process_type>(obj.process_type_id);

                ////如果能够根据新建process的process_type_id找到内容
                //if (_base_process_type != null)
                //{
                //    obj.process_type_id = _base_process_type.process_type_id;//锁死code
                //    obj.process_type_name = _base_process_type.process_type_name;//锁死name
                //}
                //else
                //{
                //    result.Code = 100;
                //    result.Message = "新建信息时不能填写不存在的类型id";
                //    return result;
                //}

                base_process data = _app.InsertBaseProcess(obj);
                if (data != null)
                {
                    result.Result = data.process_id;
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
        /// 根据ID得到工序明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_process> GetDetail(string id)
        {
            Response<base_process> result = new Response<base_process>();
            try
            {
                result.Result = _pikachuApp.GetById<base_process>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据传入的_type_id和分页数据找叶
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetLeafs(string type_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            long lcount = 0;
            List<base_process> r = new();
            try
            {
                r = _pikachuApp.GetByOneFeildsSql<base_process>("process_type_id", type_id).OrderBy(c => c.create_time).ToList();
                pd.Data = r;
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
        /// 搜索接口
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> SearchLeafs(string type_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            long lcount = 0;
            try
            {
                if (req.key != "" && type_id != null)
                {

                    pd.Data = _pikachuApp.GetList<base_process>(req, ref lcount).Where(a => a.process_type_id == type_id).ToList();
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {

                    pd.Data = _pikachuApp.GetList<base_process>(req, ref lcount);
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
        [HttpPost]
        public Response<List<base_process>> GetAll()
        {
            Response<List<base_process>> result = new Response<List<base_process>>();
            try
            {
                result.Result = _pikachuApp.GetAll<base_process>();
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
