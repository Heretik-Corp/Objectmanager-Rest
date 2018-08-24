using System;
using System.Configuration;
using Relativity.API;
using Relativity.Test.Helpers;

namespace ObjectManager.Rest.Tests.Integration.Common.TestFixtures
{
    public class WorkspaceSetupFixtureHelper : IDisposable
    {
        public const string CollectionName = "Workspace Collection";
        protected readonly string WorkspaceName = "Integration test";
        public int WorkspaceId { get; protected set; }
        public IHelper Helper { get; protected set; }
        public string UserName { get; set; } = ConfigurationManager.AppSettings["AdminUsername"];

        public WorkspaceSetupFixtureHelper()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            var helper = new TestHelper();
            this.Helper = helper;
            this.WorkspaceId = SetupEnvironment(helper, WorkspaceName);
        }

        public int SetupEnvironment(IHelper helper, string workspaceName)
        {
            //Create workspace
            var workspaceId = Relativity.Test.Helpers.WorkspaceHelpers.CreateWorkspace.CreateWorkspaceAsync(
                workspaceName,
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME,
                helper.GetServicesManager(),
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME,
                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD)
                .Result;
            return workspaceId;
        }

        public void TearDown(int workspaceId, IHelper helper)
        {
            Relativity.Test.Helpers.WorkspaceHelpers.DeleteWorkspace.DeleteTestWorkspace(workspaceId,
                                helper.GetServicesManager(),
                                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME,
                                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD);
        }

        public void Dispose()
        {
            TearDown(this.WorkspaceId, this.Helper);
        }
    }
}
