using Hhmocon.Mes.Repository.Domain.Equipment;
using Hhmocon.Mes.Repository.HelpModel;
using Hhmocon.Mes.Repository.Repository.Equipment.WorkPlace;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.WorkPlace
{
    public class WorkPlaceApp : IWorkPlaceApp
    {
        private readonly IWorkPlaceRepository _workPlaceRepository;
        private readonly ILogger<WorkPlaceApp> _logger;

        public WorkPlaceApp(IWorkPlaceRepository workPlaceRepository, ILogger<WorkPlaceApp> logger)
        {
            this._workPlaceRepository = workPlaceRepository;
            this._logger = logger;
        }
        public async Task<bool> CreateAsync(WorkShop entity)
        {
            if(entity==null)
            {
                _logger.LogError("新增数据为空");
                throw new Exception(nameof(entity));
            }
            entity.CREATED_TIME = DateTime.Now;
            entity.UPDATE_TIME = null;
            entity.UPDATE_BY = null;
            return await _workPlaceRepository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            id = id.Trim();
            return await _workPlaceRepository.DeleteAsync(id);
        }

        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<WorkShop> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkShop>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<WorkShop> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkShop>> QueryAsync()
        {
            return await _workPlaceRepository.QueryAsync();
        }

        public Task<IEnumerable<WorkShop>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkShop>> QueryByIdAsync(string id)
        {
            id = id.Trim();
            return await _workPlaceRepository.QueryByIdAsync(id);
        }

        public Task<IEnumerable<WorkShop>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, WorkShop entity)
        {
            throw new NotImplementedException();
        }
    }
}
