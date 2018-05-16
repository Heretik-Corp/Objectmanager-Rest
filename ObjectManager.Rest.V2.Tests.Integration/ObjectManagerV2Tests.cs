using kCura.Relativity.Client;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Interfaces.Extensions;
using ObjectManager.Rest.Interfaces.Models;
using ObjectManager.Rest.Tests.Integration.Common;
using ObjectManager.Rest.Tests.Integration.Common.Extensions;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ObjectManagerV2Tests(WorkspaceSetupFixture fixture, InstallApplicationSetupFixture installFixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV2(_fixture.Helper.GetRestUrl(), new UsernamePasswordAuthentication(ConfigHelper.UserName, ConfigHelper.Password));
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

        [Fact]
        public async Task UpdateAsync_SanityCheck()
        {
            //ARRANGE
            _creation.Create(_fixture.WorkspaceId, 1);

            //ACT
            var result = await _manager.UpdateAsync(_fixture.WorkspaceId, new Interfaces.RelativityObject
            {
                ArtifactId = _creation.DocIds.Single(),
                FieldValues = new List<FieldValuePair>
                {
                    new FieldValuePair{
                        Field =new FieldRef {
                            Name = "MD5 Hash",
                        },
                        Value = "hello world"
                    }
                }
            }, null);

            //ASSERT
            Assert.All(result.EventHandlerStatuses, (ehs) => Assert.True(ehs.Success));
        }

        [Theory]
        [InlineData(DocumentFieldDefinitions.FixedLength, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.LongText, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.Currency, "5,025.30", 5025.30)]
        [InlineData(DocumentFieldDefinitions.Decimal, "1.05", 1.05)]
        [InlineData(DocumentFieldDefinitions.WholeNumber, "1", 1L)]
        [InlineData(DocumentFieldDefinitions.YesNo, true, true)]
        //[InlineData(DocumentFieldDefinitions.Date, "5,025.30")]
        //[InlineData(DocumentFieldDefinitions.SingleChoice, "true")]
        //[InlineData(DocumentFieldDefinitions.Multichoice, "true")]

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

        [Theory]
        [InlineData(DocumentFieldDefinitions.FixedLength, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.LongText, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.Currency, "5,025.30", 5025.30)]
        [InlineData(DocumentFieldDefinitions.Decimal, "1.05", 1.05)]
        [InlineData(DocumentFieldDefinitions.WholeNumber, "1", 1L)]
        [InlineData(DocumentFieldDefinitions.YesNo, true, true)]
        //[InlineData(DocumentFieldDefinitions.Date, "5,025.30")]
        //[InlineData(DocumentFieldDefinitions.SingleChoice, "true")]
        //[InlineData(DocumentFieldDefinitions.Multichoice, "true")]

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
                        Value = value.ToString()
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

        [Theory]
        [InlineData(DocumentFieldDefinitions.FixedLength, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.LongText, "hello world", "hello world")]
        [InlineData(DocumentFieldDefinitions.Currency, "5,025.30", 5025.30)]
        [InlineData(DocumentFieldDefinitions.Decimal, "1.05", 1.05)]
        [InlineData(DocumentFieldDefinitions.WholeNumber, "1", 1L)]
        [InlineData(DocumentFieldDefinitions.YesNo, true, true)]
        //[InlineData(DocumentFieldDefinitions.Date, "5,025.30")]
        //[InlineData(DocumentFieldDefinitions.SingleChoice, "true")]
        //[InlineData(DocumentFieldDefinitions.Multichoice, "true")]

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
                        Value = value.ToString()
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
        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
