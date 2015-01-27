﻿using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Dal.Ef.Tests
{
	[TestFixture]
	public class GeneralDataContextTests
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
			var newGuid = Guid.NewGuid();
			//action
			var user = Builder<User>.CreateNew().With(x => x.Id = newGuid).With(x => x.Name = GetRandom.String(30))
				.Build();
			user.Roles.Add(new Role { Name = GetRandom.String(30) });
			using (var dataContext = new GeneralUnitOfWork())
			{
				dataContext.Users.Add(user);
				dataContext.Commit();
				dataContext.Users.All.Any(x => x.Name == user.Name).Should().BeTrue();
				foreach (var userToRemove in dataContext.Users.All.Where(x => x.Name == user.Name))
				{
					dataContext.Users.Remove(userToRemove);
				}
				dataContext.Commit();
				// assert
				dataContext.Users.All.Any(x => x.Name == user.Name).Should().BeFalse();
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
			using (var dataContext = new GeneralUnitOfWork())
			{
				dataContext.Roles.Add(role);
				dataContext.Commit();
				dataContext.Roles.All.Any(x => x.Name == role.Name).Should().BeTrue();
				foreach (var userToRemove in dataContext.Roles.All.Where(x => x.Name == role.Name))
				{
					dataContext.Roles.Remove(userToRemove);
				}
				dataContext.Commit();
				// assert
				dataContext.Roles.All.Any(x => x.Name == role.Name).Should().BeFalse();
			}
		}

		[Test]
		public void Users_GivenLazyLoadingExample_ShouldWork()
		{
			// arrange
			Setup();
			
			//action

			Guid? guid = null;
			using (var dataContext = new GeneralUnitOfWork())
			{
				var user = Builder<User>.CreateNew().With(x => x.Name = GetRandom.String(30)).Build();
				user.Roles.Add(new Role { Name = GetRandom.String(30) });
				dataContext.Users.Add(user);
				dataContext.Commit();
				guid = user.Id;
			}
			// assert
			User userLoaded;
			using (var dataContext = new GeneralUnitOfWork())
			{
				userLoaded = dataContext.Users.All.First(x => x.Id == guid.Value);
				userLoaded.Roles.Should().HaveCount(1);
			}
			userLoaded.Roles.Should().HaveCount(1);
			//dataContext.Users.Any(x => x.Name == user.Name).Should().BeFalse();
		}


	}

}    