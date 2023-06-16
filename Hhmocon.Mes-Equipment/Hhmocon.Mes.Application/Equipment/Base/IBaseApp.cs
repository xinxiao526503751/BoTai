using Hhmocon.Mes.Repository.HelpModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Application.Equipment.Base
{
    public interface IBaseApp<TEntity> where TEntity:class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(TEntity entity);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string[] ids);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string id, TEntity entity);
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindById(string id);
        /// <summary>
        /// 批量查找
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> FindByIds(string[] ids);
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TEntity> FindByName(string name);
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryPageAsync(int pageIndex, int pageSize);
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync(string Q1, string Q2);
        /// <summary>
        /// 获取数据量
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalNum();
        /// <summary>
        /// 根据type_id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryByIdAsync(string id);
    }
}
