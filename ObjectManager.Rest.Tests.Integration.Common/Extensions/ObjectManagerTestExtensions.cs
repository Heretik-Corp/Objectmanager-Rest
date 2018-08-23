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
        public static async Task UpdateAsync_UpdateSingleObjectByArtifactId_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleObject);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;
            var obj = client.Repositories.RDO.CreateSingle(new RDO
            {
                ArtifactTypeGuids = new List<Guid> { Guid.Parse(ObjectTypeGuids.SingleObject) },
                TextIdentifier = Guid.NewGuid().ToString()
            });

            //ACT
            var value = new RelativityObject()
            {
                ArtifactId = obj
            };
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
                workspaceId,
                docId,
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(obj, result[fieldGuid].ValueAsSingleObject().ArtifactId);
        }

        public static async Task UpdateAsync_UpdateMultiObjectByArtifactId_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.MultiObject);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;
            var obj = client.Repositories.RDO.CreateSingle(new RDO
            {
                ArtifactTypeGuids = new List<Guid> { Guid.Parse(ObjectTypeGuids.MultiObject) },
                TextIdentifier = Guid.NewGuid().ToString()
            });
            var obj2 = client.Repositories.RDO.CreateSingle(new RDO
            {
                ArtifactTypeGuids = new List<Guid> { Guid.Parse(ObjectTypeGuids.MultiObject) },
                TextIdentifier = Guid.NewGuid().ToString()
            });

            //ACT
            var value = new List<RelativityObject>
            {
                new RelativityObject
                {
                    ArtifactId = obj
                },
                new RelativityObject
                {
                    ArtifactId = obj2
                }
            };
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
                workspaceId,
                docId,
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            var values = result[fieldGuid].ValueAsMultiObject().Select(x => x.ArtifactId).ToList();
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Contains(obj, values);
            Assert.Contains(obj2, values);
        }

        public static async Task UpdateAsync_UpdateUserByArtifactId_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId, string userName)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.User);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = -1;

            var q = new Query<Group>();
            q.ArtifactTypeID = (int)kCura.Relativity.Client.ArtifactType.Group;
            q.Fields = FieldValue.AllFields;
            q.Condition = new kCura.Relativity.Client.CompositeCondition(new kCura.Relativity.Client.WholeNumberCondition(GroupFieldNames.GroupType, kCura.Relativity.Client.NumericConditionEnum.EqualTo, 2),
                                                                       kCura.Relativity.Client.CompositeConditionEnum.And,
                                                                      new kCura.Relativity.Client.ObjectsCondition(GroupFieldNames.Workspaces, kCura.Relativity.Client.ObjectsConditionEnum.AnyOfThese, new int[] { workspaceId }));
            var res = client.Repositories.Group.Query(q);

            var userQ = new Query<kCura.Relativity.Client.DTOs.User>();
            userQ.Fields = FieldValue.AllFields;
            userQ.Condition = new ObjectsCondition(UserFieldNames.Groups, ObjectsConditionEnum.AnyOfThese, res.Results.Select(x => x.Artifact.ArtifactID).ToList());
            var userRes = client.Repositories.User.Query(userQ);
            if (userRes.TotalCount < 1)
            {
                //ideally you'd create a user in this but that's too much for the scope of this work
                throw new NotSupportedException("No user to run this test, please create a non system admin and ensure the group is assigned to the workspace.");
            }
            var userId = userRes.Results.First().Artifact.ArtifactID;
            //ACT
            var value = new RelativityObject()
            {
                ArtifactId = userId
            };
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
               workspaceId,
               docId,
                    new FieldRef(fieldGuid),
                    value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(userId, result[fieldGuid].ValueAsSingleObject().ArtifactId);
        }

        public static async Task UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess(this IObjectManager manager, IHelper helper, int workspaceId, int docId)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleChoice);
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;
            var choice = client.Repositories.Choice.ReadSingle(Guid.Parse(SingleChoiceChoiceDefinitions.Single1));

            //ACT
            var value = new ChoiceRef(choice.ArtifactID);
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
                workspaceId,
                docId,
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(docId, result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(choice.ArtifactID, result[fieldGuid].ValueAsSingleChoice().ArtifactId);
        }

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

            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
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
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(manager,
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
            query.Condition = new WholeNumberCondition(LayoutFieldNames.ObjectType, NumericConditionEnum.EqualTo, 10);
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query)
                .Results
                .First(x => x.Artifact.Guids.Contains(Guid.Parse(LayoutDefinitions.EventHandlerErrorOnYes))).Artifact;

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
            query.Condition = new WholeNumberCondition(LayoutFieldNames.ObjectType, NumericConditionEnum.EqualTo, 10);
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query)
                .Results
                .First(x => x.Artifact.Guids.Contains(Guid.Parse(LayoutDefinitions.PreloadPopulatesLongText))).Artifact;

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
