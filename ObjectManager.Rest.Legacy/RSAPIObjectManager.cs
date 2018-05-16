using kCura.Relativity.Client;
using ObjectManager.Rest.Interfaces;
using Relativity.API;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Legacy
{
    internal class RSAPIObjectManager : IObjectManager
    {
        private readonly IHelper _helper;

        public RSAPIObjectManager(IHelper helper)
        {
            _helper = helper;
        }

        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.ReadAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            //TODO: manage repo based on objectType
            using (var client = _helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.CurrentUser))
            {
                client.APIOptions.WorkspaceID = workspaceId;
                var result = client.Repositories.Document.ReadSingle(obj.ArtifactId);
                return null;
            }
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            //TODO: manage repo based on objectType
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            using (var client = _helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.CurrentUser))
            {
                var dto = DTOHelpers.ConvertToDocument(obj);
                var result = client.Repositories.Document.Update(dto);
                if (!result.Success)
                {
                    throw new System.Exception("lazy");
                }
                return null;
            }
        }
    }
}
