using System;
using System.Linq;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class EnumHelperTests
    {
        [Test]
        public void method_GiventestingFor_Shouldresult()
        {
            // arrange
            Setup();
            // action
            var dayOfWeeks = EnumHelper.ToArray<DayOfWeek>();
            // assert
            dayOfWeeks.Should().Contain(DayOfWeek.Friday).And.HaveCount(7);
        }

        private void Setup()
        {
            
        }
    }

}