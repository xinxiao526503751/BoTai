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
    public class UpkeepItemController : ControllerBase
    {
        private readonly IUpkeepItemApp _upkeepItemApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upkeepItemApp"></param>
        public UpkeepItemController(IUpkeepItemApp upkeepItemApp)
        {
            this._upkeepItemApp = upkeepItemApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="upkeepItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateUpkeepItem(UpkeepItem upkeepItem)
        {
            //生成UPKEEP_ITEM_ID
            upkeepItem.UPKEEP_ITEM_ID = CommonHelper.GetUpkeepItemCode(upkeepItem.TYPE_ID);

            var getById = await _upkeepItemApp.FindById(upkeepItem.UPKEEP_ITEM_ID);
            var getByName = await _upkeepItemApp.FindByName(upkeepItem.UPKEEP_ITEM_NAME);
            if (getById != null)
            {
                return ApiHelpers.Error($"类型编号为{upkeepItem.UPKEEP_ITEM_ID}已存在，请重新命名");
            }
            if (getByName != null)
            {
                return ApiHelpers.Error($"类型名称为{upkeepItem.UPKEEP_ITEM_NAME}已存在，请重新命名");
            }
            bool b = await _upkeepItemApp.CreateAsync(upkeepItem);
            if (!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(upkeepItem);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _upkeepItemApp.QueryAsync();
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 根据typeId查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> GetByTypeId(QueryModel queryModel)
        {
            if (string.IsNullOrWhiteSpace(queryModel.QId))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            var data = await _upkeepItemApp.QueryByIdAsync(queryModel.QId);
            var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("抱歉，数据为空");
            }
            var returnData = ApiHelpers.Success(pageData);
            returnData.Total = data.Count();
            return returnData;
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
                return ApiHelpers.Error("id数组为空，请重新输入");
            }
            var data = await _upkeepItemApp.FindByIds(ids);
            if (data.Count() != ids.Length)
            {
                return ApiHelpers.Error("存在id未查找到的数据，请注意id的正确性");
            };
            bool b = await _upkeepItemApp.DeleteAsync(ids);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
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
                return ApiHelpers.Error("pageIndex或者pageSize小于0，请重新输入");
            }
            var data = await _upkeepItemApp.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _upkeepItemApp.GetTotalNum();
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
            var data = await _upkeepItemApp.QueryAsync(queryModel.Q1, queryModel.Q2);
            var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            var returnData = ApiHelpers.Success(pageData);
            returnData.Total = data.Count();
            return returnData;
        }
    }
}
