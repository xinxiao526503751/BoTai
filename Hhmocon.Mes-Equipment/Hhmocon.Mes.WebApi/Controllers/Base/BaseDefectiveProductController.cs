using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
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
    /// <summary>
    /// 产品缺陷定义控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Defective", IgnoreApi = false)]
    public class BaseDefectiveProductController : ControllerBase
    {
        private BaseDefectiveProductApp _app;
        private PikachuApp _pikachuApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public BaseDefectiveProductController(BaseDefectiveProductApp app, PikachuApp picachuApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
        }

        [HttpPost]
        public Response<string> Create(base_defective_product obj)
        {
            var result = new Response<string>();
            try
            {
                base_defective_reason base_Defective_Reason = _pikachuApp.GetById <base_defective_reason>(obj.defective_reason_id);
                if (base_Defective_Reason == null)
                {
                    throw new Exception("不存在的不合格原因");
                }
                obj.defective_type_id = base_Defective_Reason.defective_type_id;
                base_defective_product data = _app.Insert(obj);
                if (data != null)
                {
                    result.Result = data.defective_type_id;
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
        /// 更新信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_defective_product obj)
        {
            var result = new Response<string>();
            try
            {
                base_defective_product _Product = _pikachuApp.GetById<base_defective_product>(obj.defective_product_id);
                //如果能够根据id找
                if (_Product != null)
                { 
                    obj.create_time = _Product.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                    obj.defective_type_id = _pikachuApp.GetById<base_defective_reason>(obj.defective_reason_id).defective_type_id;
                }
                else
                { //找不到顾客要返回错误信息
                    result.Result = obj.defective_product_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.defective_product_id;

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


        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            var result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<base_defective_product>(ids))
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
        /// 得到产品-缺陷关联列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req, string typeId)
        {
            var result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_defective_product>(req, ref lcount);
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
        ///  根据ID得到用户明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_defective_product> GetDetail(string id)
        {
            var result = new Response<base_defective_product>();
            try
            {
                result.Result = _pikachuApp.GetById<base_defective_product>(id);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据不合格类型获取记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetProductsByType(string typeId, PageReq req)
        {
            var result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                List<base_defective_product> data = new();
                long lcount = 0;
                if(typeId!=null)
                    data = _pikachuApp.GetAll<base_defective_product>()
                        .Where(a => a.defective_type_id == typeId).ToList();
                else
                    data = _pikachuApp.GetAll<base_defective_product>();
                pd.Data =data;
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    pd.Data = data?.Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Total = data != null ? data.Count : 0;
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
        /// defectivestatistic不合格产品统计页面的环状图和右边的直线图
        /// 查询日期范围(year-mouth ~ year-mouth)内不和格原因的总数
        /// </summary>
        /// <param name="StartTime">years-mouth</param>
        /// <param name="EndTime">years-mouth</param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<GetProductsNumberByTimeDonotChartResponse>> GetProductsNumberByTimeDonotChart(string StartTime,string EndTime)
        {
            var result = new Response<List<GetProductsNumberByTimeDonotChartResponse>>();
            try
            {
                List<GetProductsNumberByTimeDonotChartResponse> data = new();

                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
                {
                    throw new Exception("请选择时间");
                }

                DateTime Start = (StartTime + "-01 00:00:00").ToDate();
                DateTime End = (EndTime + "-28 23:59:59").ToDate();

              

               string[] SplitArray = EndTime.Split('-');//由于结束的月份天数不好确定，需要判断

                //大月
                string[] BigMouth = { "01", "03", "05", "10", "12" };
                //小月
                string[] SmallMouth = { "04", "06", "07", "08", "09", "11" };
                //平月
                string[] nonleapMouth = { "02" };

                //如果月份是大月
                if (BigMouth.Contains(SplitArray[1]))
                {
                    End = (EndTime + "-31 23:59:59").ToDate();
                }
                else if (SmallMouth.Contains(SplitArray[1]))
                {
                    End = (EndTime + "-30 23:59:59").ToDate();
                }
                else if (nonleapMouth.Contains(SplitArray[1]))
                {
                    //闰年
                    if (SplitArray[0].ToInt() % 400 == 0)//能被400整除是闰年
                    {
                        End = (EndTime + "-29 23:59:59").ToDate();
                    }
                    //能被4整除不能被100整除的是平年
                    else if (SplitArray[0].ToInt() % 4 == 0 && SplitArray[0].ToInt() % 100 != 0)
                    {
                        End = (EndTime + "-28 23:59:59").ToDate();
                    }
                }
                
                data = _app.GetProductsNumberByTimeDonotChartApp(Start,End);
                result.Result = data;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 时间范围内按照月份统计不同不合格类及数据
        /// </summary>
        /// <param name="StartTime">years-mouth</param>
        /// <param name="EndTime">years-mouth</param>
        /// <returns></returns>
        [HttpPost]
        public Response<UnqulifiedStaticsByTime> UnqualifiedStatisticsByTime(string StartTime, string EndTime)
        {
            var result = new Response<UnqulifiedStaticsByTime>();
            try
            {
                if (string.IsNullOrEmpty(StartTime)||string.IsNullOrEmpty(EndTime))
                {
                    throw new Exception("请选择开始时间和结束时间");
                }

                UnqulifiedStaticsByTime data = new();
             
                data = _app.UnqualifiedStatisticsByTimeApp(StartTime, EndTime);
                result.Result = data;
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
