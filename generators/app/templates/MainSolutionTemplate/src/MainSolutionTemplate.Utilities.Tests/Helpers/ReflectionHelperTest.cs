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
            var type = ReflectionHelper.FindOfType(typeof(ReflectionHelperTest).Assembly, "ReflectionHelperTest");
            // assert
            type.Should().Be(typeof (ReflectionHelperTest));
        }
         
        [Test]
        public void CreateGenericType_GivenCreateGenericType_Should()
        {
            // arrange
            var genericType = typeof(Task<>);
            var ofType = ReflectionHelper.FindOfType(typeof(ReflectionHelperTest).Assembly, "ReflectionHelperTest");// action
            var type = ReflectionHelper.MakeGenericType(genericType, ofType);
            // assert
            type.Should().Be(typeof(Task<ReflectionHelperTest>));
        }

        [Test]
        public void GetMember_GivenExpression_ShouldReturnValue()
        {
            // arrange
            var member = ReflectionHelper.GetPropertyInfo<User,Guid>(x => x.Id);
            // assert
            member.Name.Should().Be("Id");
            member.PropertyType.Name.Should().Be("Guid");
        }


        [Test]
        public void GetMemberString_GivenExpression_ShouldReturnValue()
        {
            // assert
            ReflectionHelper.GetPropertyString<User, MyClass>(x => x.Cl).Should().Be("Cl");
            ReflectionHelper.GetPropertyString<User, string>(x => x.Cl.S1).Should().Be("Cl.S1");
            ReflectionHelper.GetPropertyString<User, string>(x => x.Cl.Cl.S1).Should().Be("Cl.Cl.S1");
        }


        class User
        {
            public Guid Id { get; set; }
            public MyClass Cl { get; set; }    
        }

        class MyClass
        {

            public string S1 { get; set; }
            public MyClass Cl { get; set; } 
        }
    }

    
}