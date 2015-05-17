using MainSolutionTemplate.OAuth2;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using Owin;

namespace MainSolutionTemplate.Api.AppStartup
{
    public class BootStrap
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
	    private static BootStrap _instance;

	    protected BootStrap(IAppBuilder appBuilder)
	    {
            OathAuthorizationSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IOAuthDataManager>());
	    }

	    #region Initialize

		public static BootStrap Initialize(IAppBuilder appBuilder)
        {
			if (_isInitialized) return _instance;
            lock (_locker)
            {
                if (!_isInitialized)
                {
					_instance = new BootStrap(appBuilder);
	                _isInitialized = true;
                }
            }
			return _instance;
        }

	    #endregion
    }

	
}