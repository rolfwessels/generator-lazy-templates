using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests.Persistence
{
    [TestFixture]
    public class ProjectCollectionTests
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
        public void Users_GivenCrudCommands_ShouldAddListAndDeleteRecords()
        {
            // arrange
            Setup();;
            using (var dataContext = MongoConnectionFactory.New)
            {
                var project = Builder<Project>.CreateNew().WithValidData().Build();
                var persistanceTester = new PersistanceTester<Project>(dataContext, work => work.Projects);
                persistanceTester.ValueValidate(x => x.Name, project.Name, GetRandom.String(30));
                persistanceTester.ValidateCrud(project).Wait();
            }
        }


    }
}