using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMV.Helpers;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData("2000/1/1", 13)]
        [InlineData("2000/7/1", 12)]
        [InlineData("2000/6/5", 12)]
        [InlineData("1985/8/23", 27)]
        [InlineData("1985/5/8", 28)]
        public void Age_Always_Correct(string dob, int expectedAge)
        {
            //Arrange
            TimeProvider.Current = new StubTimeProvider(new DateTime(2013, 6, 1));

            // Act
            var result = DateTime.Parse(dob).Age();

            // Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void Age_PassedNull_ReturnsNull()
        {
            //Arrange
            DateTime? dob = null;

            // Act
            var result = dob.Age();

            // Assert
            result.Should().Be(null);
        }
    }
}
