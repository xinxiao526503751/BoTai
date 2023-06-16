using Hhmocon.Mes.Application.Equipment.WorkPlace;
using Hhmocon.Mes.Repository.Domain.Equipment;
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
    [ApiExplorerSettings(GroupName = "EquType", IgnoreApi = false)]
    public class WorkPlaceController : ControllerBase
    {
        private readonly IWorkPlaceApp _workPlaceApp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workPlaceApp"></param>
        public WorkPlaceController(IWorkPlaceApp workPlaceApp)
        {
            this._workPlaceApp = workPlaceApp;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="workShop"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> AddWorkPlace(WorkShop workShop)
        {
            if(workShop==null)
            {
                return ApiHelpers.Error("抱歉，添加数据为空");
            }
            bool b = await _workPlaceApp.CreateAsync(workShop);
            if(!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(workShop);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _workPlaceApp.QueryAsync();
            return ApiHelpers.ReturnCountAndSuccess(data);
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
                return ApiHelpers.Error("抱歉！id为空，请重新输入");
            }

            bool b = await _workPlaceApp.DeleteAsync(id);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
    }
}
