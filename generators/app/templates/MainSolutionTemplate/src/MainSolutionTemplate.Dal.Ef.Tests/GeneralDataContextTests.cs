﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using MainSolutionTemplate.Dal.Models;
using NUnit.Framework;
using FluentAssertions;
using log4net;

namespace MainSolutionTemplate.Dal.Ef.Tests
{
	[TestFixture]
	public class GeneralDataContextTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private GeneralDataContext _dataContext;

		#region Setup/Teardown

		public void Setup()
		{
			//Database.SetInitializer(new MigrateDatabaseToLatestVersion<GeneralDataContext, Migrations.Configuration>()); 
			var type = typeof (System.Data.Entity.SqlServer.SqlProviderServices);
			_log.Info("Sneaky work around for getting the SqlServer included in ncrunch tests:" + type);
			_dataContext = new GeneralDataContext();
		}

		[TearDown]
		public void TearDown()
		{
			_dataContext.Dispose();
		}

		#endregion

		[Test]
		public void Users_GivenGradCommands_ShouldAddListAndDeleteUsers()
		{
			// arrange
			Setup();
			var newGuid = Guid.NewGuid();
			//action
			var user = Builder<User>.CreateNew().With(x => x.Id = newGuid).With(x => x.Name = GetRandom.String(30))
				
				.Build();
			user.Roles.Add(new Role() { Name = GetRandom.String(30) });
			
			_dataContext.Users.Add(user);
			_dataContext.SaveChanges();
			_dataContext.Users.Any(x => x.Name == user.Name).Should().BeTrue();
			foreach (var userToRemove in _dataContext.Users.Where(x => x.Name == user.Name))
			{
				_dataContext.Users.Remove(userToRemove);
			}
			_dataContext.SaveChanges();
			// assert
			_dataContext.Users.Any(x => x.Name == user.Name).Should().BeFalse();
		}


	}

}    