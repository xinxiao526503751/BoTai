using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.Check;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Check
{
    public class EquCheckItemApp : IEquCheckItemApp
    {
        private readonly IEquCheckItemRepository _equCheckItemRepository;
        private readonly ILogger<EquCheckItemApp> _logger;

        public EquCheckItemApp(IEquCheckItemRepository equCheckItemRepository, ILogger<EquCheckItemApp> logger)
        {
            this._equCheckItemRepository = equCheckItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(EquCheckItem entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            bool b = await _equCheckItemRepository.CreateAsync(entity);
            if(!b)
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
            return await _equCheckItemRepository.DeleteAsync(ids);
        }

        public async Task<EquCheckItem> FindById(string id)
        {
            id = id.Trim();
            return await _equCheckItemRepository.FindById(id);
        }

        public Task<IEnumerable<EquCheckItem>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<EquCheckItem> FindByName(string name)
        {
            name = name.Trim();
            return await _equCheckItemRepository.FindByName(name);
        }

        public async Task<int> GetTotalNum()
        {
            return await _equCheckItemRepository.GetTotalNum();
        }

        public async Task<IEnumerable<EquCheckItem>> QueryAsync()
        {
            return await _equCheckItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<EquCheckItem>> QueryAsync(string Q1, string Q2)
        {
            return await _equCheckItemRepository.QueryAsync(Q1,Q2);
        }

        public async Task<IEnumerable<EquCheckItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _equCheckItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<EquCheckItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _equCheckItemRepository.QueryPageAsync(pageIndex, pageSize);
        }

        public async Task<bool> UpdateAsync(string id, EquCheckItem entity)
        {
            bool b = await _equCheckItemRepository.UpdateAsync(id, entity);
            if (!b)
            {
                throw new Exception(nameof(entity));
            }
            return true;
        }
    }
}
