using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Equipment.EquInfo;
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
    /// 设备信息控制类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquInformation", IgnoreApi = false)]
    public class EquInfoController : ControllerBase
    {
        private readonly IEquInfoApp _equInfoApp;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="equInfoApp"></param>
        public EquInfoController(IEquInfoApp equInfoApp)
        {
            this._equInfoApp = equInfoApp;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _equInfoApp.QueryAsync();
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetTotalNum()
        {
            var num = await _equInfoApp.GetTotalNum();
            return ApiHelpers.Success(num);
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
                return ApiHelpers.Error("id为空，请重新输入");
            }
            var data = await _equInfoApp.FindById(id);
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 根据name查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ApiHelpers.Error("name为空，请重新输入");
            }
            var data = await _equInfoApp.FindByName(name);
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equipmentInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateEquInfo(EquipmentInfo equipmentInfo)
        {
            //生成EQU_ID
            equipmentInfo.EQU_ID = CommonHelper.GetEquCode(equipmentInfo.TYPE_ID);

            var getById = await _equInfoApp.FindById(equipmentInfo.EQU_ID);
            var getByName = await _equInfoApp.FindByName(equipmentInfo.EQU_NAME);
            if (getById != null)
            {
                return ApiHelpers.Error($"类型编号为{equipmentInfo.EQU_ID}已存在，请重新命名");
            }
            if (getByName != null)
            {
                return ApiHelpers.Error($"类型名称为{equipmentInfo.EQU_NAME}已存在，请重新命名");
            }
            bool b = await _equInfoApp.CreateAsync(equipmentInfo);
            if(!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(equipmentInfo);
        }
        /// <summary>
        /// 根据QId查询
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
            var data = await _equInfoApp.QueryByIdAsync(queryModel.QId);
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
                return ApiHelpers.Error("id不能为空，请输入");
            }
            var data = await _equInfoApp.FindById(id);
            if(data==null)
            {
                return ApiHelpers.Error("抱歉，数据不存在");
            }
            bool b = await _equInfoApp.DeleteAsync(id);
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
            if(ids.Length==0)
            {
                return ApiHelpers.Error("抱歉，id数组为空，请重新输入");
            }
            var data = await _equInfoApp.FindByIds(ids);
            if (data.Count() != ids.Length)
            {
                return ApiHelpers.Error("存在id未查找到的数据，请注意id的正确性");
            }
            bool b = await _equInfoApp.DeleteAsync(ids);
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
                return ApiHelpers.Error("pageIndex或者pageSize小于0，请重新输入");
            }
            var data = await _equInfoApp.QueryPageAsync(pageIndex, pageSize);
            if(data==null)
            {
                return ApiHelpers.Error("未查询到数据");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _equInfoApp.GetTotalNum();
                returnData.Total = num;
                return returnData;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipmentInfo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult> UpdateEquType(string id,EquipmentInfo equipmentInfo)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("请输入需要进行更新的设备id");
            }
            var dataQuery = await _equInfoApp.FindById(id);
            if(dataQuery==null)
            {
                return ApiHelpers.Error("未查找到该id相对应的数据，请检查id是否正确");
            }
            equipmentInfo.EQU_ID = dataQuery.EQU_ID;
            equipmentInfo.TYPE_ID = dataQuery.TYPE_ID;
            equipmentInfo.CREATED_TIME = dataQuery.CREATED_TIME;//锁死创建时间
            equipmentInfo.UPDATE_TIME = DateTime.Now;//给定修改时间
            bool b= await _equInfoApp.UpdateAsync(id, equipmentInfo);
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
        /// 模糊分页
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> QueryAndPage(QueryModel queryModel)
        {
            var data = await _equInfoApp.QueryAsync(queryModel.Q1, queryModel.Q2);
            var pageData = data.Skip(queryModel.pageSize*(queryModel.pageIndex-1)).Take(queryModel.pageSize);
            if(data==null)
            {
                return ApiHelpers.Error("数据为空");
            }
            var returnData= ApiHelpers.Success(pageData);
            returnData.Total = data.Count();
            return returnData;
        }
    }
}
