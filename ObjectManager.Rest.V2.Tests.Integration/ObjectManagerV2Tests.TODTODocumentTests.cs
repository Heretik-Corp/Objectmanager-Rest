using System;
using System.Linq;
using System.Threading.Tasks;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Tests.Integration.Common;
using ObjectManager.Rest.Tests.Integration.Common.Extensions;
using ObjectManager.Rest.Tests.Integration.Common.TestFixtures;
using Xunit;
using Xunit.Categories;

namespace ObjectManager.Rest.V2.Tests.Integration
{
    [IntegrationTest]
    [Collection(WorkspaceSetupFixtureHelper.CollectionName)]
    public class ObjectManagerV2TestsTODTODocumentTests : IClassFixture<InstallApplicationSetupFixture>, IDisposable
    {
        private readonly ObjectManagerV2 _manager;
        private readonly WorkspaceSetupFixture _fixture;
        private readonly DocumentCreationSetupFixture _creation;
        private readonly InstallApplicationSetupFixture _installFixture;


        public ObjectManagerV2TestsTODTODocumentTests(WorkspaceSetupFixture fixture, InstallApplicationSetupFixture installFixture)
        {
            _fixture = fixture;
            _manager = new ObjectManagerV2(_fixture.Helper.GetRestUrl(), new UsernamePasswordAuthentication(ConfigHelper.UserName, ConfigHelper.Password));
            _creation = new DocumentCreationSetupFixture(fixture.Helper);
            _installFixture = installFixture;

            _creation.Create(_fixture.WorkspaceId, 1);
            _installFixture.Init(_fixture.WorkspaceId, ApplicationInstallContext.FieldTestPath);
        }

        [Fact]
        public Task ToDTO_FieldIsFixedLength_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsFixedLength_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public Task ToDTO_FieldIsLongText_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsLongText_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public Task ToDTO_FieldIsYesNo_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsYesNo_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public Task ToDTO_FieldIsSingleChoice_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsSingleChoice_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsMultiChoice_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsMultiChoice_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }

        [Fact]
        public Task ToDTO_FieldIsCurrency_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsCurrency_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsDecimal_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsDecimal_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsWholeNumber_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsWholeNumber_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsDate_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsDate_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsSingleObject_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsSingleObject_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsMultiObject_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsMultiObject_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        [Fact]
        public Task ToDTO_FieldIsUser_ReturnsCorrectValue()
        {
            return _manager.ToDTO_FieldIsUser_ReturnsCorrectValue(_fixture.Helper, _fixture.WorkspaceId, _creation.DocIds.First());
        }


        public void Dispose()
        {
            _creation?.Dispose();
        }
    }
}
