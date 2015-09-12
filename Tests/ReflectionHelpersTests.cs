using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AMV.Helpers;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class ReflectionHelpersTests
    {
        [Fact]
        public void GetDescription_FieldNull_ReturnsEmptyString()
        {
            var result = ReflectionHelpers.GetDescription(null);

            result.Should().BeEmpty();
        }


        [Fact]
        public void GetDescription_WithDisplay_ReturnsDisplay()
        {
            var field = typeof(FieldTesting).GetProperty("WithDisplay");
            Assert.NotNull(field);

            var result = field.GetDescription();

            result.Should().Be("Display");
        }


        [Fact]
        public void GetDescriptionExpression_WithDisplay_ReturnsDisplay()
        {
            var param = new FieldTesting();

            var result = param.GetDescription(p => p.WithDisplay);

            result.Should().Be("Display");
        }


        [Fact]
        public void GetDescription_WithDescription_ReturnsDisplay()
        {
            var field = typeof(FieldTesting).GetProperty("WithDescription");
            Assert.NotNull(field);

            var result = field.GetDescription();

            result.Should().Be("Description");
        }


        [Fact]
        public void GetDescriptionExpression_WithDescription_ReturnsDisplay()
        {
            var param = new FieldTesting();

            var result = param.GetDescription(p => p.WithDescription);

            result.Should().Be("Description");
        }


        [Fact]
        public void GetDescription_EmptyString_ReturnsName()
        {
            var field = typeof(FieldTesting).GetProperty("EmptyString");
            Assert.NotNull(field);

            var result = field.GetDescription();

            result.Should().Be("Empty String");
        }


        [Fact]
        public void GetDescriptionExpression_EmptyString_ReturnsName()
        {
            var param = new FieldTesting();

            var result = param.GetDescription(p => p.EmptyString);

            result.Should().Be("Empty String");
        }


        [Fact]
        public void GetDescription_EmptyDescription_ReturnsPropertyName()
        {
            var field = typeof(FieldTesting).GetProperty("EmptyDescription");
            Assert.NotNull(field);

            var result = field.GetDescription();

            result.Should().Be("Empty Description");
        }

        [Fact]
        public void GetDescriptionExpression_EmptyDescription_ReturnsPropertyName()
        {
            var param = new FieldTesting();

            var result = param.GetDescription(p => p.EmptyDescription);

            result.Should().Be("Empty Description");
        }


        [Fact]
        public void GetDescription_EmptyDisplayName_ReturnsPropertyName()
        {
            var field = typeof(FieldTesting).GetProperty("EmptyDisplayName");
            Assert.NotNull(field);

            var result = field.GetDescription();

            result.Should().Be("Empty Display Name");
        }


        [Fact]
        public void GetDescriptionExpression_EmptyDisplayName_ReturnsPropertyName()
        {
            var param = new FieldTesting();

            var result = param.GetDescription(p => p.EmptyDisplayName);

            result.Should().Be("Empty Display Name");
        }


        [Fact]
        public void GetTypesInheritingFromType_Returns_FirstChild()
        {
            var assembly = typeof(ReflectionHelpersTests).Assembly;
            var result = assembly.GetTypesInheritingFromType(typeof(FieldTesting));

            result.Should().Contain(typeof(FieldTestingChild));
        }

        [Fact]
        public void GetTypesInheritingFromType_Returns_SecondChild()
        {
            var assembly = typeof(ReflectionHelpersTests).Assembly;
            var result = assembly.GetTypesInheritingFromType(typeof(FieldTesting));

            result.Should().Contain(typeof(SecondChild));
        }


        [Fact]
        public void GetDescriptionExpression_Should_Work_WithCollection()
        {
            var param = new FieldTestingChild
            {
                FieldTestings = new List<FieldTesting>()
                {
                    new FieldTestingChild(),
                    new FieldTestingChild(),
                    new FieldTestingChild(),
                    new FieldTestingChild(),
                }
            };

            var result = param.GetDescription(p => p.FieldTestings.First().WithDisplay);

            result.Should().Be("Display");
        }



        [Fact]
        public void IsNullableEnum_OnNullableEnum_ReturnsTrue()
        {
            var field = typeof(FieldTesting).GetProperty("NullableEnum");
            Assert.NotNull(field);

            var result = field.PropertyType.IsNullableEnum();

            result.Should().BeTrue();
        }


        [Fact]
        public void IsNullableEnum_OnString_ReturnsFalse()
        {
            var field = typeof(FieldTesting).GetProperty("EmptyDescription");
            Assert.NotNull(field);

            var result = field.PropertyType.IsNullableEnum();

            result.Should().BeFalse();
        }


        [Fact]
        public void IsNullableEnum_OnNullableInt_ReturnsFalse()
        {
            var field = typeof(FieldTesting).GetProperty("NullableInteger");
            Assert.NotNull(field);

            var result = field.PropertyType.IsNullableEnum();

            result.Should().BeFalse();
        }

        [Fact]
        public void IsNullableEnum_OnEnum_ReturnsFalse()
        {
            var field = typeof(FieldTesting).GetProperty("RegularEnum");
            Assert.NotNull(field);

            var result = field.PropertyType.IsNullableEnum();

            result.Should().BeFalse();
        }



        internal enum MyEnum
        {
            SomeValue = 2,
            SomeOtherValue = 33,
        }

        internal class FieldTesting
        {
            [Display(Name = "Display")]
            public String WithDisplay { get; set; }

            [Description("Description")]
            public String WithDescription { get; set; }

            public String EmptyString { get; set; }

            [Description("")]
            public String EmptyDescription { get; set; }

            public MyEnum RegularEnum { get; set; }

            public MyEnum? NullableEnum { get; set; }

            [Display(Name = "")]
            public String EmptyDisplayName { get; set; }

            public int? NullableInteger { get; set; }
        }


        internal class FieldTestingChild : FieldTesting
        {
            public String Name { get; set; }

            public List<FieldTesting> FieldTestings { get; set; }
        }

        internal class SecondChild : FieldTestingChild
        {
            public int SomeNumber { get; set; }
        }
    }
}
