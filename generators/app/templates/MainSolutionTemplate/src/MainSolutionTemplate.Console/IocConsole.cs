using Autofac;
using MainSolutionTemplate.Core.Startup;
using MainSolutionTemplate.Dal.Mongo;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Console
{
	public class IocConsole : IocCoreBase
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocConsole _instance;
		private readonly IContainer _container;

	    public IocConsole()
		{
		    
	        var builder = new ContainerBuilder();
			SetupCore(builder);
		    SetupTools(builder);
			_container = builder.Build();
		    
		}
        
        private void SetupTools(ContainerBuilder builder)
		{
            
		}

		#region Instance

		public static IocConsole Instance
		{
			get
			{
				if (_isInitialized) return _instance;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_instance = new IocConsole();
						_isInitialized = true;
					}
				}
				return _instance;
			}
		}

		public IContainer Container => _container;


	    public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		#endregion

		#region Overrides of IocCoreBase

        protected override IGeneralUnitOfWorkFactory GetInstanceOfIGeneralUnitOfWorkFactory(IComponentContext arg)
		{
            return new MongoConnectionFactory(Dal.Mongo.Properties.Settings.Default.Connection);
		}

		#endregion

	}
}