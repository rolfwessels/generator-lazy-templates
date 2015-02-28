using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Interfaces;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
    public static class FakeRepoHelper
    {
        public static IList<T> AddFake<T>(this IRepository<T> repository, int size = 5) where T : IBaseDalModel
        {
            var repoItems = Builder<T>.CreateListOfSize(size).Build();
            foreach (var item in repoItems)
            {
                var user = item as User;
                if (user != null) user.Email = GetRandom.Email().ToLower();
                repository.Add(item);
            }
            return repoItems;
        }
    }
}