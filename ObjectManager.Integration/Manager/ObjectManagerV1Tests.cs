using ObjectManager.Authentication;
using ObjectManager.Integration.TestFixtures;
using ObjectManager.Manager;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Integration.Manager
{
    [IntegrationTest]
    [Collection(WorkspaceSetupFixture.CollectionName)]
    public class ObjectManagerV1Tests
    {
        private readonly WorkspaceSetupFixture _fixture;
        private readonly ObjectManagerV1 _manager;

        public ObjectManagerV1Tests(WorkspaceSetupFixture fixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV1(ConfigHelper.V1Url, new UsernamePasswordAuthentication(ConfigHelper.AdminUserName, ConfigHelper.AdminPassword));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesObject()
        {
            //ARRANGE

            //ACT
            var result = await _manager.UpdateAsync(_fixture.WorkspaceId, null, null);

            //ASSERT
            Assert.All(result.EventHandlerStatuses, (r) => Assert.True(r.Success));
        }
    }
}
