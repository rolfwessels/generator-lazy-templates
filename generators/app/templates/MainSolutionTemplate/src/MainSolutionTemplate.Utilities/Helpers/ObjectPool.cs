using System;
using System.Threading;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _getGeneralUnitOfWork;
        private readonly int _size;
        private readonly T[] _generalUnitOfWorks;
        private int _counter;

        public ObjectPool(Func<T> getGeneralUnitOfWork, int size = 4)
        {
            _getGeneralUnitOfWork = getGeneralUnitOfWork;
            _size = size;
            _generalUnitOfWorks = new T[_size];
            _counter = 0;
        }

        public T Get()
        {
            var index0 = _counter % _size;
            Interlocked.Increment(ref _counter);
            if (_generalUnitOfWorks[index0] == null)
            {
                _generalUnitOfWorks[index0] = _getGeneralUnitOfWork();
                
            }
            return _generalUnitOfWorks[index0];
            
        }
    }
}