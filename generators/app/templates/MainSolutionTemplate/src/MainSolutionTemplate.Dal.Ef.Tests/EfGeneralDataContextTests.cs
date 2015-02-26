﻿using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Dal.Ef.Tests
{
	[TestFixture]
	public class EfGeneralDataContextTests
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
			using (var dataContext = new EfGeneralUnitOfWork())
			{
				var user = Builder<User>.CreateNew().With(x => x.Name = GetRandom.String(30))
                    .With(x=>x.LastLoginDate = DateTime.Now)
				.Build();
				user.Roles.Add(new Role { Name = GetRandom.String(30) });
				var persistanceTester = new PersistanceTester<User>(dataContext, work => work.Users);
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
			using (var dataContext = new EfGeneralUnitOfWork())
			{
				var persistanceTester = new PersistanceTester<Role>(dataContext, work => work.Roles);
				persistanceTester.ValueValidate(x => x.Name, role.Name, "NewRole123");
				persistanceTester.ValidateCrud(role);
			}
		}

		

		[Test]
		public void Users_GivenLazyLoadingExample_ShouldWork()
		{
			// arrange
			Setup();
			
			//action
			Guid? guid = null;
			using (var dataContext = new EfGeneralUnitOfWork())
			{
				var user = Builder<User>.CreateNew().With(x => x.Name = GetRandom.String(30)).Build();
				user.Roles.Add(new Role { Name = GetRandom.String(30) });
				dataContext.Users.Add(user);
				guid = user.Id;
			}
			// assert
			User userLoaded;
			string name = "NewName" + GetRandom.String(3);
			using (var dataContext = new EfGeneralUnitOfWork())
			{
				userLoaded = dataContext.Users.First(x => x.Id == guid.Value);
				userLoaded.Name = name;
				dataContext.Users.Update(userLoaded);
				
			}

			using (var dataContext = new EfGeneralUnitOfWork())
			{
				userLoaded = dataContext.Users.First(x => x.Id == guid.Value);
				userLoaded.Name.Should().Be(name);
			}
			
			using (var dataContext = new EfGeneralUnitOfWork())
			{
				userLoaded = dataContext.Users.First(x => x.Id == guid.Value);
				userLoaded.Roles.Should().HaveCount(1);
			}
			userLoaded.Roles.Should().HaveCount(1);
			//dataContext.Users.Any(x => x.Name == user.Name).Should().BeFalse();
		}


	}

}    