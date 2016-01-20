﻿using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests.Persistance
{
	[TestFixture]
	public class UserCollectionTests
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
			Setup();
			using (var dataContext = MongoConnectionFactory.New)
			{
				var user = GetUser();
			    var persistanceTester = new PersistanceTester<User>(dataContext,work => work.Users);
				persistanceTester.ValueValidate(x => x.Name, user.Name, GetRandom.String(30));
                persistanceTester.ValidateCrud(user).Wait();
			}
		}

	    private static User GetUser()
	    {
	        var user = Builder<User>.CreateNew().WithValidData().Build();
	        user.Roles.Add(RoleManager.Guest.Name);
	        return user;
	    }

	    [Test]
        public void Count_GivenUserRepo_ShouldAlwaysHaveOneRecord()
	    {
            // arrange
            Setup();
            using (var dataContext = MongoConnectionFactory.New)
            {
                dataContext.Users.Count().Result.Should().BeGreaterThan(0);
            }
	    }

	    [Test]
        public void AddRange_GivenMultipleUsers_ShouldAddAllUsers()
	    {
            // arrange
            Setup();
            var users = new[] { GetUser(), GetUser() };
            using (var dataContext = MongoConnectionFactory.New)
            {
                var addRange = dataContext.Users.AddRange(users);
                var enumerable = addRange.Result;
                enumerable.Count().Should().Be(2);
                foreach (var added in enumerable)
                {
                    dataContext.Users.Remove(x => x.Id == added.Id).Result.Should().BeTrue();
                }
                
            }
	    }



	}
}    