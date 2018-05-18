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
            Assert.IsType<RestFieldParser.MultipleChoiceFieldUpdateValue>(result);
            var choices = ((RestFieldParser.MultipleChoiceFieldUpdateValue)result).Choices.ToList();
            Assert.Equal(123, ((RChoice.ArtifactIdChoice)choices.First()).ArtifactID);
        }
        [Fact]
        public void ParseValue_ValueIsMultiChoice_ReturnsDefaultBehaviorCorrectValue()
        {
            //ARRANGE
            var choice = new ChoiceRef(123);

            //ACT
            var result = _parser.ParseValue(new List<ChoiceRef> { choice });

            //ASSERT
            Assert.IsType<RestFieldParser.MultipleChoiceFieldUpdateValue>(result);
            var r = (RestFieldParser.MultipleChoiceFieldUpdateValue)result;
            Assert.Equal(FieldUpdateBehavior.Replace, r.Behavior);
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
