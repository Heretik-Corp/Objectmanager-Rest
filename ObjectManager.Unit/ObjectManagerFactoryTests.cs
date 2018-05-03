using Moq;
using ObjectManager.Manager;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Unit
{
    [UnitTest]
    public class ObjectManagerFactoryTests
    {
        private const string BaseUri = "https://localhost";
        private readonly Mock<IRelativityVersionResolver> _versionMock;
        private readonly ObjectManagerFactory _factory;

        public ObjectManagerFactoryTests()
        {
            _versionMock = new Mock<IRelativityVersionResolver>();
            _factory = new ObjectManagerFactory(_versionMock.Object);
        }
        [Fact]
        public async Task GetObjectManagerAsync_VersionIs9_5_411_4_ReturnsV1ObjectManager()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.5.411.4")));

            //ACT
            var v = await _factory.GetObjectManagerAsync(BaseUri, Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<ObjectManagerV1>(v);
        }

        [Fact]
        public async Task GetObjectManagerAsync_VersionIsLegacy_ReturnsLegacyService()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.5.411.3")));

            //ACT
            var v = await _factory.GetObjectManagerAsync(BaseUri, Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<RSAPIObjectManager>(v);
        }

        [Fact]
        public async Task GetObjectManagerAsync_VersionIsAfter9_5_411_4_ReturnsV2Manager()
        {
            //ARRANGE
            _versionMock.Setup(x => x.GetRelativityVersionAsync()).Returns(Task.FromResult(new Version("9.6.50.31")));
            //ACT
            var v = await _factory.GetObjectManagerAsync(BaseUri, Mock.Of<IAuthentication>());

            //ASSERT
            Assert.IsType<ObjectManagerV2>(v);
        }
    }
}
