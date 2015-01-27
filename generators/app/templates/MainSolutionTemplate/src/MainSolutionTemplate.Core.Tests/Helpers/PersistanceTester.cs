using System;
using System.Linq;
using FluentAssertions;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
	public class PersistanceTester<T>
	{
		private readonly IGeneralUnitOfWork _unitOfWork;
		private readonly Func<IGeneralUnitOfWork, IRepository<T>> _repo;

		public PersistanceTester(IGeneralUnitOfWork unitOfWork , Func<IGeneralUnitOfWork, IRepository<T>> repo )
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}

		public void ValidateCrud(T user)
		{
			IRepository<T> repository = _repo(_unitOfWork);
			int countOld = repository.Count();
			T add = repository.Add(user);
			add.Should().NotBeNull("Saving should return the saved value");
			int count = repository.Count();
			count.Should().Be(countOld + 1, "One record should be added");
			repository.Remove(add).Should().BeTrue("Remove record should return true");
			repository.Count().Should().Be(countOld, "One record should be removed");
		}
	}
}