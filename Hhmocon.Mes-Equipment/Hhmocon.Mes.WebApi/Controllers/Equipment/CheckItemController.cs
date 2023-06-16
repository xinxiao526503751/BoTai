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
    /// 点检项目控制层
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquCheck", IgnoreApi = false)]
    public class CheckItemController : ControllerBase
    {
        private readonly ICheckItemApp _checkItemApp;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="checkItemApp"></param>
        public CheckItemController(ICheckItemApp checkItemApp)
        {
            this._checkItemApp = checkItemApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="checkItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateCheckItem(CheckItem checkItem)
        {
            //生成CHECK_ITEM_ID
            checkItem.CHECK_ITEM_ID = CommonHelper.GetCheckItemCode(checkItem.TYPE_ID);

            var getById = await _checkItemApp.FindById(checkItem.CHECK_ITEM_ID);
            var getByName = await _checkItemApp.FindByName(checkItem.CHECK_ITEM_NAME);
            if (getById != null)
            {
                return ApiHelpers.Error($"类型编号为{checkItem.CHECK_ITEM_ID}已存在，请重新命名");
            }
            if (getByName != null)
            {
                return ApiHelpers.Error($"类型名称为{checkItem.CHECK_ITEM_NAME}已存在，请重新命名");
            }
            bool b = await _checkItemApp.CreateAsync(checkItem);
            if(!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(checkItem);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _checkItemApp.QueryAsync();
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 根据QId查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> GetByTypeId(QueryModel queryModel)
        {
            if(string.IsNullOrWhiteSpace(queryModel.QId))
            {
                return ApiHelpers.Error("抱歉，id为空，请重新输入");
            }
            var data = await _checkItemApp.QueryByIdAsync(queryModel.QId);
            var pageData = data.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);
            if (data==null)
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
            if(ids==null)
            {
                return ApiHelpers.Error("id数组为空，请重新输入");
            }
            var data = await _checkItemApp.FindByIds(ids);
            if(data.Count()!=ids.Length)
            {
                return ApiHelpers.Error("存在id未查找到的数据，请注意id的正确性");
            };
            bool b= await _checkItemApp.DeleteAsync(ids);
            if(!b)
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
        public async Task<ApiResult> GetPageList(int pageIndex,int pageSize)
        {
            if(pageIndex<0||pageSize<0)
            {
                return ApiHelpers.Error("pageIndex或者pageSize小于0，请重新输入");
            }
            var data = await _checkItemApp.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _checkItemApp.GetTotalNum();
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
            var data = await _checkItemApp.QueryAsync(queryModel.Q1, queryModel.Q2);
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
