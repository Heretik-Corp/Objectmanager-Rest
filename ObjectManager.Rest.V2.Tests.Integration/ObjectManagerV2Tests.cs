using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Interfaces.Models;
using ObjectManager.Rest.Tests.Integration.Common;
using ObjectManager.Rest.Tests.Integration.Common.Extensions;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.V2.Tests.Integration
{

    [IntegrationTest]
    [Collection(WorkspaceSetupFixtureHelper.CollectionName)]
    public class ObjectManagerV2Tests : IClassFixture<InstallApplicationSetupFixture>, IDisposable
    {
        private readonly ObjectManagerV2 _manager;
        private readonly WorkspaceSetupFixture _fixture;
        private readonly DocumentCreationSetupFixture _creation;
        private readonly InstallApplicationSetupFixture _installFixture;

        public static IEnumerable<object[]> FieldTestData
        {
            get { return DocumentFieldDefinitions.FieldTestData; }
        }


        public ObjectManagerV2Tests(WorkspaceSetupFixture fixture, InstallApplicationSetupFixture installFixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV2(_fixture.Helper.GetRestUrl(), new UsernamePasswordAuthentication(ConfigHelper.UserName, ConfigHelper.Password));
            _creation = new DocumentCreationSetupFixture(fixture.Helper);
            _installFixture = installFixture;
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
        }

        #region SanityChecks
        [Fact]
        public async Task ReadAsync_SanityCheck()
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.First()
            }, null);

            //ASSERT
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Name.Equals("Artifact Id", StringComparison.CurrentCultureIgnoreCase));
            Assert.Equal(_creation.DocIds.Single(), result["Artifact Id"].ValueAsWholeNumber());
        }

        #endregion

        #region SingleChoice
        [Fact]
        public Task UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            return _manager.UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public async Task UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceGuid_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleChoice);
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var value = new ChoiceRef(Guid.Parse(SingleChoiceChoiceDefinitions.Single1));
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager,
                _fixture.WorkspaceId,
                _creation.DocIds.First(),
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(value.Guids.First(), result[fieldGuid].ValueAsSingleChoice().Guids.First());
        }

        #endregion

        #region Single Object
        [Fact]
        public Task UpdateAsync_UpdateSingleObjectByArtifactId_ReturnsSuccess()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            return _manager.UpdateAsync_UpdateSingleObjectByArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        #endregion

        #region User
        [Fact]
        public Task UpdateAsync_UpdateUserByArtifactId_ReturnsSuccess()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            return _manager.UpdateAsync_UpdateUserByArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First(), _fixture.UserName);
        }
        #endregion

        #region Multi Object
        [Fact]
        public Task UpdateAsync_UpdateMultiObjectByArtifactId_ReturnsSuccess()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            return _manager.UpdateAsync_UpdateMultiObjectByArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }
        #endregion

        #region MultiChoice

        [Fact]
        public async Task UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceGuid_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.Multichoice);
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var value = new List<ChoiceRef> {
                new ChoiceRef(Guid.Parse(MultiChoiceChoiceDefinitions.Multi1)),
                new ChoiceRef(Guid.Parse(MultiChoiceChoiceDefinitions.Multi2))
            };
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager,
                    _fixture.WorkspaceId,
                    _creation.DocIds.First(),
                    new FieldRef(fieldGuid),
                    value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(value.First().Guids.First(), result[fieldGuid].ValueAsMultiChoice().First().Guids.First());
            Assert.Equal(value.Last().Guids.First(), result[fieldGuid].ValueAsMultiChoice().Last().Guids.First());
        }

        [Fact]
        public Task UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            return _manager.UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        #endregion

        #region UpdateByGuid
        [Theory]
        [MemberData(nameof(FieldTestData))]
        public async Task UpdateAsync_UpdateFieldByGuid_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(fieldGuidString);
            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);

            //ACT
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(
                _manager,
                _fixture.WorkspaceId,
                _creation.DocIds.First(),
                new FieldRef(fieldGuid), value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(expected, result[fieldGuid].Value);
        }

        #endregion

        #region CallingContext
        [Fact]
        public async Task UpdateAsync_CallingContextArtifactIdSet_ReturnsCorrectStatus()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.FixedLength);
            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;

            var query = new Query<Layout>();
            query.Condition = new TextCondition(LayoutFieldNames.TextIdentifier, TextConditionEnum.EqualTo, "Default Test Layout");
            query.Fields = FieldValue.AllFields;
            var layout = client.Repositories.Layout.Query(query).Results.First().Artifact;

            //ACT
            var obj = SharedTestCases.CreateTestObject(
                _creation.DocIds.First(),
                new FieldRef(fieldGuid),
                "hello world");

            var result = await _manager.UpdateAsync(_fixture.WorkspaceId, obj, new Interfaces.CallingContext
            {
                Layout = new Interfaces.LayoutRef(layout.Name, layout.ArtifactID)
            });

            //ASSERT
            Assert.True(result.EventHandlerStatuses.All(x => x.Success));
        }

        [Fact]
        public async Task UpdateAsync_CallingContextSetLayoutHasEventhandlerError_ReturnsCorrectStatus()
        {
            _creation.Create(_fixture.WorkspaceId, 1);
            var result = await _manager.UpdateAsync_CallingContextSetLayoutHasEventhandlerError_ReturnsCorrectStatus(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());

            //ASSERT
            Assert.Contains(result.EventHandlerStatuses, x => !x.Success);
        }

        [Fact]
        public async Task ReadAsync_CallingContextSetLayoutHasPreload_ReturnsCorrectLoadedFields()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.LongText);
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var result = await _manager.ReadAsync_CallingContextSetLayoutHasPreload_ReturnsCorrectLoadedFields(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());

            //ASSERT
            Assert.NotNull(result[fieldGuid].Value);
        }

        #endregion

        #region UpdateFieldByArtifactId

        [Theory]
        [MemberData(nameof(FieldTestData))]

        public async Task UpdateAsync_UpdateFieldByArtifactId_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;

            var fieldGuid = Guid.Parse(fieldGuidString);
            var field = client.Repositories.Field.ReadSingle(fieldGuid);

            //ACT
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager, _fixture.WorkspaceId, _creation.DocIds.First(), new FieldRef(field.ArtifactID), value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.ArtifactId == field.ArtifactID);
            Assert.Equal(expected, result[field.ArtifactID].Value);

        }

        #endregion

        #region UpdateByName
        [Theory]
        [MemberData(nameof(FieldTestData))]

        public async Task UpdateAsync_UpdateFieldByName_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;
            _creation.Create(_fixture.WorkspaceId, 1);

            var fieldGuid = Guid.Parse(fieldGuidString);
            var field = client.Repositories.Field.ReadSingle(fieldGuid);

            //ACT
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager, _fixture.WorkspaceId, _creation.DocIds.First(), new FieldRef(field.ArtifactID), value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Name == field.Name);
            Assert.Equal(expected, result[field.Name].Value);

        }

        #endregion

        [Fact]
        public Task CreateAsync_SanityCheckRDO_ReturnsSuccess()
        {
            return _manager.CreateAsync_SanityCheckRDO_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, Guid.Parse(ObjectTypeGuids.SingleObject));
        }

        [Fact]
        public Task CreateAsync_SanityCheckDocument_ReturnsSuccess()
        {
            return _manager.CreateAsync_SanityCheckDocument_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId);
        }



        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
