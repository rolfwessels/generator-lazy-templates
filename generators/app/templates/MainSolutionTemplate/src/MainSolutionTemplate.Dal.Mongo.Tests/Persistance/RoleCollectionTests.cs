using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Tests.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests.Persistance
{
    [TestFixture]
    public class RoleCollectionTests
    {
	
        #region Setup/Teardown

        public void Setup()
        {
			
        }

        [TearDown]
        public void TearDown()
        {
			
        }

        #endregion


        [Test]
        public void Roles_GivenCrudCommands_ShouldAddListAndDeleteRecords()
        {
            // arrange
            Setup();
            //action
            var role = Builder<Role>.CreateNew().WithValidData().Build();
            using (var dataContext = MongoDbConnectionFactory.New)
            {
                var persistanceTester = new PersistanceTester<Role>(dataContext, work => work.Roles);
                persistanceTester.ValueValidate(x => x.Name, role.Name, GetRandom.String(30));
                persistanceTester.ValidateCrud(role);
            }
        }



    }
}