using MainSolutionTemplate.Core.Startup;

namespace MainSolutionTemplate.Web.AppStartup
{
	public class IocContainerSetup : IocContainerBase
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocContainerSetup _instance;

		public IocContainerSetup()
		{
		}

		#region Initialize

		public static void Initialize()
		{
			if (_isInitialized) return;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new IocContainerSetup();
					_isInitialized = true;
				}
			}
		}

		#endregion

	}
}