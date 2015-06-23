using System;
using AMV.Helpers;
using Xunit;

namespace Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("FirstName", "First Name")]
        [InlineData("FirstLastWord", "First Last Word")]
        [InlineData("PersonId", "Person Id")]
        [InlineData("CRM", "CRM")]
        [InlineData("HR", "HR")]
        [InlineData("POName", "PO Name")]
        [InlineData("OnboardTeam", "Onboard Team")]
        [InlineData("XMLEditor", "XML Editor")]
        public void ToSeparateWords_Separates_Correctly(String source, String expected)
        {
            var result = source.ToSeparatedWords();

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("True")]
        [InlineData("true")]
        [InlineData("truE")]
        public void IsTrue_TrueString_CaseInsensitive(string value)
        {
            var result = value.IsTrue();
            Assert.True(result);
        }


        [Theory]
        [InlineData("false")]
        [InlineData("hello")]
        [InlineData("")]
        public void IsTrue_OtherString_ReturnsFalse(string value)
        {
            var result = value.IsTrue();
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Elipsis_NullOrEmpty_ReturnsEmptyString(string value)
        {
            var result = value.Elipsis(100);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("hello.?.")]
        [InlineData("")]
        public void Elipsis_UnderLength_ReturnsStringUnaltered(string value)
        {
            var result = value.Elipsis(100);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("Test Test. Test?", "Test Test...")]
        [InlineData("Test Test Test", "Test Test...")]
        [InlineData("Test Test Test", "Test Test...")]
        [InlineData("Test Test Test!", "Test Test...")]
        [InlineData("Hi. Is A Longword Now", "Hi. Is A...")]
        [InlineData("Test Test. Test! Hello. Hello. Hello!!!!", "Test Test...")]
        [InlineData("Disestablishmentarianism!", "Disestabli...")]
        public void Elipsis_OverLength_ReturnsStringWithEllipsis(string value, string expectedResult)
        {
            var result = value.Elipsis(10);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "", false)]
        [InlineData(null, "Anything", false)]
        [InlineData("Test", "", true)]
        [InlineData("Test", "Test", true)]
        [InlineData("Test", "Not", false)]
        public void Contains_WithRangeOfStrings_ValidatesOrdinalIgnoreCaseCorrectly(string value, string soughtValue, bool expected)
        {
            var result = value.Contains(soughtValue, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(expected, result);
        }
    }
}
