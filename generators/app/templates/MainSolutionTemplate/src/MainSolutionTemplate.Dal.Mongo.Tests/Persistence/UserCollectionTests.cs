using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.Mappers;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Utilities.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests.Persistence
{
    [TestFixture]
    public class UserCollectionTests
    {
        private IGeneralUnitOfWork _dataContext;

        #region Setup/Teardown

        public void Setup()
        {
            _dataContext = MongoConnectionFactory.New;
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void AddRange_GivenMultipleUsers_ShouldAddAllUsers()
        {
            // arrange
            Setup();
            var users = new[] {GetUser(), GetUser()};

            Task<IEnumerable<User>> addRange = _dataContext.Users.AddRange(users);
            IEnumerable<User> enumerable = addRange.Result;
            enumerable.Count().Should().Be(2);
            foreach (User added in enumerable)
            {
                _dataContext.Users.Remove(x => x.Id == added.Id).Result.Should().BeTrue();
            }
        }

        [Test]
        public void Count_GivenUserRepo_ShouldAlwaysHaveOneRecord()
        {
            // arrange
            Setup();

            _dataContext.Users.Count().Result.Should().BeGreaterThan(0);
        }


        [Test]
        public async Task Update()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            var l = await _dataContext.Users.Update(x => x.DefaultProject.Id == project.Id, x => x.DefaultProject, project.ToReference());
            _dataContext.Users.Count().Result.Should().BeGreaterThan(0);
        }


        [Test]
        public void Users_GivenCrudCommands_ShouldAddListAndDeleteRecords()
        {
            // arrange
            Setup();
            User user = GetUser();
            var persistenceTester = new PersistanceTester<User>(_dataContext, work => work.Users);
            persistenceTester.ValueValidate(x => x.Name, user.Name, GetRandom.String(30));
            persistenceTester.ValidateCrud(user).Wait();
        }


        


        #region Private Methods

        private static User GetUser()
        {
            User user = Builder<User>.CreateNew().WithValidData().Build();
            user.Roles.Add(RoleManager.Guest.Name);
            return user;
        }

        #endregion
    }
}