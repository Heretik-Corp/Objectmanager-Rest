using ObjectManager.Rest.Interfaces.Extensions;
using ObjectManager.Rest.Interfaces.Models;
using ObjectManager.Rest.Legacy.Tests.Integration.TestFixtures;
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
    public class RSAPIObjectManagerTests
    {
        private readonly WorkspaceSetupFixture _fixture;
        private readonly RSAPIObjectManager _manager;
        private readonly DocumentCreationSetupFixture _creation;

        public RSAPIObjectManagerTests(WorkspaceSetupFixture fixture)
        {
            _fixture = fixture;
            _manager = new RSAPIObjectManager(fixture.Helper);
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
    }
}
