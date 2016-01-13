﻿using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
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
		public void Users_GivenGrudCommands_ShouldAddListAndDeleteRecords()
		{
			// arrange
			Setup();
			using (var dataContext = MongoConnectionFactory.New)
			{
				var user = Builder<User>.CreateNew().WithValidData().Build();
                user.Roles.Add(RoleManager.Guest.Name);
				var persistanceTester = new PersistanceTester<User>(dataContext,work => work.Users);
				persistanceTester.ValueValidate(x => x.Name, user.Name, GetRandom.String(30));
                persistanceTester.ValidateCrud(user).Wait();
			}
		}


	}
}    