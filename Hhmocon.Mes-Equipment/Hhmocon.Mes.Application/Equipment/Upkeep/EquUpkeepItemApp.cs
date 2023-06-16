using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.Upkeep;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Upkeep
{
    public class EquUpkeepItemApp : IEquUpkeepItemApp
    {
        private readonly IEquUpkeepItemRepository _equUpkeepItemRepository;
        private readonly ILogger<EquUpkeepItemApp> _logger;

        public EquUpkeepItemApp(IEquUpkeepItemRepository equUpkeepItemRepository, ILogger<EquUpkeepItemApp> logger)
        {
            this._equUpkeepItemRepository = equUpkeepItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(EquUpkeepItem entity)
        {
            if (entity == null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            bool b = await _equUpkeepItemRepository.CreateAsync(entity);
            if (!b)
            {
                return false;
            }
            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            return await _equUpkeepItemRepository.DeleteAsync(ids);
        }

        public async Task<EquUpkeepItem> FindById(string id)
        {
            id = id.Trim();
            return await _equUpkeepItemRepository.FindById(id);
        }

        public Task<IEnumerable<EquUpkeepItem>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<EquUpkeepItem> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalNum()
        {
            return await _equUpkeepItemRepository.GetTotalNum();
        }

        public async Task<IEnumerable<EquUpkeepItem>> QueryAsync()
        {
            return await _equUpkeepItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<EquUpkeepItem>> QueryAsync(string Q1, string Q2)
        {
            return await _equUpkeepItemRepository.QueryAsync(Q1, Q2);
        }

        public async Task<IEnumerable<EquUpkeepItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _equUpkeepItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<EquUpkeepItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _equUpkeepItemRepository.QueryPageAsync(pageIndex, pageSize);
        }

        public async Task<bool> UpdateAsync(string id, EquUpkeepItem entity)
        {
            bool b = await _equUpkeepItemRepository.UpdateAsync(id, entity);
            if (!b)
            {
                throw new Exception(nameof(entity));
            }
            return true;
        }
    }
}
