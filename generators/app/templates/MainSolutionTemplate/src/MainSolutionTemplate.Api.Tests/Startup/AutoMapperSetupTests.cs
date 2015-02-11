using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Mappers;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Api.Tests.Startup
{
    [TestFixture]
    public class AutoMapperSetupTests
    {
	    private IEnumerable<Type> _types;

	    public AutoMapperSetupTests()
        {
	        _types = (from t in typeof (AutoMapperUser).Assembly.GetTypes()
					   where t.Name.Contains("AutoMapper") && t.IsSealed && t.IsAbstract
					  select t).Union((from t in typeof(AutoMapperUserModel).Assembly.GetTypes()
									   where t.Name.Contains("AutoMapper") && t.IsSealed && t.IsAbstract
									   select t))
			;
        }

	    [Test]
        public void AutoMapperSetup_WhenConstructed_ShouldNotBeNull()
        {
            // assert
			_types.Select(x => x.Name).Should().Contain("AutoMapperUser");
        }
[Test]
		public void AutoMapperSetup_WhenConstructed_ShouldContainAutoMapperUserModel()
        {
            // assert
			_types.Select(x => x.Name).Should().Contain("AutoMapperUserModel");
        }

	    [Test]
        public void AutoMapperSetup_ConstructAll_ShouldNotBeNull()
        {
            // assert
		    _types.Select(x => x.TypeInitializer.Invoke(null, null)).Count().Should().BeGreaterThan(1);
			Mapper.AssertConfigurationIsValid();
        }

    }

}