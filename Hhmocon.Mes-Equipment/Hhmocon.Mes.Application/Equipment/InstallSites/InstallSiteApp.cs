using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.Repository.Equipment.InstallSites;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.InstallSites
{
    public class InstallSiteApp : IInstallSiteApp
    {
        private readonly IInstallSiteRepository _installSiteRepository;
        private readonly ILogger<InstallSiteApp> _logger;

        public InstallSiteApp(IInstallSiteRepository installSiteRepository, ILogger<InstallSiteApp> logger)
        {
            this._installSiteRepository = installSiteRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(InstallSite entity)
        {
            if (entity == null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            return await _installSiteRepository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _installSiteRepository.DeleteAsync(id);
        }

        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<InstallSite> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InstallSite>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<InstallSite> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstallSite>> QueryAsync()
        {
            return await _installSiteRepository.QueryAsync();
        }

        public Task<IEnumerable<InstallSite>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstallSite>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _installSiteRepository.QueryByIdAsync(id);
        }

        public Task<IEnumerable<InstallSite>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, InstallSite entity)
        {
            throw new NotImplementedException();
        }
    }
}
