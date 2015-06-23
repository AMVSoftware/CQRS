using System;
using System.Collections.Generic;
using AMV.Helpers;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void Merge_EmptyDictionary_AddsMergedElement()
        {
            //Arrange
            var sut = new Dictionary<String, object>();

            // Act
            sut.Merge("key", "Value");

            // Assert
            object mergedResult;
            var ok = sut.TryGetValue("key", out mergedResult);
            ok.Should().BeTrue();
            mergedResult.Should().Be("Value");
        }


        [Fact]
        public void Merge_DictionaryWithValues_AddsMergedElement()
        {
            //Arrange
            var sut = new Dictionary<String, object>();
            sut.Add("hello", new object());
            // Act
            sut.Merge("key", "Value");

            // Assert
            object mergedResult;
            var ok = sut.TryGetValue("key", out mergedResult);
            ok.Should().BeTrue();
            mergedResult.Should().Be("Value");
        }


        [Fact]
        public void Merge_DictionaryWithClashingKey_MergesElements()
        {
            //Arrange
            var sut = new Dictionary<String, object>();
            sut.Add("key", "Hello");

            // Act
            sut.Merge("key", "World");

            // Assert
            object mergedResult;
            var ok = sut.TryGetValue("key", out mergedResult);
            ok.Should().BeTrue();
            mergedResult.Should().Be("Hello World");
        }

        [Fact]
        public void MergeTwice_DictionaryWithClashingKey_MergesAllElements()
        {
            //Arrange
            var sut = new Dictionary<String, object>();
            sut.Add("key", "Hello");

            // Act
            sut.Merge("key", "Wicked");
            sut.Merge("key", "World");

            // Assert
            object mergedResult;
            var ok = sut.TryGetValue("key", out mergedResult);
            ok.Should().BeTrue();
            mergedResult.Should().Be("Hello Wicked World");
        }


        [Fact]
        public void AddOrReplace_NoValue_AddsNewElement()
        {
            //Arrange
            var sut = new Dictionary<String, String>();

            // Act
            sut.AddOrReplace("Key", "Value");

            // Assert
            sut.Should().Contain("Key", "Value");
        }


        [Fact]
        public void AddOrReplace_SomeValue_AddsNewElement()
        {
            //Arrange
            var sut = new Dictionary<String, String>();
            sut.Add("Hello", "World");

            // Act
            sut.AddOrReplace("Key", "Value");

            // Assert
            sut.Should().Contain("Key", "Value")
                .And.Contain("Hello", "World");
        }

        [Fact]
        public void AddOrReplace_ValueExists_ReplacesElement()
        {
            //Arrange
            var sut = new Dictionary<String, String>();
            sut.Add("Key", "World");

            // Act
            sut.AddOrReplace("Key", "Value");

            // Assert
            sut.Should().Contain("Key", "Value")
                .And.HaveCount(1);
        }
    }
}
