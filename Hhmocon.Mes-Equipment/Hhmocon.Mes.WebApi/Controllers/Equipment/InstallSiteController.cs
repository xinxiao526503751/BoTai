using Hhmocon.Mes.Application.Equipment.InstallSites;
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
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "EquType", IgnoreApi = false)]
    public class InstallSiteController : ControllerBase
    {
        private readonly IInstallSiteApp _installSiteApp;

        public InstallSiteController(IInstallSiteApp installSiteApp)
        {
            this._installSiteApp = installSiteApp;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="installSite"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> AddInstallSite(InstallSite installSite)
        {
            if (installSite == null)
            {
                return ApiHelpers.Error("抱歉，添加数据为空");
            }
            bool b = await _installSiteApp.CreateAsync(installSite);
            if (!b)
            {
                return ApiHelpers.Error("添加失败");
            }
            return ApiHelpers.Success(installSite);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult> GetAll()
        {
            var data = await _installSiteApp.QueryAsync();
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

            bool b = await _installSiteApp.DeleteAsync(id);
            if (!b)
            {
                return ApiHelpers.Error("删除失败");
            }
            return ApiHelpers.Success(b);
        }
        [HttpGet]
        public async Task<ApiResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiHelpers.Error("抱歉！id为空，请重新输入");
            }

            var data = await _installSiteApp.QueryByIdAsync(id);
            if (data == null)
            {
                return ApiHelpers.Error("抱歉，数据为空");
            }
            return ApiHelpers.ReturnCountAndSuccess(data);
        }
    }
}
