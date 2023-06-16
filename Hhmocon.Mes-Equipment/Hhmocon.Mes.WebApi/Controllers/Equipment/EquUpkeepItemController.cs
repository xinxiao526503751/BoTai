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
    public class EquUpkeepItemController : ControllerBase
    {
        private readonly IEquUpkeepItemApp _equUpkeepItemApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equUpkeepItemApp"></param>
        public EquUpkeepItemController(IEquUpkeepItemApp equUpkeepItemApp)
        {
            this._equUpkeepItemApp = equUpkeepItemApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equUpkeepItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateEquUpkeepItem(EquUpkeepItem equUpkeepItem)
        {
            if (equUpkeepItem == null)
            {
                return ApiHelpers.Error("新增数据为空");
            }
            if (string.IsNullOrWhiteSpace(equUpkeepItem.PLAN_ID))
            {
                equUpkeepItem.PLAN_ID = CommonHelper.GetPlanId();
            }
            var getById = await _equUpkeepItemApp.FindById(equUpkeepItem.PLAN_ID);

            if (getById != null)
            {
                return ApiHelpers.Error("抱歉，无法添加重复计划点检项目");
            }
            equUpkeepItem.CREATED_TIME = DateTime.Now;
            equUpkeepItem.UPDATE_TIME = null;
            equUpkeepItem.UPDATE_BY = null;
            bool b = await _equUpkeepItemApp.CreateAsync(equUpkeepItem);
            if (!b)
            {
                return ApiHelpers.Error("新增失败");
            }
            return ApiHelpers.Success(b);
        }

        /// <summary>
        /// 根据设备Id查找
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> GetByEquId(QueryModel queryModel)
        {
            if (string.IsNullOrWhiteSpace(queryModel.QId))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            var data = await _equUpkeepItemApp.QueryByIdAsync(queryModel.QId);
            if (data == null)
            {
                return ApiHelpers.Error("抱歉，数据为空");
            }
            var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
            var returnData = ApiHelpers.Success(pageData);
            returnData.Total = data.Count();
            return returnData;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetEquUpkeepItemPage(int pageindex, int pagesize)
        {
            if (pageindex < 0 || pagesize < 0)
            {
                return ApiHelpers.Error("pageindex或pagesize小于0，请重新输入");
            }
            var data = await _equUpkeepItemApp.QueryPageAsync(pageindex, pagesize);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }

            var returnData = ApiHelpers.ReturnCountAndSuccess(data);
            int num = await _equUpkeepItemApp.GetTotalNum();
            returnData.Total = num;
            return returnData;
        }
        /// <summary>
        /// 模糊分页查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> QueryAndPage(QueryModel queryModel)
        {
            var data = await _equUpkeepItemApp.QueryAsync(queryModel.Q1, queryModel.Q2);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            if (queryModel.flag == 0)
            {
                var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
                var returnData = ApiHelpers.Success(pageData);
                returnData.Total = data.Count();
                return returnData;
            }
            else
            {
                //进行数据过滤
                var filterData = data.GroupBy(x => x.EQU_ID).Select(g => g.First());
                //进行数据分页
                var pageData = filterData.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
                var returnData = ApiHelpers.Success(pageData);
                returnData.Total = filterData.Count();
                return returnData;
            }
        }
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteEquUpkeepItemByIds(string[] ids)
        {
            if (ids == null)
            {
                return ApiHelpers.Error("抱歉，ids数组为空，无法进行删除");
            }
            bool b = await _equUpkeepItemApp.DeleteAsync(ids);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equUpkeepItem"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateEquUpkeepItem(string id, EquUpkeepItem equUpkeepItem)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("请输入需要进行更新的点检计划编码");
            }
            var dataQuery = await _equUpkeepItemApp.FindById(id);
            if (dataQuery == null)
            {
                return ApiHelpers.Error("未查找到该id相对应的数据，请检查id是否正确");
            }
            equUpkeepItem.PLAN_ID = dataQuery.PLAN_ID;
            equUpkeepItem.CREATED_TIME = dataQuery.CREATED_TIME;//锁死创建时间
            equUpkeepItem.UPDATE_TIME = DateTime.Now;//给定修改时间
            bool b = await _equUpkeepItemApp.UpdateAsync(id, equUpkeepItem);
            if (!b)
            {
                return ApiHelpers.Error("更新失败");
            }
            else
            {
                return ApiHelpers.Success(b);
            }
        }
        /// <summary>
        /// 根据planId查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetByPlanId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            var data = await _equUpkeepItemApp.FindById(id);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data =await _equUpkeepItemApp.QueryAsync();
            if (data == null)
            {
                return ApiHelpers.Error("数据未空");
            }
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
    }
}
