namespace MainSolutionTemplate.Api.AppStartup
{
    public class BootStrap
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
	    private static BootStrap _instance;

	    protected BootStrap()
	    {
			
	    }

	    #region Initialize

		public static BootStrap Initialize()
        {
			if (_isInitialized) return _instance;
            lock (_locker)
            {
                if (!_isInitialized)
                {
	                _instance = new BootStrap();
	                _isInitialized = true;
                }
            }
			return _instance;
        }

	    #endregion
    }

	
}