using Relativity.API;
using Relativity.Test.Helpers;
using System;
using Xunit;

namespace ObjectManager.Integration.TestFixtures
{
    [CollectionDefinition(CollectionName)]
    public class WorkspaceSetupFixture : ICollectionFixture<WorkspaceSetupFixture>, IDisposable
    {
        public const string CollectionName = "Workspace Collection";
        private const string WORKSPACE_NAME = "Integration test";
        public IHelper Helper { get; private set; }
        public int WorkspaceId { get; private set; }
        public WorkspaceSetupFixture()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Helper = new TestHelper();
            SetupEnvironment(this.Helper);
        }

        private void SetupEnvironment(IHelper helper)
        {
            //Create workspace
            this.WorkspaceId = Relativity.Test.Helpers.WorkspaceHelpers.CreateWorkspace.CreateWorkspaceAsync(
                WORKSPACE_NAME,
                ConfigHelper.TemplateName,
                helper.GetServicesManager(),
                ConfigHelper.AdminUserName,
                ConfigHelper.AdminPassword)
                .Result;
        }

        public void Dispose()
        {
            Relativity.Test.Helpers.WorkspaceHelpers.DeleteWorkspace.DeleteTestWorkspace(
                this.WorkspaceId,
                this.Helper.GetServicesManager(),
                ConfigHelper.AdminUserName,
                ConfigHelper.AdminPassword);
        }
    }
}
