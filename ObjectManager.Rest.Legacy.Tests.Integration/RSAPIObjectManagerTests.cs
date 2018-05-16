using kCura.Relativity.Client;
using ObjectManager.Rest.Interfaces.Extensions;
using ObjectManager.Rest.Interfaces.Models;
using ObjectManager.Rest.Legacy.Tests.Integration.TestFixtures;
using ObjectManager.Rest.Tests.Integration.Common;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.Legacy.Tests.Integration
{
    [IntegrationTest]
    [Collection(WorkspaceSetupFixture.CollectionName)]
    public class RSAPIObjectManagerTests : IClassFixture<InstallApplicationSetupFixture>, IDisposable
    {
        public static IEnumerable<object[]> FieldTestData
        {
            get { return DocumentFieldDefinitions.FieldTestData; }
        }

        private readonly WorkspaceSetupFixture _fixture;
        private readonly RSAPIObjectManager _manager;
        private readonly DocumentCreationSetupFixture _creation;
        private readonly InstallApplicationSetupFixture _installFixture;

        public RSAPIObjectManagerTests(WorkspaceSetupFixture fixture, InstallApplicationSetupFixture installFixture)
        {
            _fixture = fixture;
            _manager = new RSAPIObjectManager(fixture.Helper);
            _creation = new DocumentCreationSetupFixture(fixture.Helper);
            _installFixture = installFixture;
        }

        [Fact]
        public async Task ReadAsync_SanityCheck()
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.Single()
            }, null);

            //ASSERT
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Name.Equals("Artifact Id", StringComparison.CurrentCultureIgnoreCase));
            Assert.Equal(_creation.DocIds.Single(), result["Artifact Id"].ValueAsWholeNumber());
        }

        #region UpdateByGuid
        [Theory]
        [MemberData(nameof(FieldTestData))]

        public async Task UpdateAsync_UpdateFieldByGuid_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
            var fieldGuid = Guid.Parse(fieldGuidString);

            //ACT
            var obj = new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.Single(),
                FieldValues = new List<FieldValuePair>
                {
                    new FieldValuePair
                    {
                        Field = new FieldRef(fieldGuid),
                        Value = value
                    }
                }
            };

            var uResult = await _manager.UpdateAsync(_fixture.WorkspaceId, obj, null);
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, obj, null);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(expected, result[fieldGuid].Value);

        }

        #endregion

        #region UpdateFieldByArtifactId

        [Theory]
        [MemberData(nameof(FieldTestData))]
        public async Task UpdateAsync_UpdateFieldByArtifactId_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
            var fieldGuid = Guid.Parse(fieldGuidString);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;
            var field = client.Repositories.Field.ReadSingle(fieldGuid);

            //ACT
            var obj = new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.Single(),
                FieldValues = new List<FieldValuePair>
                {
                    new FieldValuePair
                    {
                        Field = new FieldRef(field.ArtifactID),
                        Value = value
                    }
                }
            };

            var uResult = await _manager.UpdateAsync(_fixture.WorkspaceId, obj, null);
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, obj, null);

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
            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
            var fieldGuid = Guid.Parse(fieldGuidString);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;
            var field = client.Repositories.Field.ReadSingle(fieldGuid);

            //ACT
            var obj = new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.Single(),
                FieldValues = new List<FieldValuePair>
                {
                    new FieldValuePair
                    {
                        Field = new FieldRef(field.Name),
                        Value = value
                    }
                }
            };

            var uResult = await _manager.UpdateAsync(_fixture.WorkspaceId, obj, null);
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, obj, null);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Name == field.Name);
            Assert.Equal(expected, result[fieldGuid].Value);

        }

        #endregion

        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
