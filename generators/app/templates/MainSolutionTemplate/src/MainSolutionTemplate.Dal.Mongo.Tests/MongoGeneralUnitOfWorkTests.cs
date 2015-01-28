﻿using System;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests
{
	[TestFixture]
	public class MongoGeneralUnitOfWorkTests
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
			using (var dataContext = new MongoGeneralUnitOfWork())
			{
				var user = Builder<User>.CreateNew().With(x => x.Name = GetRandom.String(30))
				.Build();
				user.Roles.Add(new Role { Name = GetRandom.String(30) });
				var persistanceTester = new PersistanceTester<User>(dataContext,work => work.Users);
				persistanceTester.ValidateCrud(user);
			}
		}

		[Test]
		public void Roles_GivenGrudCommands_ShouldAddListAndDeleteRecords()
		{
			// arrange
			Setup();
			var newGuid = Guid.NewGuid();
			//action
			var role = Builder<Role>.CreateNew().With(x => x.Id = newGuid).With(x => x.Name = GetRandom.String(30)).Build();
			using (var dataContext = new MongoGeneralUnitOfWork())
			{
				var persistanceTester = new PersistanceTester<Role>(dataContext, work => work.Roles);
				persistanceTester.ValidateCrud(role);
			}
		}

		


	}
}    