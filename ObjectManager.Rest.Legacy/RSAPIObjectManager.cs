using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Legacy.Extensions;
using Relativity.API;
using System.Linq;
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
            using (var client = _helper.GetClient(workspaceId))
            {
                var dto = obj.ToDTODocument();
                var result = client.Repositories.Document.Read(dto).EnsureSuccess();
                var resultObject = result.First().ToRelativityObject();
                return Task.FromResult(resultObject);
            }
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            //TODO: manage repo based on objectType
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            using (var client = _helper.GetClient(workspaceId))
            {
                var dto = obj.ToDTODocument();
                client.APIOptions.WorkspaceID = workspaceId;
                client.Repositories.Document.UpdateSingle(dto);
                return Task.FromResult(new ObjectUpdateResult());
            }
        }
    }
}
