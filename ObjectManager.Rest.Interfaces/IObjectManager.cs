using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Interfaces
{
    public interface IObjectManager
    {
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context);
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token);
    }
}
