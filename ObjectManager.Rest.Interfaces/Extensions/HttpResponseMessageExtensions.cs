using System.Net.Http;

namespace ObjectManager.Rest.Interfaces.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureSuccess(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                //TODO parse message
            }
        }
    }
}
