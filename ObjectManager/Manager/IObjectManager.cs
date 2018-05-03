using Relativity.Services.Objects.DataContracts;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Manager
{
    public interface IObjectManager
    {
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context);
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token);
    }
}
