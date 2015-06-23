using System;
using AMV.Helpers;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace Tests
{
    public class ObjectExtensionsTests
    {
        private IFixture fixture;

        public ObjectExtensionsTests()
        {
            fixture = new Fixture();
        }


        private class TestObject
        {
            public String SomeString { get; set; }
            public int? NullableInt { get; set; }
        }

        [Fact]
        public void CheckForNull_NullObject_ReturnsEmptyString()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = testObject.CheckForNull(t => t.SomeString);

            // Assert
            result.Should().BeEmpty();
        }


        [Fact]
        public void CheckForNull_SomeObject_ReturnsString()
        {
            //Arrange
            var testObject = fixture.Create<TestObject>();

            // Act
            var result = testObject.CheckForNull(t => t.SomeString);

            // Assert
            //Assert.AreEqual(testObject.SomeString, result);
            result.Should().Be(testObject.SomeString);
        }


        [Fact]
        public void CheckForNull_NullObjectForNullableInt_ReturnsNull()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = testObject.CheckForNull(t => t.NullableInt);

            // Assert
            result.Should().Be(null);
        }


        [Fact]
        public void CheckForNull_IntegerForNullableInt_ReturnsNullableInt()
        {
            //Arrange
            var testObject = new TestObject()
            {
                NullableInt = 3,
            };

            // Act
            int? result = testObject.CheckForNull(t => t.NullableInt);

            // Assert
            result.Should().Be(3);
        }


        // ReSharper disable ExpressionIsAlwaysNull
        [Fact]
        public void CheckForNull_DefaultObject_ReturnsProvidedObject()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            int? result = testObject.CheckForNull(t => t.NullableInt, 42);

            // Assert
            result.Should().Be(42);
        }

        [Fact]
        public void CheckForNull_DefaultObject_ReturnsProvidedNull()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            var result = testObject.CheckForNull(t => t.NullableInt, null);

            // Assert
            result.Should().Be(null);
        }


        [Fact]
        public void CheckForNull_NonNullWithDefaultObject_ReturnsProvidedValue()
        {
            //Arrange
            var testObject = new TestObject()
            {
                NullableInt = 3,
            };

            // Act
            int? result = testObject.CheckForNull(t => t.NullableInt, 42);

            // Assert
            result.Should().Be(3);
        }



        [Fact]
        public void IfNotNull_Null_DoesNothing()
        {
            //Arrange
            TestObject testObject = null;

            var result = new object();

            // Act
            testObject.IfNotNull(o => result = null);

            // Assert
            result.Should().NotBeNull();
        }
        // ReSharper restore ExpressionIsAlwaysNull


        [Fact]
        public void IfNotNull_NotNull_ExecutesTheFunction()
        {
            //Arrange
            var testObject = new TestObject();

            var result = new object();
            // Act
            testObject.IfNotNull(t => result = null);

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public void IsNumeric_Null_ReturnsFalse()
        {
            String o = null;
            var result = o.IsNumeric();

            result.Should().BeFalse();
        }


        [Fact]
        public void IsNumeric_SomeString_ReturnsFalse()
        {
            var o = fixture.Create<String>();
            var result = o.IsNumeric();

            result.Should().BeFalse();
        }


        [Fact]
        public void IsNumeric_SomeNumber_ReturnsTrue()
        {
            var o = fixture.Create<int>();
            var result = o.IsNumeric();

            result.Should().BeTrue();
        }


        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1.1")]
        [InlineData("-1.1")]
        [InlineData("-0.000009")]
        [InlineData("0.000009")]
        [InlineData("32767")]
        [InlineData("32768")]
        [InlineData("-32768")]
        [InlineData("-32769")]
        [InlineData("2,147,483,647")]
        [InlineData("2,147,483,648")]
        [InlineData("-2,147,483,648")]
        [InlineData("-2,147,483,649")]
        [InlineData("9,223,372,036,854,775,807")]
        [InlineData("9,223,372,036,854,775,808")]
        [InlineData("19,223,372,036,854,775,808")]
        [InlineData("-19,223,372,036,854,775,809")]
        public void IsNumeric_StringNumber_ReturnsTrue(string number)
        {
            var result = number.IsNumeric();

            result.Should().BeTrue();
        }
    }
}
