namespace MainSolutionTemplate.Api.AppStartup
{
    public class BootStrap
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
	    private static BootStrap _instance;

	    protected BootStrap()
	    {
			AutoMapperSetup.Initialize();
			IocContainerSetup.Initialize();
	    }

	    #region Initialize

		public static void Initialize()
        {
			if (_isInitialized) return;
            lock (_locker)
            {
                if (!_isInitialized)
                {
	                _instance = new BootStrap();
	                _isInitialized = true;
                }
            }
        }

	    #endregion
    }
}