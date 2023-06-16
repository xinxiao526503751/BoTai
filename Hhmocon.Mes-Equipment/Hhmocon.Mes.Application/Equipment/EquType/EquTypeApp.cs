using Hhmocon.Mes.Application.Equipment.Base;
using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.EquType;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.EquType
{
    public class EquTypeApp : IEquTypeApp
    {
        private readonly IEquTypeRepository _equTypeRepository;
        private readonly ILogger<EquTypeApp> _logger;

        public EquTypeApp(IEquTypeRepository equTypeRepository, ILogger<EquTypeApp> logger)
        {
            this._equTypeRepository = equTypeRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(EquipmentType entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new ArgumentNullException(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            bool b = await _equTypeRepository.CreateAsync(entity);
            if(!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _equTypeRepository.DeleteAsync(id);
        }

        public async Task<EquipmentType> FindById(string id)
        {
            id = id.Trim();
            return await _equTypeRepository.FindById(id);
        }

        public Task<IEnumerable<EquipmentType>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<EquipmentType> FindByName(string name)
        {
            name = name.Trim();
            return await _equTypeRepository.FindByName(name);
        }

        public Task<EquipmentType> FindByTwoIds(string id1, string id2)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalNum()
        {
            return await _equTypeRepository.GetTotalNum();
        }

        public async Task<IEnumerable<EquipmentType>> QueryAsync()
        {
            return await _equTypeRepository.QueryAsync();
        }

        public Task<IEnumerable<EquipmentType>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EquipmentType>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EquipmentType>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _equTypeRepository.QueryPageAsync(pageIndex,pageSize);
        }

        public Task<bool> UpdateAsync(string id, EquipmentType entity)
        {
            throw new NotImplementedException();
        }
    }
}
