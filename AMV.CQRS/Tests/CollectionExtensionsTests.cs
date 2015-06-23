using System;
using System.Collections.Generic;
using AMV.Helpers;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void AddIfNotNull_value_adds()
        {
            var sut = new List<String>() { "one", "two" };

            sut.AddIfNotNull("three");

            sut.Should().BeEquivalentTo(new List<String>() { "one", "two", "three" });
        }

        [Fact]
        public void AddIfNotNull_Null_DoesNotAdd()
        {
            var sut = new List<String>() { "one", "two" };

            string addition = null;

            sut.AddIfNotNull(addition);

            sut.Should().BeEquivalentTo(new List<String>() { "one", "two" });
        }


        [Fact]
        public void AddRange_GivenList_AddsAllItems()
        {
            // Arrange
            var sut = new List<String>() { "one", "two" };
            var addition = new List<String>() { "three", "four" };

            // Act
            sut.AddRange(addition);

            // Assert
            sut.Should().BeEquivalentTo(new List<String>() { "one", "two", "three", "four" });
        }
    }
}
