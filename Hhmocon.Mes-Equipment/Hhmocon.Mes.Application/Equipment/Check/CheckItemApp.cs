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
    public class CheckItemApp : ICheckItemApp
    {
        private readonly ICheckItemRepository _checkItemRepository;
        private readonly ILogger<CheckItemApp> _logger;

        public CheckItemApp(ICheckItemRepository checkItemRepository, ILogger<CheckItemApp> logger)
        {
            this._checkItemRepository = checkItemRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(CheckItem entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new ArgumentNullException(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            bool b = await _checkItemRepository.CreateAsync(entity);
            if (!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _checkItemRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            return await _checkItemRepository.DeleteAsync(ids);
        }

        public async Task<CheckItem> FindById(string id)
        {
            id = id.Trim();
            return await _checkItemRepository.FindById(id);
        }

        public async Task<IEnumerable<CheckItem>> FindByIds(string[] ids)
        {
            return await _checkItemRepository.FindByIds(ids);
        }

        public async Task<CheckItem> FindByName(string name)
        {
            name = name.Trim();
            return await _checkItemRepository.FindByName(name);
        }


        public async Task<int> GetTotalNum()
        {
            return await _checkItemRepository.GetTotalNum();
        }

        public async Task<IEnumerable<CheckItem>> QueryAsync()
        {
            return await _checkItemRepository.QueryAsync();
        }

        public async Task<IEnumerable<CheckItem>> QueryAsync(string Q1, string Q2)
        {
            return await _checkItemRepository.QueryAsync(Q1, Q2);
        }

        public async Task<IEnumerable<CheckItem>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _checkItemRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<CheckItem>> QueryPageAsync(int pageIndex, int pageSize)
        {
            var data = await _checkItemRepository.QueryPageAsync(pageIndex, pageSize);
            if (data == null)
            {
                throw new Exception(nameof(data));
            }
            return data;
        }

        public Task<bool> UpdateAsync(string id, CheckItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
