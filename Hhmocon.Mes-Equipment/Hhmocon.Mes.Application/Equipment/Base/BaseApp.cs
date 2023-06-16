using Hhmocon.Mes.Repository.HelpModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Base
{
    public class BaseApp<TEntity> : IBaseApp<TEntity> where TEntity : class
    {
        public Task<bool> CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
