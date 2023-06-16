using System;
using System.Collections.Concurrent;
   
namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// sqlServer缓存
    /// </summary>
    internal class SqlServerCache
    {
        /// <summary>
        /// 句柄Handle封装该类型内部数据结构的指针
        /// 比如可以通过Type的Handle获取Type的类型
        /// </summary>
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, TableEntity> tableDict = new ConcurrentDictionary<RuntimeTypeHandle, TableEntity>();
        private static readonly object _locker = new object();

        /// <summary>
        /// 根据T创建表的实体类TableEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableEntity GetTableEntity<T>()
        {
            Type t = typeof(T);
            RuntimeTypeHandle typeHandle = t.TypeHandle;
            //如果字典中不包含泛型T的句柄
            if (!tableDict.Keys.Contains(typeHandle))
            {
                lock (_locker)
                {
                    if (!tableDict.Keys.Contains(typeHandle))
                    {
                        TableEntity table = CommonUtil.CreateTableEntity(t);
                        CommonUtil.InitTableForOracle(table);
                        tableDict[typeHandle] = table;//存入字典
                    }
                }
            }

            //通过 字典[句柄] 返回根据类型创建的表实体类 
            var res = tableDict[typeHandle];
            return tableDict[typeHandle];
        }
    }
}
