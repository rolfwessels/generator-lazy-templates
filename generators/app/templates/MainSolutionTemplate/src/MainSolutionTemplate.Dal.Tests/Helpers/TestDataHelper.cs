using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static ISingleObjectBuilder<T> WithValidData<T>(this ISingleObjectBuilder<T> createNew)
        {
            createNew.With(WithValidDataAdded);
            return createNew;
        }

        private static T WithValidDataAdded<T>(T buildingObject)
        {
            var user = buildingObject as User;
            if (user != null)
            {
                user.HashedPassword = GetRandom.String(123);
                user.Email = GetRandom.Email();
            }
            return buildingObject;
        }
    }
}