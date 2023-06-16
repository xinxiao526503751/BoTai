using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.Maintain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Maintain
{
    public class MaintainItemApp : IMaintainItemApp
    {
        private readonly IMaintainItemRepository _maintainItemRepository;
        private readonly ILogger<MaintainItemApp> _logger;

        public MaintainItemApp(IMaintainItemRepository maintainItemRepository, ILogger<MaintainItemApp> logger)
        {
            this._maintainItemRepository = maintainItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(MaintainItem entity)
        {
            if (entity == null)
            {
                _logger.LogError("新增数据为空");
                return false;
            }
            return await _maintainItemRepository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _maintainItemRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            if(ids==null)
            {
                return false;
            }
            return await _maintainItemRepository.DeleteAsync(ids);
        }

        public async Task<MaintainItem> FindById(string id)
        {
            id = id.Trim();
            return await _maintainItemRepository.FindById(id);
        }

        public async Task<IEnumerable<MaintainItem>> FindByIds(string[] ids)
        {
            if (ids == null)
            {
                _logger.LogError("ids为空");
                throw new Exception(nameof(ids));
            }
            return await _maintainItemRepository.FindByIds(ids);
        }

        public async Task<MaintainItem> FindByName(string name)
        {
            name = name.Trim();
            return await _maintainItemRepository.FindByName(name);
        }

        public async Task<int> GetTotalNum()
        {
            return await _maintainItemRepository.GetTotalNum();
        }

        public async Task<IEnumerable<MaintainItem>> QueryAsync()
        {
            return await _maintainItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<MaintainItem>> QueryAsync(string Q1, string Q2)
        {
            return await _maintainItemRepository.QueryAsync(Q1,Q2);
        }

        public async Task<IEnumerable<MaintainItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _maintainItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<MaintainItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _maintainItemRepository.QueryPageAsync(pageIndex,pageSize);
        }

        public async Task<bool> UpdateAsync(string id, MaintainItem entity)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return false;
            }
            return await _maintainItemRepository.UpdateAsync(id,entity);
        }
    }
}
