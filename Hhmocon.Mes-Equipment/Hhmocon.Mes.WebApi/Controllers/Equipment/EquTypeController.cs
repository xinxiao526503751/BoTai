using Hhmocon.Mes.Application.Equipment.Check;
using Hhmocon.Mes.Application.Equipment.EquInfo;
using Hhmocon.Mes.Application.Equipment.EquType;
using Hhmocon.Mes.Application.Equipment.Maintain;
using Hhmocon.Mes.Application.Equipment.Upkeep;
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
    /// 设备类型控制类
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquType", IgnoreApi = false)]
    public class EquTypeController : ControllerBase
    {
        private readonly IEquTypeApp _equTypeApp;
        private readonly IEquInfoApp _equInfoApp;
        private readonly ICheckItemApp _checkItemApp;
        private readonly IUpkeepItemApp _upkeepItemApp;
        private readonly IMaintainItemApp _maintainItemApp;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="equTypeApp"></param>
        /// <param name="equInfoApp"></param>
        /// <param name="checkItemApp"></param>
        /// <param name="upkeepItemApp"></param>
        /// <param name="maintainItemApp"></param>
        public EquTypeController(
            IEquTypeApp equTypeApp, 
            IEquInfoApp equInfoApp,
            ICheckItemApp checkItemApp,
            IUpkeepItemApp upkeepItemApp,
            IMaintainItemApp maintainItemApp)
        {
            this._equTypeApp = equTypeApp;
            this._equInfoApp = equInfoApp;
            this._checkItemApp = checkItemApp;
            this._upkeepItemApp = upkeepItemApp;
            this._maintainItemApp = maintainItemApp;
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _equTypeApp.QueryAsync();
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
        /// <summary>
        /// 根据id查找
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("id为空，请重新输入");
            }
            var data = await _equTypeApp.FindById(id);
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 根据name查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        public async Task<ApiResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ApiHelpers.Error("name为空，请重新输入");
            }
            var data = await _equTypeApp.FindByName(name);
            return ApiHelpers.Success(data);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equipmentType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> CreateEquType(EquipmentType equipmentType)
        {
            var getById = await _equTypeApp.FindById(equipmentType.EQU_TYPE_ID);
            var getByName = await _equTypeApp.FindByName(equipmentType.EQU_TYPE_NAME);
            if (getById != null)
            {
                return ApiHelpers.Error($"类型编号为{equipmentType.EQU_TYPE_ID}已存在，请重新命名");
            }
            if (getByName != null)
            {
                return ApiHelpers.Error($"类型名称为{equipmentType.EQU_TYPE_NAME}已存在，请重新命名");
            }
            bool b = await _equTypeApp.CreateAsync(equipmentType);
            if (!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(equipmentType);
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResult> DeleteEquType(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉，编码不能为空，请重新输入");
            }
            var equTypeData = await _equTypeApp.FindById(id);
            if (equTypeData==null)
            {
                return ApiHelpers.Error("此设备不存在，无需进行删除");
            }
            var equInfoData = await _equInfoApp.QueryByIdAsync(id);
            var checkItemData = await _checkItemApp.QueryByIdAsync(id);
            var upkeepItemData = await _upkeepItemApp.QueryByIdAsync(id);
            var maintainItemData = await _maintainItemApp.QueryByIdAsync(id);
            if (equInfoData.Count() != 0)
            {
                return ApiHelpers.Error($"此设备类型已被{equInfoData.Count()}个设备关联，无法越级删除");
            }
            if (checkItemData.Count()!=0)
            {
                return ApiHelpers.Error($"此设备类型挂载着{checkItemData.Count()}个点检项目，无法越级删除");
            }
            if (upkeepItemData.Count() != 0)
            {
                return ApiHelpers.Error($"此设备类型挂载着{upkeepItemData.Count()}个保养项目，无法越级删除");
            }
            if (maintainItemData.Count() != 0)
            {
                return ApiHelpers.Error($"此设备类型挂载着{maintainItemData.Count()}个维修项目，无法越级删除");
            }
            bool b = await _equTypeApp.DeleteAsync(id);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetEquTypePage(int pageindex, int pagesize)
        {
            if (pageindex<0||pagesize<0)
            {
                return ApiHelpers.Error("pageindex或pagesize小于0，请重新输入");
            }
            var data = await _equTypeApp.QueryPageAsync(pageindex,pagesize);
            if(data==null)
            {
                return ApiHelpers.Error("数据为空");
            }
            else
            {
                var returnData = ApiHelpers.ReturnCountAndSuccess(data);
                int num = await _equTypeApp.GetTotalNum();
                returnData.Total = num;
                return returnData;
            }
        }
    }
}
