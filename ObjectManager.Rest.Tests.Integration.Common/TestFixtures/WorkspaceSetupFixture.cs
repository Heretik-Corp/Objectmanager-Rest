using Relativity.API;
using Relativity.Test.Helpers;
using Xunit;

namespace ObjectManager.Rest.Tests.Integration.Common.TestFixtures
{
    [CollectionDefinition(CollectionName)]
    public class WorkspaceSetupFixture
    {
        public const string CollectionName = "Workspace Collection";
        private readonly string _workspaceName = "Integration test";
        public int WorkspaceId { get; private set; }
        public IHelper Helper { get; private set; }

        public WorkspaceSetupFixture()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            var helper = new TestHelper();
            this.Helper = helper;
            SetupEnvironment(helper);
        }

        private void SetupEnvironment(IHelper helper)
        {
            //Create workspace
            this.WorkspaceId = Relativity.Test.Helpers.WorkspaceHelpers.CreateWorkspace.CreateWorkspaceAsync(
                _workspaceName,
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME,
                helper.GetServicesManager(),
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME,
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD)
                .Result;
        }
    }
}
