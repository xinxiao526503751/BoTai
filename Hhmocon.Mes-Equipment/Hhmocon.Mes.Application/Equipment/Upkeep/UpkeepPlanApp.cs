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
    public class UpkeepPlanApp : IUpkeepPlanApp
    {
        private readonly IUpkeepPlanRepository _upkeepPlanRepository;
        private readonly ILogger<UpkeepPlanApp> _logger;

        public UpkeepPlanApp(IUpkeepPlanRepository upkeepPlanRepository, ILogger<UpkeepPlanApp> logger)
        {
            this._upkeepPlanRepository = upkeepPlanRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(UpkeepPlan entity)
        {
            if (entity == null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            entity.IS_UPKEEP = "否";
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;

            bool b = await _upkeepPlanRepository.CreateAsync(entity);
            if (!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _upkeepPlanRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            if (ids == null)
            {
                return false;
            }
            return await _upkeepPlanRepository.DeleteAsync(ids);
        }

        public async Task<UpkeepPlan> FindById(string id)
        {
            id = id.Trim();
            return await _upkeepPlanRepository.FindById(id);
        }

        public Task<IEnumerable<UpkeepPlan>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<UpkeepPlan> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalNum()
        {
            return await _upkeepPlanRepository.GetTotalNum();
        }

        public async Task<IEnumerable<UpkeepPlan>> QueryAsync()
        {
            return await _upkeepPlanRepository.QueryAsync();
        }

        public async Task<IEnumerable<UpkeepPlan>> QueryAsync(string Q1, string Q2)
        {
            return await _upkeepPlanRepository.QueryAsync(Q1, Q2);
        }

        public Task<IEnumerable<UpkeepPlan>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UpkeepPlan>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _upkeepPlanRepository.QueryPageAsync(pageIndex,pageSize);
        }

        public async Task<bool> UpdateAsync(string id, UpkeepPlan entity)
        {
            bool b = await _upkeepPlanRepository.UpdateAsync(id, entity);
            if (!b)
            {
                throw new Exception(nameof(entity));
            }
            return true;
        }
    }
}
