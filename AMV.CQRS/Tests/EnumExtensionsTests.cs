using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AMV.Helpers;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class EnumExtensionsTests
    {
        public enum EnumWithDisplayNames
        {
            [Description("Some Description")]
            WithDescription,

            [Display(Name = "Some Display Name")]
            WithDisplay,

            ToSeparateWords,
            Oneword,
        }


        [Theory]
        [InlineData(EnumWithDisplayNames.WithDescription, "Some Description")]
        [InlineData(EnumWithDisplayNames.WithDisplay, "Some Display Name")]
        [InlineData(EnumWithDisplayNames.ToSeparateWords, "To Separate Words")]
        [InlineData(EnumWithDisplayNames.Oneword, "Oneword")]
        public void GetDisplayName_DescriptionAttribute_TakesValueFromDescription(EnumWithDisplayNames enumValue, String expected)
        {
            var result = enumValue.GetDisplayName();

            result.Should().Be(expected);
        }
    }
}
