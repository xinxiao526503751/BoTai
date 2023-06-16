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
    public class EquMaintainItemApp : IEquMaintainItemApp
    {
        private readonly IEquMaintainItemRepository _equMaintainItemRepository;
        private readonly ILogger<EquMaintainItemApp> _logger;

        public EquMaintainItemApp(IEquMaintainItemRepository equMaintainItemRepository,ILogger<EquMaintainItemApp> logger)
        {
            this._equMaintainItemRepository = equMaintainItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(EquMaintainItem entity)
        {
            if (entity == null)
            {
                _logger.LogError("数据为空");
                throw new Exception(nameof(entity));
            }
            return await _equMaintainItemRepository.CreateAsync(entity);
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByTwoId(string id1, string id2)
        {
            return await _equMaintainItemRepository.DeleteByTwoId(id1,id2);
        }

        public async Task<EquMaintainItem> FindById(string id)
        {
            id = id.Trim();
            return await _equMaintainItemRepository.FindById(id);
        }

        public Task<IEnumerable<EquMaintainItem>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<EquMaintainItem> FindByName(string name)
        {
            name = name.Trim();
            return await _equMaintainItemRepository.FindByName(name);
        }

        public async Task<EquMaintainItem> FindByTwoId(string id1, string id2)
        {
            if (string.IsNullOrWhiteSpace(id1) && string.IsNullOrWhiteSpace(id2))
            {
                throw new Exception("参数不能未空");
            }
            return await _equMaintainItemRepository.FindByTwoId(id1,id2);
        }

        public async Task<int> GetTotalNum()
        {
            return await _equMaintainItemRepository.GetTotalNum();
        }

        public  async Task<IEnumerable<EquMaintainItem>> QueryAsync()
        {
            return await _equMaintainItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<EquMaintainItem>> QueryAsync(string Q1, string Q2)
        {
            return await _equMaintainItemRepository.QueryAsync(Q1,Q2);
        }

        public async Task<IEnumerable<EquMaintainItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _equMaintainItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<EquMaintainItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _equMaintainItemRepository.QueryPageAsync(pageIndex, pageSize);
        }

        public Task<bool> UpdateAsync(string id, EquMaintainItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWithTwoId(string id1, string id2, EquMaintainItem equMaintainItem)
        {
            return await _equMaintainItemRepository.UpdateWithTwoId(id1,id2,equMaintainItem);
        }
    }
}
