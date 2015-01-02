using System.Collections.Generic;
using MainSolutionTemplate.Core.Helpers;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
	[TestFixture]
	public class IEnumerableTests
	{
		[Test]
		public void StringJoin_WhenCalledWithArrayOfStrings_ShouldJoinValues()
		{
			// arrange
			var values = new[] {"1", "2"};
			// action
			var stringJoin = values.StringJoin();
			// assert
			stringJoin.Should().Be("1, 2");
		}

		[Test]
		public void StringJoin_WhenCalledWithDifferenceSeparator_ShouldUseSeparator()
		{
			// arrange
			var values = new[] {"1", "2"};
			// action
			var stringJoin = values.StringJoin("-");
			// assert
			stringJoin.Should().Be("1-2");
		}
		
		[Test]
		public void StringJoin_WhenCalledWithOneValue_ShouldDisplayOnlyValue()
		{
			// arrange
			var values = new[] {"1"};
			// action
			var stringJoin = values.StringJoin("-");
			// assert
			stringJoin.Should().Be("1");
		}

		[Test]
		public void StringJoin_WhenCalledWithOneNull_ShouldReturnNull()
		{
			// arrange
			var values = null as IEnumerable<object>;
			// action
			var stringJoin = values.StringJoin("-");
			// assert
			stringJoin.Should().Be(null);
		}
	}
}