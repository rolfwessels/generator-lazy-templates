using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
    public static class FakeRepoHelper
    {
        public static IList<T> AddFake<T>(this IRepository<T> repository, int size = 5) where T : IBaseDalModel
        {
            var repoItems = Builder<T>.CreateListOfSize(size).All().WithValidData().Build();
            foreach (var item in repoItems)
            {
                repository.Add(item);
            }
            return repoItems;
        }
        
        public static T AddAFake<T>(this IRepository<T> repository) where T : IBaseDalModel
        {
            return AddFake(repository, 1).First();
        }
    }
}