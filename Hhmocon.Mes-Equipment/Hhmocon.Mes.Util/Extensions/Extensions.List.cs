﻿using System.Collections.Generic;


namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 描 述：List扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取list的分页数据
        /// </summary>
        /// <param name="obj">list对象</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public static List<T> FindPage<T>(this List<T> obj, Pagination pagination) where T : class
        {
            pagination.records = obj.Count;
            int index = (pagination.page - 1) * pagination.rows;
            if (index >= obj.Count)
            {
                return new List<T>();
            }
            int end = index + pagination.rows;
            int count = end > obj.Count ? obj.Count - index : pagination.rows;
            List<T> list = obj.GetRange(index, count);
            return list;
        }
    }
}
