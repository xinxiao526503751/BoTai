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
    public class EquMaintainItemController : ControllerBase
    {
        private readonly IEquMaintainItemApp _equMaintainItemApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equMaintainItemApp"></param>
        public EquMaintainItemController(IEquMaintainItemApp equMaintainItemApp)
        {
            this._equMaintainItemApp = equMaintainItemApp;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equMaintainItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Create(EquMaintainItem equMaintainItem)
        {
            if (equMaintainItem == null)
            {
                return ApiHelpers.Error("抱歉！新增数据为空");
            }
            var getByTwoId = await _equMaintainItemApp.FindByTwoId(equMaintainItem.EQU_ID, equMaintainItem.MAINTAIN_ITEM_ID);
            //var getById = await _equMaintainItemApp.FindById(equMaintainItem.EQU_ID);
            //var getByName = await _equMaintainItemApp.FindByName(equMaintainItem.MAINTAIN_ITEM_NAME);
            if (getByTwoId == null)
            {
                equMaintainItem.CREATED_TIME = DateTime.Now;
                equMaintainItem.UPDATE_TIME = null;
                equMaintainItem.UPDATE_BY = null;

                bool b = await _equMaintainItemApp.CreateAsync(equMaintainItem);
                if (!b)
                {
                    return ApiHelpers.Error("添加失败");
                }
                return ApiHelpers.Success(equMaintainItem);
            }
            else
            {
                return ApiHelpers.Error("抱歉！无法添加重复数据");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _equMaintainItemApp.QueryAsync();
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 根据EquId查找
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> GetByEquId(QueryModel queryModel)
        {
            if (string.IsNullOrWhiteSpace(queryModel.QId))
            {
                return ApiHelpers.Error("抱歉，id不能为空");
            }
            var data = await _equMaintainItemApp.QueryByIdAsync(queryModel.QId);
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
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteByTwoId(string id1,string id2)
        {
            if(string.IsNullOrWhiteSpace(id1)||string.IsNullOrWhiteSpace(id2))
            {
                return ApiHelpers.Error("抱歉！id1或id2为空，请重新输入");
            }
            bool b = await _equMaintainItemApp.DeleteByTwoId(id1, id2);
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
        public async Task<ApiResult> QueryPageList(int pageIndex,int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
            {
                return ApiHelpers.Error("抱歉！pageIndex或pageSize小于0，请重新输入");
            }

            var data = await _equMaintainItemApp.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                return ApiHelpers.Error("未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _equMaintainItemApp.GetTotalNum();
                returnData.Total = num;
                return returnData;
            }
        }
        /// <summary>
        /// 模糊分页查询
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> QueryAndPage(QueryModel queryModel)
        {
            var data = await _equMaintainItemApp.QueryAsync(queryModel.Q1, queryModel.Q2);
            if (data == null)
            {
                return ApiHelpers.Error("数据为空");
            }
            if (queryModel.flag == 0)
            {
                //进行数据分页
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
        /// 更新
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="equMaintainItem"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateWithTwoId(string id1, string id2, EquMaintainItem equMaintainItem)
        {
            if (string.IsNullOrWhiteSpace(id1) || string.IsNullOrWhiteSpace(id2))
            {
                return ApiHelpers.Error("抱歉，id1或id2为空，请重新输入");
            }

            var queryData = await _equMaintainItemApp.FindByTwoId(id1, id2);
            if (queryData == null)
            {
                return ApiHelpers.Error("抱歉，无此更新数据，请先添加");
            }

            equMaintainItem.CREATED_TIME = queryData.CREATED_TIME;
            equMaintainItem.UPDATE_TIME = DateTime.Now;
            bool b = await _equMaintainItemApp.UpdateWithTwoId(id1,id2,equMaintainItem);
            if (!b)
            {
                return ApiHelpers.Error("更新失败");
            }
            return ApiHelpers.Success(b);
        }
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> FilterAndPage(QueryModel queryModel)
        {
            var data = await _equMaintainItemApp.QueryAsync();
            if (data == null)
            {
                return ApiHelpers.Error("抱歉，数据为空");
            }
            //进行数据过滤
            var filterData = data.GroupBy(x => x.EQU_ID).Select(g=>g.First());
            //在进行数据分页
            var pageData=filterData.Skip(queryModel.pageSize * (queryModel.pageIndex - 1)).Take(queryModel.pageSize);

            var returnData = ApiHelpers.Success(pageData);
            returnData.Total = filterData.Count();
            return returnData;
        }
    }
}
