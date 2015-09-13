using System;
using System.Threading.Tasks;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class ReflectionHelperTest
    {

        [Test]
        public void FindOfType_GivenAssemblyAndName_ShouldSearchForATypeAndReturnIt()
        {
            // arrange
            // action
            var type = MyReflectionHelper.FindOfType(typeof(ReflectionHelperTest).Assembly, "ReflectionHelperTest");
            // assert
            type.Should().Be(typeof (ReflectionHelperTest));
        }
         
        [Test]
        public void CreateGenericType_GivenCreateGenericType_Should()
        {
            // arrange
            var genericType = typeof(Task<>);
            var ofType = MyReflectionHelper.FindOfType(typeof(ReflectionHelperTest).Assembly, "ReflectionHelperTest");// action
            var type = MyReflectionHelper.MakeGenericType(genericType, ofType);
            // assert
            type.Should().Be(typeof(Task<ReflectionHelperTest>));
        }
        
    }

    
}