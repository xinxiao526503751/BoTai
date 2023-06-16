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
    /// <summary>
    /// 点检项目和保养项目用相同的控制器，相同的方法
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class BaseExamitemController : ControllerBase
    {
        private readonly BaseExamitemApp _app;
        private readonly PikachuApp _pikachuApp;
        public BaseExamitemController(BaseExamitemApp app, PikachuApp pikachuApp)
        {
            _app = app;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 得到点检项目分页列表数据/维修/保养
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
                pd.Data = _pikachuApp.GetList<base_examitem_type>(req, ref lcount);
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
        /// 更新点检项目/保养项目信息/维修项目信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_examitem obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                result.Result = obj.examitem_id;
                _app.Update(obj);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }


        /// <summary>
        /// 根据ID删除点检项目/维修/保养，可批量删除（假删除）
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

                if (!_pikachuApp.DeleteMask<base_examitem>(ids))
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
        /// 新建点检项目信息.method_type="1"代表生成点检项目，method="2"代表生成保养项目 "3"维修
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_examitem obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                List<base_examitem> getbycodes = _pikachuApp.GetByOneFeildsSql<base_examitem>("examitem_code", obj.examitem_code);
                base_examitem getbycode = getbycodes.Where(c => c.method_type == obj.method_type).FirstOrDefault();

                if (getbycode != null)//如果能根据code找到内容
                {
                    result.Code = 500;
                    result.Message =
          string.Format("已存在有该编码={0}的数据，请检查并重新填写", obj.examitem_code);
                    return result;
                }

                //新建时根据_type_id自动生成_type_code和_type_name
                base_examitem_type _base_examitem_type = _pikachuApp.GetByName<base_examitem_type>(obj.examitem_type_name);

                //如果能够根据新建examitem的examitem_type_id找到内容
                if (_base_examitem_type != null)
                {
                    obj.examitem_type_id = _base_examitem_type.examitem_type_id;//锁死code
                    obj.examitem_type_name = _base_examitem_type.examitem_type_name;//锁死name
                }
                else
                {
                    result.Code = 100;
                    result.Message = "新建信息时不能填写不存在的类型name";
                }

                if (string.IsNullOrEmpty(obj.examitem_type_id))
                {
                    throw new Exception("绑定类型时出现未知错误，请尝试重试选择类型");
                }

                base_examitem data = _app.InsertExamitem(obj);
                if (data != null)
                {
                    result.Result = data.examitem_id;
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
        /// 根据ID得到点检项目明细信息 点检/保养/维修
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_examitem> GetDetail(string id)
        {
            Response<base_examitem> result = new Response<base_examitem>();
            try
            {
                result.Result = _pikachuApp.GetById<base_examitem>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据传入的type_id和分页数据找叶
        /// </summary>
        /// <param name="type_id">examitem_type_id</param>
        /// <param name="req">req.key传"1"代表对点检操作
        /// 传"2"代表对保养操作
        /// "3"维修
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetLeafs(string type_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            long lcount = 0;
            List<base_examitem> r = new();
            try
            {
                if (type_id == null)
                {
                    r = _pikachuApp.GetAll<base_examitem>().Where(c => c.method_type == req.key).OrderBy(c => c.examitem_code).ToList();
                    lcount = r.Count();
                }
                else
                {
                    r = _pikachuApp.GetByOneFeildsSql<base_examitem>("examitem_type_id", type_id).Where(c => c.method_type == req.key).OrderBy(c => c.examitem_code).ToList();
                    lcount = r.Count();
                }

                //分页
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;

                    r = r.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }

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
        /// 搜索接口。req.key="1"代表对点检搜索，req.key="2"代表对保养搜索 "3"维修
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
                    pd.Data = _pikachuApp.GetList<base_examitem>(req, ref lcount).Where(a => a.examitem_type_id == type_id).ToList();
                    pd.Total = lcount;
                    result.Result = pd;
                    return result;

                }
                if (req.key != "")
                {
                    pd.Data = _pikachuApp.GetList<base_examitem>(req, ref lcount);
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
