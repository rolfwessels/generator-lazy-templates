using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Models;

namespace MainSolutionTemplate.Api.Tests.Helper
{
    public static class TestDataHelper
    {
        public static ISingleObjectBuilder<T> WithValidModelData<T>(this ISingleObjectBuilder<T> createNew)
        {
            createNew.With(WithValidDataAdded);
            return createNew;
        }

        private static T WithValidDataAdded<T>(T buildingObject)
        {
            var user = buildingObject as UserDetailModel;
            if (user != null)
            {
                user.Email = GetRandom.Email();
            }
            return buildingObject;
        }
    }
}