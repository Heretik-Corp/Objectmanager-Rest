using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Interfaces
{
    public interface IObjectManager
    {
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context);
        Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token);
        Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context);
        Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token);
        Task<RelativityObject> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context);
        Task<RelativityObject> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token);
    }
}
