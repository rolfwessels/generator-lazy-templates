namespace MainSolutionTemplate.Api.AppStartup
{
	public class SwaggerSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static SwaggerSetup _instance;

		protected SwaggerSetup()
		{
		//	var xmlDoc = System.String.Format(@"{0}\bin\WebApiSwagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
//			SwaggerSpecConfig.Customize(c =>
//			{
//				c.IncludeXmlComments(GetXmlCommentsPath());
//			});
		}

		#region Initialize

		public static void Initialize()
		{
			if (_isInitialized) return;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new SwaggerSetup();
					_isInitialized = true;
				}
			}
		}

		#endregion

	}
}