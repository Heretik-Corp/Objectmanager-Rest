using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces.Models;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.Common.Tests.Unit.Extensions
{
    [UnitTest]
    public class FieldValuePairExtensionsTests
    {
        [Fact]
        public void ValueAsSingleChoice_ValueIsNull_ReturnsNull()
        {
            //ARRANGE
            var pair = new FieldValuePair();
            pair.Value = null;

            //ACT
            var result = FieldValuePairExtensions.ValueAsSingleChoice(pair);

            //ASSERT
            Assert.Null(result);
        }

        [Fact]
        public void ValueAsSingleChoice_ValueIsNotNull_ReturnsCorrectField()
        {
            //ARRANGE
            var pair = new FieldValuePair();
            pair.Value = Newtonsoft.Json.JsonConvert.SerializeObject(new ChoiceRef(123));

            //ACT
            var result = FieldValuePairExtensions.ValueAsSingleChoice(pair);

            //ASSERT
            Assert.NotNull(result);
            Assert.Equal(123, result.ArtifactId);
        }
    }
}
