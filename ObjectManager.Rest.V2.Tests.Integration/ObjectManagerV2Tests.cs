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
    [Collection(WorkspaceSetupFixture.CollectionName)]
    public class ObjectManagerV2Tests : IDisposable
    {
        private readonly ObjectManagerV2 _manager;
        private readonly WorkspaceSetupFixture _fixture;
        private readonly DocumentCreationSetupFixture _creation;
        public ObjectManagerV2Tests(WorkspaceSetupFixture fixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV2(_fixture.Helper.GetRestUrl(), new UsernamePasswordAuthentication(ConfigHelper.UserName, ConfigHelper.Password));
            _creation = new DocumentCreationSetupFixture(fixture.Helper);
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
        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
