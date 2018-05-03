using ObjectManager.Manager;
using System.Threading.Tasks;

namespace ObjectManager
{
    public interface IObjectManagerFactory
    {
        Task<IObjectManager> GetObjectManagerAsync(string host, IAuthentication authentication);
    }
}