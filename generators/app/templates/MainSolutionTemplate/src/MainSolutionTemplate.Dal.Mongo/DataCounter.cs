using System.Threading;

namespace MainSolutionTemplate.Dal.Mongo
{
    public class DataCounter
    {
        private long _getCounter;
        private long _deleteCounter;
        private long _insertCounter;
        private long _updateCounter;
        private string _name;

        public DataCounter(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public long GetCounter
        {
            get { return _getCounter; }
        }

        public long DeleteCounter
        {
            get { return _deleteCounter; }
        }

        public long UpdateCounter
        {
            get { return _updateCounter; }
        }

        public long InsertCounter
        {
            get { return _insertCounter; }
        }

        public void AddDelete()
        {
            Interlocked.Increment(ref _deleteCounter);
        }

        public void AddUpdate()
        {
            Interlocked.Increment(ref _updateCounter);
        }

        public void AddInsert()
        {
            Interlocked.Increment(ref _insertCounter);
        }

        public void AddGet()
        {
            Interlocked.Increment(ref _getCounter);
        }
    }
}