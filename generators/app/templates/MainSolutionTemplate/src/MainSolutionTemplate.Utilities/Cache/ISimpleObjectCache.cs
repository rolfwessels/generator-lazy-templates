using System;

namespace MainSolutionTemplate.Utilities.Cache
{
    public interface ISimpleObjectCache
    {
        TValue Get<TValue>(string value, Func<TValue> getValue) where TValue : class;
        TValue GetAndReset<TValue>(string value, Func<TValue> getValue) where TValue : class;
        void Set<TValue>(string value, TValue newvalue);
        TValue Get<TValue>(string value) where TValue : class;
    }
}