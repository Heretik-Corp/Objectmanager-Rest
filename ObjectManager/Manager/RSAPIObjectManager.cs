using Relativity.Services.Objects.DataContracts;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Manager
{
    internal class RSAPIObjectManager : IObjectManager
    {
        public RSAPIObjectManager()
        {

        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
