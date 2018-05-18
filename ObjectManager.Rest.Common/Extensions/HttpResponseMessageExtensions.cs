using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ObjectManager.Rest
{
    public class EventHandlerFailedException : Exception
    {
        public EventHandlerFailedException(string message) : base(message) { }
    }
    internal static class HttpResponseMessageExtensions
    {
        internal class ErrorEnvelope
        {
            public string ErrorType { get; set; }
            public string Message { get; set; }
        }
        public static async Task<ErrorEnvelope> EnsureSuccessAsync(this HttpResponseMessage message)
        {
            if (!message.IsSuccessStatusCode)
            {
                var text = await message.Content.ReadAsStringAsync();
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorEnvelope>(text);
                return error;
            }
            return null;
        }
        public static void ThrowIfNotNull(this ErrorEnvelope e)
        {
            if (e != null)
            {
                var ex = ParseError(e);
                throw ex;
            }
        }

        private static Exception ParseError(ErrorEnvelope e)
        {
            if (e.ErrorType == "Relativity.Services.Objects.Exceptions.EventHandlerFailedException")
            {
                return new EventHandlerFailedException(e.Message);
            }
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(e);
            return new System.Exception(str);
        }
    }
}
