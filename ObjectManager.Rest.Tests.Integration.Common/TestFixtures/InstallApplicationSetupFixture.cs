using kCura.Relativity.Client;
using Relativity.API;
using Relativity.Test.Helpers;
using System.Collections.Generic;

namespace ObjectManager.Rest.Tests.Integration.Common.TestFixtures
{
    public class InstallApplicationSetupFixture
    {
        private readonly IHelper _helper;
        private HashSet<string> _installCache = new HashSet<string>();
        public InstallApplicationSetupFixture()
        {
            _helper = new TestHelper();
        }

        public void Init(int workspaceId, ApplicationInstallContext installContext)
        {
            this.SetupEnvironment(workspaceId, installContext);
        }

        private void SetupEnvironment(int workspaceId, ApplicationInstallContext context)
        {
            var client = _helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System);
            client.APIOptions.WorkspaceID = workspaceId;
            if (_installCache.Contains(context.Name))
            {
                return;
            }
            Relativity.Test.Helpers.Application.ApplicationHelpers.ImportApplication(client,
                workspaceId,
                true,
                context.Path,
                context.Name);
            _installCache.Add(context.Name);
        }
    }
}
