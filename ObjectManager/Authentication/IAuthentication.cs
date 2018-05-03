using System.Net.Http;

namespace ObjectManager
{
    public interface IAuthentication
    {
        void SetHeaders(HttpClient request);
    }
}
