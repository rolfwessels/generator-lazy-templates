using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static ISingleObjectBuilder<T> WithValidData<T>(this ISingleObjectBuilder<T> createNew)
        {
            return createNew.With(WithValidDataAdded);
        }

        public static IOperable<T> WithValidData<T>(this IOperable<T> all)
        {
            return all.With(WithValidDataAdded);
        }

        private static T WithValidDataAdded<T>(T buildingObject)
        {
            var user = buildingObject as User;
            if (user != null)
            {
                user.HashedPassword = GetRandom.String(123);
                user.Email = GetRandom.Email().ToLower();
                user.Roles.Add(RoleManager.Guest.Name);
            }
            return buildingObject;
        }

        
    }
}