using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using System;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.V1.Tests.Integration.Manager
{
    [IntegrationTest]
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
        public void ReadAsync_SanityCheck()
        {
            //ARRANGE

            //ACT

            //ASSERT
        }
    }
}
