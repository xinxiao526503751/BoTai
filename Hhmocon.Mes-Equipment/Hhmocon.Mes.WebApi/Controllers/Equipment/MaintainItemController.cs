using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Equipment.Maintain;
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
    [ApiExplorerSettings(GroupName = "EquMaintenance", IgnoreApi = false)]

    public class MaintainItemController : ControllerBase
    {
        private readonly IMaintainItemApp _maintainItemApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintainItemApp"></param>
        public MaintainItemController(IMaintainItemApp maintainItemApp)
        {
            this._maintainItemApp = maintainItemApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="maintainItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Create(MaintainItem maintainItem)
        {
            if (maintainItem == null)
            {
                return ApiHelpers.Error("抱歉，添加数据为空");
            }
            maintainItem.MAINTAIN_ITEM_ID = CommonHelper.GetMaintainItemCode(maintainItem.TYPE_ID);

            var getByName = await _maintainItemApp.FindByName(maintainItem.MAINTAIN_ITEM_NAME);
            if(getByName!=null)
            {
                return ApiHelpers.Error($"抱歉，{maintainItem.MAINTAIN_ITEM_NAME}已存在，无法添加重复项");
            }
            maintainItem.CREATED_TIME = DateTime.Now;
            maintainItem.UPDATE_TIME = null;
            maintainItem.UPDATE_BY = null;

            bool b = await _maintainItemApp.CreateAsync(maintainItem);
            if (!b)
            {
                return ApiHelpers.Error("新增失败");
            }
            return ApiHelpers.Success(maintainItem);
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _maintainItemApp.QueryAsync();
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 根据TypeId查找
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> GetByTypeId(QueryModel queryModel)
        {
            if(string.IsNullOrWhiteSpace(queryModel.QId))
            {
                return ApiHelpers.Error("抱歉，id不能为空");
            }
            var data = await _maintainItemApp.QueryByIdAsync(queryModel.QId);
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
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteById(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，id不能为空");
            }
            var data = await _maintainItemApp.FindById(id);
            if(data==null)
            {
                return ApiHelpers.Error("数据为空，请确定id的正确性");
            }
            bool b = await _maintainItemApp.DeleteAsync(id);
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
            if (ids.Length == 0)
            {
                return ApiHelpers.Error("抱歉，id数组为空，请重新输入");
            }
            var data = await _maintainItemApp.FindByIds(ids);
            if (data.Count() != ids.Length)
            {
                return ApiHelpers.Error("存在id未查找到的数据，请注意id的正确性");
            };
            bool b = await _maintainItemApp.DeleteAsync(ids);
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
        public async Task<ApiResult> GetPageList(int pageIndex,int pageSize)
        {
            if(pageIndex<0||pageSize<0)
            {
                return ApiHelpers.Error("pageIndex或pageSize小于0，请重新输入");
            }
            var data = await _maintainItemApp.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _maintainItemApp.GetTotalNum();
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
            var data = await _maintainItemApp.QueryAsync(queryModel.Q1, queryModel.Q2);
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
