using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.EquInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.EquInfo
{
    public class EquInfoApp : IEquInfoApp
    {
        private readonly IEquInfoRepository _equInfoRepository;
        private readonly ILogger<EquInfoApp> _logger;

        public EquInfoApp(IEquInfoRepository equInfoRepository, ILogger<EquInfoApp> logger)
        {
            this._equInfoRepository = equInfoRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(EquipmentInfo entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new ArgumentNullException(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            bool b = await _equInfoRepository.CreateAsync(entity);
            if(!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            return await _equInfoRepository.DeleteAsync(ids);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _equInfoRepository.DeleteAsync(id);
        }

        public async Task<EquipmentInfo> FindById(string id)
        {
            id = id.Trim();
            return await _equInfoRepository.FindById(id);
        }

        public async Task<IEnumerable<EquipmentInfo>> FindByIds(string[] ids)
        {
            return await _equInfoRepository.FindByIds(ids);
        }

        public async Task<EquipmentInfo> FindByName(string name)
        {
            name = name.Trim();
            return await _equInfoRepository.FindByName(name);
        }

        public Task<EquipmentInfo> FindByTwoIds(string id1, string id2)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalNum()
        {
            return await _equInfoRepository.GetTotalNum();
        }

        public async Task<IEnumerable<EquipmentInfo>> QueryAsync()
        {
            return await _equInfoRepository.QueryAsync();
        }

        public async Task<IEnumerable<EquipmentInfo>> QueryAsync(string Q1, string Q2)
        {
            var data = await _equInfoRepository.QueryAsync(Q1,Q2);

            return data;
        }

        public async Task<IEnumerable<EquipmentInfo>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _equInfoRepository.QueryByIdAsync(id);
        }

        public async Task<IEnumerable<EquipmentInfo>> QueryPageAsync(int pageIndex, int pageSize)
        {
            var data = await _equInfoRepository.QueryPageAsync(pageIndex,pageSize);
            if(data==null)
            {
                throw new Exception(nameof(data));
            }
            return data;
        }

        public async Task<bool> UpdateAsync(string id, EquipmentInfo entity)
        {
            bool b = await _equInfoRepository.UpdateAsync(id, entity);
            if(!b)
            {
                throw new Exception(nameof(b));
            }
            return true;
        }
    }
}
