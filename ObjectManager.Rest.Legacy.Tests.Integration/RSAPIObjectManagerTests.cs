﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using ObjectManager.Rest.Legacy.Tests.Integration.TestFixtures;
using ObjectManager.Rest.Tests.Integration.Common;
using ObjectManager.Rest.Tests.Integration.Common.Extensions;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
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

            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
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
        }

        #region SingleObject
        [Fact]
        public async Task UpdateAsync_UpdateSingleObjectByArtifactId_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleObject);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;
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
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager,
                _fixture.WorkspaceId,
                _creation.DocIds.First(),
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(obj, result[fieldGuid].ValueAsSingleObject().ArtifactId);
        }

        #endregion

        #region SingleChoice
        [Fact]
        public async Task UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleChoice);
            var client = _fixture.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = _fixture.WorkspaceId;
            var choice = client.Repositories.Choice.ReadSingle(Guid.Parse(SingleChoiceChoiceDefinitions.Single1));

            //ACT
            var value = new ChoiceRef(choice.ArtifactID);
            var (uResult, result) = await SharedTestCases.RunUpdateTestAsync(_manager,
                _fixture.WorkspaceId,
                _creation.DocIds.First(),
                new FieldRef(fieldGuid),
                value);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Guids.Contains(fieldGuid));
            Assert.Equal(choice.ArtifactID, result[fieldGuid].ValueAsSingleChoice().ArtifactId);

        }

        [Fact]
        public async Task UpdateAsync_UpdateSingleChoiceByGuidUsingChoiceGuid_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleChoice);

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
            Assert.Empty(result[fieldGuid].ValueAsSingleChoice().Guids); //this is by design it's how Relativity works
        }

        #endregion

        #region Multi Object
        [Fact]
        public Task UpdateAsync_UpdateMultiObjectByArtifactId_ReturnsSuccess()
        {
            return _manager.UpdateAsync_UpdateMultiObjectByArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }
        #endregion



        #region User
        [Fact]
        public Task UpdateAsync_UpdateUserByArtifactId_ReturnsSuccess()
        {
            return _manager.UpdateAsync_UpdateUserByArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First(), _fixture.UserName);
        }
        #endregion


        #region MultiChoice

        [Fact]
        public Task UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess()
        {
            return _manager.UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceArtifactId_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public void UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceGuid_ReturnsSuccess()
        {
            // TODO: RSAPI does not support guids on choices
            //return _manager.UpdateAsync_UpdateMultiChoiceByGuidUsingChoiceGuid_ReturnsSuccess(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        #endregion

        #region CallingContext
        [Fact]
        public Task UpdateAsync_CallingContextArtifactIdSet_ReturnsCorrectStatus()
        {
            return _manager.UpdateAsync_CallingContextArtifactIdSet_ReturnsCorrectStatus(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public async Task UpdateAsync_CallingContextSetLayoutHasEventhandlerError_ReturnsCorrectStatus()
        {
            var result = await _manager.UpdateAsync_CallingContextSetLayoutHasEventhandlerError_ReturnsCorrectStatus(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());

            //ASSERT
            Assert.NotNull(result);
        }

        #endregion

        #region UpdateByGuid
        [Theory]
        [MemberData(nameof(FieldTestData))]

        public async Task UpdateAsync_UpdateFieldByGuid_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
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
            Assert.Equal(expected.ToString(), result[fieldGuid].Value.ToString().Trim('0'));
        }

        #endregion

        #region UpdateFieldByArtifactId

        [Theory]
        [MemberData(nameof(FieldTestData))]
        public async Task UpdateAsync_UpdateFieldByArtifactId_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
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
            Assert.Equal(expected.ToString(), result[field.ArtifactID].Value.ToString().Trim('0'));
        }


        #endregion

        #region UpdateByName

        [Theory]
        [MemberData(nameof(FieldTestData))]

        public async Task UpdateAsync_UpdateFieldByName_ReturnsSuccess(string fieldGuidString, object value, object expected)
        {
            //ARRANGE
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
            Assert.Equal(expected.ToString(), result[field.ArtifactID].Value.ToString().Trim('0'));
        }

        public async Task UpdateAsyncSingleObject_UpdateFieldByName_ReturnsSuccess()
        {
            //ARRANGE
            var fieldGuid = Guid.Parse(DocumentFieldDefinitions.SingleObject);
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
                        Value = new RelativityObject
                        {

                        }
                    }
                }
            };

            var uResult = await _manager.UpdateAsync(_fixture.WorkspaceId, obj, null);
            var result = await _manager.ReadAsync(_fixture.WorkspaceId, obj, null);

            //ASSERT
            Assert.True(uResult.EventHandlerStatuses.All(x => x.Success));
            Assert.Equal(_creation.DocIds.Single(), result.ArtifactId);
            Assert.Contains(result.FieldValues, (f) => f.Field.Name == field.Name);
            // Assert.Equal(expected.ToString(), result[field.ArtifactID].Value.ToString().Trim('0'));
        }

        #endregion

        #region Create
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

        #endregion

        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
