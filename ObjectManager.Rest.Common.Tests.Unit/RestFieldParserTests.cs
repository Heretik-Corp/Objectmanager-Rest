using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.Common.Tests.Unit
{
    [UnitTest]
    public class RestFieldParserTests
    {
        private readonly RestFieldParser _parser;

        public RestFieldParserTests()
        {
            _parser = new RestFieldParser();
        }

        [Fact]
        public void ParseValue_ValueIsNull_ReturnsCorrectValue()
        {
            //ARRANGE
            var choice = new ChoiceRef(123);

            //ACT
            var result = _parser.ParseValue(null);

            //ASSERT
            Assert.Null(result);
        }
        [Fact]
        public void ParseValue_ValueIsSingleChoiceWithArtifactId_ReturnsCorrectValue()
        {
            //ARRANGE
            var choice = new ChoiceRef(123);

            //ACT
            var result = _parser.ParseValue(choice);

            //ASSERT
            Assert.IsType<RChoice.ArtifactIdChoice>(result);
            Assert.Equal(123, ((RChoice.ArtifactIdChoice)result).ArtifactID);
        }

        [Fact]
        public void ParseValue_ValueIsSingleChoiceWithGuid_ReturnsCorrectValue()
        {
            //ARRANGE
            var g = Guid.NewGuid();
            var choice = new ChoiceRef(g);

            //ACT
            var result = _parser.ParseValue(choice);

            //ASSERT
            Assert.IsType<RChoice.GuidChoice>(result);
            Assert.Equal(g, ((RChoice.GuidChoice)result).Guid);
        }

        [Fact]
        public void ParseValue_ValueIsMultiChoice_ReturnsCorrectValue()
        {
            //ARRANGE
            var choice = new ChoiceRef(123);

            //ACT
            var result = _parser.ParseValue(new List<ChoiceRef> { choice });

            //ASSERT
            Assert.True(typeof(IEnumerable<RChoice>).IsAssignableFrom(result.GetType()));
            var rChoice = ((IEnumerable<RChoice>)result).Single();
            Assert.Equal(123, ((RChoice.ArtifactIdChoice)rChoice).ArtifactID);
        }

        [Fact]
        public void ParseValue_ValueIsDate_ReturnsCorrectValue()
        {
            //ARRANGE
            var date = DateTime.UtcNow;

            //ACT
            var result = _parser.ParseValue(date);

            //ASSERT
            Assert.Equal(date.ToString("yyyy-MM-ddTHH:mm:ss.ffZ"), result);
        }
    }
}
