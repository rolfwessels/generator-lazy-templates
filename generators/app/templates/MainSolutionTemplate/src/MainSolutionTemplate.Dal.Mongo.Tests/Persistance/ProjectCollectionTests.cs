using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Tests.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests.Persistance
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
        public void Users_GivenGrudCommands_ShouldAddListAndDeleteRecords()
        {
            // arrange
            Setup();
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