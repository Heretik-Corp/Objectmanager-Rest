using Relativity.API;

namespace ObjectManager.Rest.Tests.Integration.Common.TestFixtures
{
    public class WorkspaceSetupFixtureHelper
    {
        public const string CollectionName = "Workspace Collection";
        public int WorkspaceId { get; private set; }
        public IHelper Helper { get; private set; }


        public static int SetupEnvironment(IHelper helper, string workspaceName)
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

        public static void TearDown(int workspaceId, IHelper helper)
        {
            Relativity.Test.Helpers.WorkspaceHelpers.DeleteWorkspace.DeleteTestWorkspace(workspaceId,
                                helper.GetServicesManager(),
                                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.ADMIN_USERNAME,
                                Relativity.Test.Helpers.SharedTestHelpers.ConfigurationHelper.DEFAULT_PASSWORD);
        }
    }
}
