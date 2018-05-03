using System.Net.Http;

namespace ObjectManager.Rest.Interfaces.Authentication
{
    public interface IAuthentication
    {
        void SetHeaders(HttpClient request);
    }
}
