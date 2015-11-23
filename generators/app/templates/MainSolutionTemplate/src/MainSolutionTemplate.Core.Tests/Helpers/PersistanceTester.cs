using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using log4net;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
	public class PersistanceTester<T>  where T:IBaseDalModelWithId
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IGeneralUnitOfWork _unitOfWork;
		private readonly Func<IGeneralUnitOfWork, IRepository<T>> _repo;
		private readonly List<Action<T, T>> _testSaved = new List<Action<T, T>>();
		private readonly List<Action<T, T>> _testUpdated = new List<Action<T, T>>();

		public PersistanceTester(IGeneralUnitOfWork unitOfWork , Func<IGeneralUnitOfWork, IRepository<T>> repo )
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}

	    public async Task ValidateCrud(T user)
	    {
	        _log.Info(string.Format("Checking persistance of {0}", typeof (T)));
	        IRepository<T> repository = _repo(_unitOfWork);

            T findFirst = await repository.FindOne(x => x.Id == user.Id);
            findFirst.Should().BeNull("Could not load the value");
            T add = await repository.Add(user);
	        foreach (var action in _testSaved)
	        {
	            T firstOrDefault = await repository.FindOne(x => x.Id == user.Id);
	            firstOrDefault.Should().NotBeNull("Could not load the value");
	            action(user, firstOrDefault);
	        }
	        add.Should().NotBeNull("Saving should return the saved value");
	        var remove =  await repository.Remove(x => x.Id == add.Id);
	        remove.Should().BeTrue("Remove record should return true");

         

            T afterDelete = await repository.FindOne(x => x.Id == user.Id);
	        afterDelete.Should().BeNull("Item removed but could still be found");

            var removeTest = await repository.Remove(x => x.Id == add.Id);
            removeTest.Should().BeFalse("Remove record not be removed");

	    }

	    public void ValueValidate<TType>(Expression<Func<T, TType>> func, TType value, TType value2)
		{
			Func<T, TType> compile = func.Compile();
			_testSaved.Add((type,newValue) => compile(type).Should().Be(compile(type), string.Format("Original value for {0} not saved", func)));
			_testUpdated.Add((type, newValue) => compile(type).Should().Be(compile(type), string.Format("Original value for {0} not saved", func)));
		}
	}
}