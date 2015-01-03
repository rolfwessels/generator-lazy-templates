namespace MainSolutionTemplate.Web.AppStartup
{
	public class AutoMapperSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static AutoMapperSetup _instance;

		protected AutoMapperSetup()
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
					_instance = new AutoMapperSetup();
					_isInitialized = true;
				}
			}
		}

		#endregion

	}
}