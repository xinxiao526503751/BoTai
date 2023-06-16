using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Equipment.Check;
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
    /// 点检计划控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquCheck", IgnoreApi = false)]
    public class CheckPlanController : ControllerBase
    {
        private readonly ICheckPlanApp _checkPlanApp;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="checkPlanApp"></param>
        public CheckPlanController(ICheckPlanApp checkPlanApp)
        {
            this._checkPlanApp = checkPlanApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="checkPlan"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateCheckPlan(CheckPlan checkPlan)
        {
            checkPlan.PLAN_ID = CommonHelper.GetPlanId();

            var getById = await _checkPlanApp.FindById(checkPlan.PLAN_ID);
            if (getById != null)
            {
                return ApiHelpers.Error($"计划编号为{checkPlan.PLAN_ID}已存在，请重新命名");
            }
            bool b = await _checkPlanApp.CreateAsync(checkPlan);
            if(!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(checkPlan);
        }
        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetById(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id不能为空，请重新输入");
            }
            var data = await _checkPlanApp.FindById(id);
            if(data==null)
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
            if(string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            bool b = await _checkPlanApp.DeleteAsync(id);
            if(!b)
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
            if(ids==null)
            {
                return ApiHelpers.Error("抱歉，ids为空，请重新输入");
            }
            bool b = await _checkPlanApp.DeleteAsync(ids);
            if(!b)
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
            var data = await _checkPlanApp.QueryAsync();
            if(data==null)
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
        public async Task<ApiResult> GetPageList(int pageIndex,int pageSize)
        {
            if(pageIndex<0||pageSize<0)
            {
                return ApiHelpers.Error("pageIndex或pageSize小于0，请重新输入");
            }
            var data = await _checkPlanApp.QueryPageAsync(pageIndex,pageSize);
            if(data==null)
            {
                return ApiHelpers.Error("抱歉，未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _checkPlanApp.GetTotalNum();
                returnData.Total = num;
                return returnData;
            }
        }
        /// <summary>
        /// 模糊查询分页
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> QueryAndPage(QueryModel queryModel)
        {
            var data = await _checkPlanApp.QueryAsync(queryModel.Q1, queryModel.Q2);
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
        /// <param name="checkPlan"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateCheckPlan(string id,CheckPlan checkPlan)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("请输入需要进行更新的点检计划编码");
            }
            var dataQuery = await _checkPlanApp.FindById(id);
            if(dataQuery==null)
            {
                return ApiHelpers.Error("无此点检计划，请确定点检计划编码的正确性");
            }
            checkPlan.PLAN_ID = dataQuery.PLAN_ID;
            checkPlan.CREATED_TIME = dataQuery.CREATED_TIME;
            checkPlan.CREATED_BY = dataQuery.CREATED_BY;
            checkPlan.UPDATE_TIME = DateTime.Now;

            bool b = await _checkPlanApp.UpdateAsync(id,checkPlan);
            if(!b)
            {
                return ApiHelpers.Error("更新失败");
            }
            return ApiHelpers.Success(checkPlan);
        }
    }
}
