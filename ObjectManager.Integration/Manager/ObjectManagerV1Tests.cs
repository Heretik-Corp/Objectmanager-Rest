using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.V1.Tests.Integration.Manager
{
    [IntegrationTest]
    [Collection(WorkspaceSetupFixture.CollectionName)]
    public class ObjectManagerV1Tests : IDisposable
    {
        private readonly ObjectManagerV1 _manager;
        private readonly WorkspaceSetupFixture _fixture;
        private readonly DocumentCreationSetupFixture _creation;
        public ObjectManagerV1Tests(WorkspaceSetupFixture fixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV1(ConfigHelper.Url, new UsernamePasswordAuthentication(ConfigHelper.UserName, ConfigHelper.Password));
            _creation = new DocumentCreationSetupFixture(fixture.Helper);
        }

        public void Dispose()
        {
            _creation?.Dispose();
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
    }
}
