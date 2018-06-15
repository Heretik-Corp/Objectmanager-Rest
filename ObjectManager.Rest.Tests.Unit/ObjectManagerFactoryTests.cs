using Moq;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Legacy;
using ObjectManager.Rest.V1;
using ObjectManager.Rest.V2;
using Relativity.API;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.Tests.Unit
{
    [UnitTest]
    public class ObjectManagerFactoryTests
    {
        private readonly Mock<IRelativityVersionResolver> _versionMock;
        private readonly Mock<IHelper> _helperMock;
        private readonly ObjectManagerFactory _factory;

        public ObjectManagerFactoryTests()
        {
            _versionMock = new Mock<IRelativityVersionResolver>();
            _helperMock = new Mock<IHelper>();
            var servicesMock = new Mock<IServicesMgr>();
            servicesMock.Setup(x => x.GetRESTServiceUrl()).Returns(new Uri("https://start.domain.com"));
            _helperMock.Setup(x => x.GetServicesManager()).Returns(servicesMock.Object);
            _factory = new ObjectManagerFactory(_versionMock.Object, _helperMock.Object);
        }
        [Fact]
        public async Task GetObjectManagerAsync_VersionIs9_5_411_4_ReturnsV1ObjectManager()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.5.411.4")));

            //ACT
            var v = await _factory.GetObjectManagerAsync(Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<ObjectManagerV1>(v);
        }

        [Fact]
        public async Task GetObjectManagerAsync_VersionIsLegacy_ReturnsLegacyService()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.5.287.42")));

            //ACT
            var v = await _factory.GetObjectManagerAsync(Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<RSAPIObjectManager>(v);
        }

        [Fact]
        public async Task GetObjectManagerAsync_VersionIsAfter9_5_411_4_ReturnsV2Manager()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.6.50.31")));
            //ACT
            var v = await _factory.GetObjectManagerAsync(Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<ObjectManagerV2>(v);
        }
    }
}
