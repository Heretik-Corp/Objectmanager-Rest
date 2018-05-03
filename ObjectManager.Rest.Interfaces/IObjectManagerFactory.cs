using ObjectManager.Rest.Interfaces.Authentication;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Interfaces
{
    public interface IObjectManagerFactory
    {
        Task<IObjectManager> GetObjectManagerAsync(string host, IAuthentication authentication);
    }
}