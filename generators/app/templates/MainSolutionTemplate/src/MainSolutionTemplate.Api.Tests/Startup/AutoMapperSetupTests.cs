using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Mappers;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Startup
{
    [TestFixture]
    public class AutoMapperSetupTests
    {
        private readonly IEnumerable<Type> _types;

        [Test]
        public void AutoMapperSetup_ConstructAll_ShouldNotBeNull()
        {
            // assert
            _types.Select(x => x.TypeInitializer.Invoke(null, null)).Count().Should().BeGreaterThan(1);
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void AutoMapperSetup_WhenConstructed_ShouldContainAutoMapperUserModel()
        {
            // assert
            _types.Select(x => x.Name).Should().Contain("MapApi");
        }

        [Test]
        public void AutoMapperSetup_WhenConstructed_ShouldNotBeNull()
        {
            // assert
            _types.Select(x => x.Name).Should().Contain("MapCore");
        }

        public AutoMapperSetupTests()
        {
            _types = new[] {typeof (MapCore), typeof (MapApi)};
        }
    }
}