using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseBomDetailController : ControllerBase
    {
        private readonly BaseBomDetailApp _app;
        private readonly PikachuApp _pikachuApp;

        public BaseBomDetailController(BaseBomDetailApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;

        }

        /// <summary>
        /// 新建bom
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_bom_detail obj)
        {

            Response<string> result = new Response<string>();
            try
            {
                if (_app.Insert(obj) != null)
                {
                    result.Result = obj.bom_detail_id;
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
        /// 删除bom
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

                if (!_pikachuApp.DeleteMask<base_bom_detail>(ids))
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
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_bom_detail obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_bom_detail _bomDetial = _pikachuApp.GetById<base_bom_detail>(obj.bom_detail_id);
                //如果能够根据id找到
                if (_bomDetial != null)
                {
                    obj.create_time = _bomDetial.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.bom_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.bom_detail_id;

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
        /// 根据bom获取bom明细
        /// </summary>
        /// <param name="bomId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetBomDetailListByBomId(string bomId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                if (_app.GetBomDetailLiistByBomId(bomId, req, ref lcount) != null)
                {
                    pd.Data = _app.GetBomDetailLiistByBomId(bomId, req, ref lcount);
                    pd.Total = lcount;
                }

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
