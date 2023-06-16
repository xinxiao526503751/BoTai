using Microsoft.Extensions.Caching.Memory;
using System;

namespace Hhmocon.Mes.Cache
{
    public class CacheContext : ICacheContext
    {
        private readonly IMemoryCache _objCache;

        public CacheContext(IMemoryCache objCache)
        {
            _objCache = objCache;
        }

        public override T Get<T>(string key)
        {
            return _objCache.Get<T>(key);
        }

        public override bool Set<T>(string key, T t, DateTime expire)
        {
            T obj = Get<T>(key);
            if (obj != null)
            {
                Remove(key);
            }

            MemoryCacheEntryOptions memoryCacheEntryOptions = new();
            memoryCacheEntryOptions.SetAbsoluteExpiration(expire);
            _objCache.Set(key, t, memoryCacheEntryOptions);   //绝对过期时间

            return true;
        }

        public override bool Remove(string key)
        {
            _objCache.Remove(key);
            return true;
        }
    }
}
