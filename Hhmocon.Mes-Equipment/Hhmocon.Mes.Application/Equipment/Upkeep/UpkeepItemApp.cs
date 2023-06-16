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
    public class UpkeepItemApp : IUpkeepItemApp
    {
        private readonly IUpkeepItemRepository _upkeepItemRepository;
        private readonly ILogger<UpkeepItemApp> _logger;

        public UpkeepItemApp(IUpkeepItemRepository upkeepItemRepository, ILogger<UpkeepItemApp> logger)
        {
            this._upkeepItemRepository = upkeepItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(UpkeepItem entity)
        {
            if (entity == null)
            {
                _logger.LogError("新增数据为空");
                throw new ArgumentNullException(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            bool b = await _upkeepItemRepository.CreateAsync(entity);
            if (!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _upkeepItemRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            return await _upkeepItemRepository.DeleteAsync(ids);
        }

        public async Task<UpkeepItem> FindById(string id)
        {
            id = id.Trim();
            return await _upkeepItemRepository.FindById(id);
        }

        public async Task<IEnumerable<UpkeepItem>> FindByIds(string[] ids)
        {
            return await _upkeepItemRepository.FindByIds(ids);
        }

        public async Task<UpkeepItem> FindByName(string name)
        {
            name = name.Trim();
            return await _upkeepItemRepository.FindByName(name);
        }

        public async Task<int> GetTotalNum()
        {
            return await _upkeepItemRepository.GetTotalNum();
        }

        public async Task<IEnumerable<UpkeepItem>> QueryAsync()
        {
            return await _upkeepItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<UpkeepItem>> QueryAsync(string Q1, string Q2)
        {
            return await _upkeepItemRepository.QueryAsync(Q1,Q2);
        }

        public async Task<IEnumerable<UpkeepItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _upkeepItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<UpkeepItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            var data = await _upkeepItemRepository.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                throw new Exception(nameof(data));
            }
            return data;
        }

        public Task<bool> UpdateAsync(string id, UpkeepItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
