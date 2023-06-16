using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Equipment.Upkeep;
using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.WebApi.Helpers.ApiResultHelper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hhmocon.Mes.WebApi.Controllers.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquUpkeep", IgnoreApi = false)]
    public class UpkeepPlanController : ControllerBase
    {
        private readonly IUpkeepPlanApp _upkeepPlanApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upkeepPlanApp"></param>
        public UpkeepPlanController(IUpkeepPlanApp upkeepPlanApp)
        {
            this._upkeepPlanApp = upkeepPlanApp;
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="upkeepPlan"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateUpkeepPlan(UpkeepPlan upkeepPlan)
        {
            upkeepPlan.PLAN_ID = CommonHelper.GetPlanId();

            var getById = await _upkeepPlanApp.FindById(upkeepPlan.PLAN_ID);
            if (getById != null)
            {
                return ApiHelpers.Error($"计划编号为{upkeepPlan.PLAN_ID}已存在，请重新命名");
            }
            bool b = await _upkeepPlanApp.CreateAsync(upkeepPlan);
            if (!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(upkeepPlan);
        }
        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id不能为空，请重新输入");
            }
            var data = await _upkeepPlanApp.FindById(id);
            if (data == null)
            {
                return ApiHelpers.Error($"{id}对应的数据不存在，请注意{id}的正确性");
            }
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            bool b = await _upkeepPlanApp.DeleteAsync(id);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteByIds(string[] ids)
        {
            if (ids == null)
            {
                return ApiHelpers.Error("抱歉，ids为空，请重新输入");
            }
            bool b = await _upkeepPlanApp.DeleteAsync(ids);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _upkeepPlanApp.QueryAsync();
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetPageList(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
            {
                return ApiHelpers.Error("pageIndex或pageSize小于0，请重新输入");
            }
            var data = await _upkeepPlanApp.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("抱歉，未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _upkeepPlanApp.GetTotalNum();
                returnData.Total = num;
                return returnData;
            }
        }
        /// <summary>
        /// 模糊分页
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> QueryAndPage(QueryModel queryModel)
        {
            var data = await _upkeepPlanApp.QueryAsync(queryModel.Q1, queryModel.Q2);
            var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            var returnData = ApiHelpers.Success(pageData);
            returnData.Total = data.Count();
            return returnData;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="upkeepPlan"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateUpkeepPlan(string id,UpkeepPlan upkeepPlan)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id不能为空");
            }
            var dataQuery = await _upkeepPlanApp.FindById(id);
            if (dataQuery == null)
            {
                return ApiHelpers.Error("无此保养计划，请确定保养计划编码的正确性");
            }
            upkeepPlan.PLAN_ID = dataQuery.PLAN_ID;
            upkeepPlan.CREATED_TIME = dataQuery.CREATED_TIME;
            upkeepPlan.CREATED_BY = dataQuery.CREATED_BY;
            upkeepPlan.UPDATE_TIME = DateTime.Now;
            bool b = await _upkeepPlanApp.UpdateAsync(id,upkeepPlan);
            if (!b)
            {
                return ApiHelpers.Error("更新失败");
            }
            return ApiHelpers.Success(upkeepPlan);
        }
    }
}
