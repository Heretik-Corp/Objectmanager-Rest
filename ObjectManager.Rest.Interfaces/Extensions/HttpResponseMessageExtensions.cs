using System.Net.Http;

namespace ObjectManager.Rest.Interfaces.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureSuccess(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                throw new System.Exception(message.Content.ReadAsStringAsync().Result);
                //TODO parse message
            }
        }
    }
}
