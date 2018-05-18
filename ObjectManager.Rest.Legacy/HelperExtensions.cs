using kCura.Relativity.Client;
using Relativity.API;

namespace ObjectManager.Rest.Legacy
{
    internal static class HelperExtensions
    {
        public static IRSAPIClient GetClient(this IHelper helper, int workspaceId)
        {
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.CurrentUser);
            client.APIOptions.WorkspaceID = workspaceId;
            return client;
        }
    }
}
