using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.Legacy.Tests.Unit
{
    [UnitTest]
    public class DTOExtensionsTests
    {
        #region DTOExtensionsTests
        [Fact]
        public void ToRelativityObject_FieldValueHasName_ReturnsNameSet()
        {
            //ARRANGE
            var doc = new kCura.Relativity.Client.DTOs.Document();
            doc.Fields = new System.Collections.Generic.List<kCura.Relativity.Client.DTOs.FieldValue>();
            doc.Fields.Add(new kCura.Relativity.Client.DTOs.FieldValue("field Name"));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.FirstOrDefault()?.Field);
            Assert.Equal("field Name", result.FieldValues.First().Field.Name);
        }

        [Fact]
        public void ToRelativityObject_ObjectArtifactIdSet_ReturnsCorrectArtifactIdSet()
        {
            //ARRANGE
            var doc = new kCura.Relativity.Client.DTOs.Document(123);

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.Equal(123, result.ArtifactId);
        }

        [Fact]
        public void ToRelativityObject_FieldValueHasValue_ReturnsValueSet()
        {
            //ARRANGE
            var doc = new kCura.Relativity.Client.DTOs.Document();
            doc.Fields = new System.Collections.Generic.List<kCura.Relativity.Client.DTOs.FieldValue>();
            doc.Fields.Add(new kCura.Relativity.Client.DTOs.FieldValue("field Name", "this field"));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.FirstOrDefault()?.Field);
            Assert.Equal("this field", result.FieldValues.First().Value);
        }

        [Fact]
        public void ToRelativityObject_FieldValueHasGuid_ReturnsGuidSet()
        {
            //ARRANGE
            var fieldGuid = Guid.NewGuid();
            var doc = new kCura.Relativity.Client.DTOs.Document();
            doc.Fields = new System.Collections.Generic.List<kCura.Relativity.Client.DTOs.FieldValue>();
            doc.Fields.Add(new kCura.Relativity.Client.DTOs.FieldValue(fieldGuid));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(fieldGuid, result.FieldValues.Single().Field.Guids.Single());
        }

        [Fact]
        public void ToRelativityObject_FieldValueHasArtifactId_ReturnsArtifactIdSet()
        {
            //ARRANGE
            var doc = new kCura.Relativity.Client.DTOs.Document();
            doc.Fields = new System.Collections.Generic.List<kCura.Relativity.Client.DTOs.FieldValue>();
            doc.Fields.Add(new kCura.Relativity.Client.DTOs.FieldValue(123));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
        }



        [Fact]
        public void ToRelativityObject_FieldValueIsChoiceWithArtifactId_ResturnsValueAsChoice()
        {
            //ARRANGE
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new Choice(456), true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal(456, result[123].ValueAsSingleChoice().ArtifactId);
            Assert.Null(result[123].ValueAsSingleChoice().Guids);
        }

        [Fact]
        public void ToRelativityObject_FieldValueIsChoiceWithGuid_ResturnsValueAsChoice()
        {
            //ARRANGE
            var choiceGuid = Guid.NewGuid();
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new Choice(choiceGuid), true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal(choiceGuid, result[123].ValueAsSingleChoice().Guids.First());
        }

        [Fact]
        public void ToRelativityObject_FieldValueIsChoiceWithName_ResturnsValueAsChoice()
        {
            //ARRANGE
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new Choice() { Name = "Choice Name" }, true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal("Choice Name", result[123].ValueAsSingleChoice().Name);
        }

        [Fact]
        public void ToRelativityObject_FieldValueIsMultiChoiceWithGuid_ResturnsValueAsChoice()
        {
            //ARRANGE
            var choiceGuid = Guid.NewGuid();
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new MultiChoiceFieldValueList(new Choice(choiceGuid)), true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal(choiceGuid, result[123].ValueAsMultiChoice().First().Guids.First());
        }

        [Fact]
        public void ToRelativityObject_FieldValueIsMultiChoiceWithName_ResturnsValueAsChoice()
        {
            //ARRANGE
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new MultiChoiceFieldValueList(new Choice() { Name = "Choice Name" }), true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal("Choice Name", result[123].ValueAsMultiChoice().First().Name);
        }


        [Fact]
        public void ToRelativityObject_FieldValueIsSingleObject_ResturnsValueAsRelativityObject()
        {
            //ARRANGE
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new Artifact(1234), true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal(1234, result[123].ValueAsSingleObject().ArtifactId);
        }

        [Fact]
        public void ToRelativityObject_FieldValueIsMultiObject_ResturnsValueAsRelativityObject()
        {
            //ARRANGE
            var doc = new Document();
            doc.Fields = new List<FieldValue>();
            doc.Fields.Add(new FieldValue(123, new List<Artifact> { new Artifact(1234) }, true));

            //ACT
            var result = doc.ToRelativityObject();

            //ASSERT
            Assert.NotEmpty(result.FieldValues);
            Assert.NotNull(result.FieldValues.SingleOrDefault()?.Field);
            Assert.Equal(123, result.FieldValues.Single().Field.ArtifactId);
            Assert.Equal(1234, result[123].ValueAsMultiObject().First().ArtifactId);
        }



        #endregion


        #region Single Object
        [Fact]
        public void ValueAsSingleObject_FieldValueIsSingleObject_ReturnsValueAsRelativityObject()
        {
            //ARRANGE
            var pair = new FieldValuePair();
            pair.Value = new RelativityObject
            {
                ArtifactId = 123
            };

            //ACT
            var r = pair.ValueAsSingleObject();

            //ASSERT
            Assert.IsType<RelativityObject>(r);
            Assert.Equal(123, r.ArtifactId);
        }

        [Fact]
        public void ValueAsSingleObject_FieldValueIsSingleObjectString_ReturnsValueAsRelativityObject()
        {
            //ARRANGE
            var pair = new FieldValuePair();
            pair.Value = @"{ 
                             ""ArtifactID"":123,
                             ""Guids"":[],
                             ""Name"":""Single 1""
                          }";

            //ACT
            var r = pair.ValueAsSingleObject();

            //ASSERT
            Assert.IsType<RelativityObject>(r);
            Assert.Equal(123, r.ArtifactId);
        }
        #endregion

        #region ToDTODocument
        [Fact]
        public void ToDTODocument_FieldValueHasName_ReturnsNameSet()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef("field Name")
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal("field Name", result.Fields.First().Name);
        }

        [Fact]
        public void ToDTODocument_FieldValueHasGuid_ReturnsGuidSet()
        {
            //ARRANGE
            var fieldGuid = Guid.NewGuid();
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(fieldGuid)
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal(fieldGuid, result.Fields.First().Guids.Single());
        }

        [Fact]
        public void ToDTODocument_FieldValueHasArtifactId_ReturnsArtifactIdSet()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123)
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal(123, result.Fields.First().ArtifactID);
        }

        [Fact]
        public void ToDTODocument_ObjectArtifactIdSet_ReturnsArtifactIdSet()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.ArtifactId = 123;

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.Equal(123, result.ArtifactID);
        }

        [Fact]
        public void ToDTODocument_FieldValueHasValue_ReturnsValueSet()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123),
                    Value = "this value"
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal("this value", result.Fields.First().Value);
        }

        [Fact]
        public void ToDTODocument_FieldValueIsChoiceWithArtifactId_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123),
                    Value = new ChoiceRef(456)
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal(123, result.Fields.First().ArtifactID);
            Assert.Equal(456, result.Fields.First().ValueAsSingleChoice.ArtifactID);
        }

        [Fact]
        public void ToDTODocument_FieldValueIsChoiceRefSerializedWithArtifactId_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123) { FieldType = "SingleChoice" },
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(new ChoiceRef(456))
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal(123, result.Fields.First().ArtifactID);
            Assert.Equal(456, result.Fields.First().ValueAsSingleChoice.ArtifactID);
        }

        [Fact]
        public void ToDTODocument_FieldValueIsChoiceWithGuid_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            var cGuid = Guid.NewGuid();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123),
                    Value = new ChoiceRef(cGuid)
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal(cGuid, result.Fields.First().ValueAsSingleChoice.Guids.First());
        }

        [Fact]
        public void ToDTODocument_FieldValueIsChoiceWithName_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            var cGuid = Guid.NewGuid();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123),
                    Value = new ChoiceRef("choice name")
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.Equal("choice name", result.Fields.First().ValueAsSingleChoice.Name);
        }


        [Theory]
        [InlineData("FixedLengthText", kCura.Relativity.Client.FieldType.FixedLengthText)]
        [InlineData("SingleChoice", kCura.Relativity.Client.FieldType.SingleChoice)]
        [InlineData("Date", kCura.Relativity.Client.FieldType.Date)]
        [InlineData("YesNo", kCura.Relativity.Client.FieldType.YesNo)]
        [InlineData("MultipleChoice", kCura.Relativity.Client.FieldType.MultipleChoice)]
        [InlineData("LongText", kCura.Relativity.Client.FieldType.LongText)]
        [InlineData("WholeNumber", kCura.Relativity.Client.FieldType.WholeNumber)]
        [InlineData("Decimal", kCura.Relativity.Client.FieldType.Decimal)]
        [InlineData("Currency", kCura.Relativity.Client.FieldType.Currency)]
        public void TODTODocument_FieldTypeIsPassedinValue_ReturnsCorrectValue(string type, kCura.Relativity.Client.FieldType expected)
        {
            //ARRANGE
            var doc = new RelativityObject();
            var cGuid = Guid.NewGuid();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123){
                        FieldType = type
                        },
                    Value = new List<ChoiceRef>{new ChoiceRef("choice name") }
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.Equal(expected, result.Fields.First().FieldType);

        }

        [Fact]
        public void ToDTODocument_FieldValueIsMultiChoiceWithName_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            var cGuid = Guid.NewGuid();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123),
                    Value = new List<ChoiceRef>{new ChoiceRef("choice name") }
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.IsType<MultiChoiceFieldValueList>(result.Fields.First().Value);
            Assert.Equal("choice name", result.Fields.First().ValueAsMultipleChoice.First().Name);
        }

        [Fact]
        public void ToDTODocument_FieldValueIsMultiChoiceSerializedWithName_ResturnsValueAsRelativityChoice()
        {
            //ARRANGE
            var doc = new RelativityObject();
            var cGuid = Guid.NewGuid();
            doc.FieldValues = new List<FieldValuePair>()
            {
                new FieldValuePair
                {
                    Field = new FieldRef(123) { FieldType = "MultipleChoice" },
                    Value = Newtonsoft.Json.JsonConvert.SerializeObject(new List<ChoiceRef>{new ChoiceRef("choice name") })
                }
            };

            //ACT
            var result = doc.ToDTODocument();

            //ASSERT
            Assert.NotEmpty(result.Fields);
            Assert.NotNull(result.Fields.FirstOrDefault());
            Assert.IsType<MultiChoiceFieldValueList>(result.Fields.First().Value);
            Assert.Equal("choice name", result.Fields.First().ValueAsMultipleChoice.First().Name);
        }



        #endregion
    }
}
