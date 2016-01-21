using System;
using System.Runtime.Caching;

namespace MainSolutionTemplate.Utilities.Cache
{
    public class SimpleObjectCache : ISimpleObjectCache
    {
        private readonly TimeSpan _defaultCacheTime;
        private readonly ObjectCache _objectCache;

        public SimpleObjectCache(TimeSpan defaultCacheTime)
        {
            _defaultCacheTime = defaultCacheTime;
            _objectCache = MemoryCache.Default;
        }

        #region ISimpleObjectCache Members

        public TValue Get<TValue>(string value, Func<TValue> getValue) where TValue : class
        {
            var retrievedValue = _objectCache.Get(value) as TValue;
            if (retrievedValue == null)
            {
                TValue result = getValue();
                if (result != null)
                {
                    _objectCache.Set(value, result, DateTimeOffset.Now.Add(_defaultCacheTime));
                    return result;
                }
            }
            return retrievedValue;
        }

        public TValue GetAndReset<TValue>(string value, Func<TValue> getValue) where TValue : class
        {
            var retrievedValue = _objectCache.Get(value) as TValue;

            if (retrievedValue == null)
            {
                TValue result = getValue();
                _objectCache.Set(value, result, DateTimeOffset.Now.Add(_defaultCacheTime));
                return result;
            }
            _objectCache.Set(value, retrievedValue, DateTimeOffset.Now.Add(_defaultCacheTime));
            return retrievedValue;
        }

        public void Set<TValue>(string value, TValue newvalue)
        {
            _objectCache.Set(value, newvalue, DateTimeOffset.Now.Add(_defaultCacheTime));
        }

        public TValue Get<TValue>(string value) where TValue : class
        {
            return _objectCache.Get(value) as TValue;
        }

        #endregion
    }
}