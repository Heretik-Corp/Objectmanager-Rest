using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using Relativity.API;
using Xunit;

namespace ObjectManager.Rest.Tests.Integration.Common.Extensions
{
    public static class ObjectManagerTestExtensions
    {
        public static async Task UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Multichoice);

            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;
            var choice1 = client.Repositories.Choice.ReadSingle(Guid.Parse(MultiChoiceChoiceDefinitions.Multi1));
            var choice2 = client.Repositories.Choice.ReadSingle(Guid.Parse(MultiChoiceChoiceDefinitions.Multi2));

            //ACT
            var value = new List<ChoiceRef> {
                new ChoiceRef(choice1.ArtifactID),
                new ChoiceRef(choice2.ArtifactID)
            };

            var (uResult, result) = await SharedTestCases.RunUpateTestAsync(manager,
                workspaceId,
                docId,
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(choice1.ArtifactID, result[fieldGuid].ValueAsMultiChoice().First().ArtifactId);
            Assert.Equal(choice2.ArtifactID, result[fieldGuid].ValueAsMultiChoice().Last().ArtifactId);
        }

        public static async Task UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceGuid_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Multichoice);

            //ACT
            var value = new List<ChoiceRef> {
                new ChoiceRef(Guid.Parse(MultiChoiceChoiceDefinitions.Multi1)),
                new ChoiceRef(Guid.Parse(MultiChoiceChoiceDefinitions.Multi2))
            };
            var (uResult, result) = await SharedTestCases.RunUpateTestAsync(manager,
                    workspaceId,
                    docId,
                    new FieldRef(fieldGuid),
                    value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(value.First().Guids.First(), result[fieldGuid].ValueAsMultiChoice().First().Guids.First());
            Assert.Equal(value.Last().Guids.First(), result[fieldGuid].ValueAsMultiChoice().Last().Guids.First());
        }

        #region CallingContext

        public static async Task UpdateAsync_CallingContextArtifactIdSet_ReturnsCorrectStatus(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.FixedLength);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;

            var query = new Query<Layout>();
            query.Condition = new TextCondition(LayoutFieldNames.TextIdentifier, TextConditionEnum.EqualTo, "Default Test Layout");
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query).Results.First().Artifact;

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                "hello world");

            var result = await manager.UpdateAsync(workspaceId, obj, new Interfaces.CallingContext
            {
                Layout = new Interfaces.LayoutRef(layout.Name, layout.ArtifactID)

            });

            //ASSERT
            Assert.True(result.EventHandlerStatuses.All(x => x.Success));
        }

        public static async Task<ObjectUpdateResult> UpdateAsync_CallingContextSetLayoutHasEventhandlerError_ReturnsCorrectStatus(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.YesNo);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;

            var query = new Query<Layout>();
            query.Condition = new TextCondition(LayoutFieldNames.TextIdentifier, TextConditionEnum.EqualTo, "Layout with eventhandler");
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query).Results.First().Artifact;

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);

            var result = await manager.UpdateAsync(workspaceId, obj, new Interfaces.CallingContext
            {
                Layout = new Interfaces.LayoutRef(layout.Name, layout.ArtifactID)
            });

            return result;

        }

        public static async Task<RelativityObject> ReadAsync_CallingContextSetLayoutHasPreload_ReturnsCorrectLoadedFields(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.LongText);

            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;

            var query = new Query<Layout>();
            query.Condition = new TextCondition(LayoutFieldNames.TextIdentifier, TextConditionEnum.EqualTo, "Test Preload");
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query).Results.First().Artifact;

            var obj = SharedTestCases.CreateTestObject(
                docId,
                new FieldRef(fieldGuid),
                true);

            var result = await manager.ReadAsync(workspaceId, obj, new Interfaces.CallingContext
            {
                Layout = new Interfaces.LayoutRef(layout.Name, layout.ArtifactID)
            });

            return result;
        }

        #endregion

    }
}
