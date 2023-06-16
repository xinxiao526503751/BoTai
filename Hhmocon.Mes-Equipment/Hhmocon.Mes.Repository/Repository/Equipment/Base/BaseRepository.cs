using Hhmocon.Mes.Repository.HelpModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Repository.Equipment.Base
{
    /// <summary>
    /// 泛型基接口实现类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string[] ids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> FindById(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 批量查找
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> FindByIds(string[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<TEntity> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <returns></returns>
        public Task<int> GetTotalNum()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> QueryAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> QueryAsync(string Q1, string Q2)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据type_id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> QueryByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> QueryPageAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
