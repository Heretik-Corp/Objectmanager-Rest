using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using Relativity.API;
using Xunit;

namespace ObjectManager.Rest.Tests.Integration.Common.Extensions
{
    public static class ToDTOExtensionTests
    {
        public static async Task ToDTO_FieldIsFixedLength_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.FixedLength);
            var client = helper.GetRSAPIClient(workspaceId);
            client.Repositories.Document.UpdateSingle(new kCura.Relativity.Client.DTOs.Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, "hello world")
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var text = dto[fieldGuid].ValueAsFixedLengthText;

            //ASSERT
            Assert.Equal("hello world", text);
            Assert.Equal(kCura.Relativity.Client.FieldType.FixedLengthText, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsLongText_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.LongText);
            var client = helper.GetRSAPIClient(workspaceId);
            client.Repositories.Document.UpdateSingle(new kCura.Relativity.Client.DTOs.Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, "hello world")
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsFixedLengthText;

            //ASSERT
            Assert.Equal("hello world", fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.LongText, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsYesNo_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.YesNo);
            var client = helper.GetRSAPIClient(workspaceId);
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, true)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsYesNo;

            //ASSERT
            Assert.True(fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.YesNo, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsSingleChoice_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleChoice);
            var client = helper.GetRSAPIClient(workspaceId);
            var choice = client.Repositories.Choice.ReadSingle(Guid.Parse(SingleChoiceChoiceDefinitions.Single1));
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, choice)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsSingleChoice;

            //ASSERT
            Assert.Equal(choice.ArtifactID, fieldValue.ArtifactID);
            Assert.Equal(kCura.Relativity.Client.FieldType.SingleChoice, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsMultiChoice_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Multichoice);
            var client = helper.GetRSAPIClient(workspaceId);
            var choice = client.Repositories.Choice.ReadSingle(Guid.Parse(MultiChoiceChoiceDefinitions.Multi1));
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, new MultiChoiceFieldValueList(choice))
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsMultipleChoice.First();

            //ASSERT
            Assert.Equal(choice.ArtifactID, fieldValue.ArtifactID);
            Assert.Equal(kCura.Relativity.Client.FieldType.MultipleChoice, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsCurrency_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Currency);
            var client = helper.GetRSAPIClient(workspaceId);
            decimal value = 3;
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, value)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsCurrency;

            //ASSERT
            Assert.Equal(value, fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.Currency, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsDecimal_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Decimal);
            var client = helper.GetRSAPIClient(workspaceId);
            decimal value = 3;
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, value)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsDecimal;

            //ASSERT
            Assert.Equal(value, fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.Decimal, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsWholeNumber_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.WholeNumber);
            var client = helper.GetRSAPIClient(workspaceId);
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, 3)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsWholeNumber;

            //ASSERT
            Assert.Equal(3, fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.WholeNumber, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsDate_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Date);
            var date = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
            var client = helper.GetRSAPIClient(workspaceId);
            client.Repositories.Document.UpdateSingle(new kCura.Relativity.Client.DTOs.Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, date)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsDate;

            //ASSERT
            Assert.Equal(date, fieldValue);
            Assert.Equal(kCura.Relativity.Client.FieldType.Date, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsSingleObject_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleObject);

            var client = helper.GetRSAPIClient(workspaceId);
            var rdo = client.Repositories.RDO.CreateSingle(new RDO
            {
                ArtifactTypeGuids = new List<Guid> { Guid.Parse(ObjectTypeGuids.SingleObject) },
                TextIdentifier = Guid.NewGuid().ToString()
            });
            var value = new RDO(rdo);
            client.Repositories.Document.UpdateSingle(new kCura.Relativity.Client.DTOs.Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, value)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsSingleObject;

            //ASSERT
            Assert.Equal(value.ArtifactID, fieldValue.ArtifactID);
            Assert.Equal(kCura.Relativity.Client.FieldType.SingleObject, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsMultiObject_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.MultiObject);

            var client = helper.GetRSAPIClient(workspaceId);
            var rdo = client.Repositories.RDO.CreateSingle(new RDO
            {
                ArtifactTypeGuids = new List<Guid> { Guid.Parse(ObjectTypeGuids.MultiObject) },
                TextIdentifier = Guid.NewGuid().ToString()
            });
            var value = new RDO(rdo);
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, new FieldValueList<Artifact>(value))
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].GetValueAsMultipleObject<Artifact>().First();

            //ASSERT
            Assert.Equal(value.ArtifactID, fieldValue.ArtifactID);
            Assert.Equal(kCura.Relativity.Client.FieldType.MultipleObject, dto[fieldGuid].FieldType);
        }

        public static async Task ToDTO_FieldIsUser_ReturnsCorrectValue(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.User);

            var client = helper.GetRSAPIClient(workspaceId);
            client.APIOptions.WorkspaceID = -1;

            var q = new Query<Group>();
            q.ArtifactTypeID = (int)kCura.Relativity.Client.ArtifactType.Group;
            q.Condition = new kCura.Relativity.Client.CompositeCondition(new kCura.Relativity.Client.WholeNumberCondition(GroupFieldNames.GroupType, kCura.Relativity.Client.NumericConditionEnum.EqualTo, 2),
                                                                       kCura.Relativity.Client.CompositeConditionEnum.And,
                                                                      new kCura.Relativity.Client.ObjectsCondition(GroupFieldNames.Workspaces, kCura.Relativity.Client.ObjectsConditionEnum.AnyOfThese, new int[] { workspaceId }));
            var res = client.Repositories.Group.Query(q);

            var userQ = new Query<kCura.Relativity.Client.DTOs.User>();
            userQ.Condition = new kCura.Relativity.Client.ObjectsCondition(UserFieldNames.Groups, kCura.Relativity.Client.ObjectsConditionEnum.AnyOfThese, res.Results.Select(x => x.Artifact.ArtifactID).ToList());
            var userRes = client.Repositories.User.Query(userQ);
            if (userRes.TotalCount < 1)
            {
                //ideally you'd create a user in this but that's too much for the scope of this work
                throw new NotSupportedException("No user to run this test, please create a non system admin and ensure the group is assigned to the workspace.");
            }
            var value = userRes.Results.First().Artifact;
            client.APIOptions.WorkspaceID = workspaceId;
            client.Repositories.Document.UpdateSingle(new Document(docId)
            {
                Fields = new List<FieldValue>
                {
                    new FieldValue(fieldGuid, value)
                }
            });

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            var dto = result.ToDTODocument();
            var fieldValue = dto[fieldGuid].ValueAsUser;

            //ASSERT
            Assert.Equal(value.ArtifactID, fieldValue.ArtifactID);
            Assert.Equal(kCura.Relativity.Client.FieldType.User, dto[fieldGuid].FieldType);
        }
    }
}
