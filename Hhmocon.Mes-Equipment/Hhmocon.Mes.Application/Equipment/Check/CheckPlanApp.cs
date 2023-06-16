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
    public class CheckPlanApp : ICheckPlanApp
    {
        private readonly ICheckPlanRepository _checkPlanRepository;
        private readonly ILogger<CheckPlanApp> _logger;

        public CheckPlanApp(ICheckPlanRepository checkPlanRepository, ILogger<CheckPlanApp> logger)
        {
            this._checkPlanRepository = checkPlanRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(CheckPlan entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            entity.IS_CHECK = "否";
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;

            bool b=await _checkPlanRepository.CreateAsync(entity);
            if(!b)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _checkPlanRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(string[] ids)
        {
            if (ids == null)
            {
                return false;
            }
            return await _checkPlanRepository.DeleteAsync(ids);
        }

        public async Task<CheckPlan> FindById(string id)
        {
            id = id.Trim();
            return await _checkPlanRepository.FindById(id);
        }

        public Task<IEnumerable<CheckPlan>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<CheckPlan> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public async Task<int> GetTotalNum()
        {
            return await _checkPlanRepository.GetTotalNum();
        }

        public async Task<IEnumerable<CheckPlan>> QueryAsync()
        {
            return await _checkPlanRepository.QueryAsync();
        }

        public async Task<IEnumerable<CheckPlan>> QueryAsync(string Q1, string Q2)
        {
            return await _checkPlanRepository.QueryAsync(Q1, Q2);
            
        }

        public Task<IEnumerable<CheckPlan>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CheckPlan>> QueryPageAsync(int pageIndex, int pageSize)
        {
            return await _checkPlanRepository.QueryPageAsync(pageIndex,pageSize);
        }
        public async Task<bool> UpdateAsync(string id, CheckPlan entity)
        {
            bool b = await _checkPlanRepository.UpdateAsync(id, entity);
            if(!b)
            {
                throw new Exception(nameof(entity));
            }
            return true;
        }
    }
}
