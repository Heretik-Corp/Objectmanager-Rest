using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ObjectManager.Rest.Exceptions;

namespace ObjectManager.Rest
{
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
                if (error == null && string.IsNullOrEmpty(message.ReasonPhrase))
                {
                    error = new ErrorEnvelope
                    {
                        ErrorType = "ReasonPhrase",
                        Message = message.ReasonPhrase
                    };
                }
                return error;
            }
            return null;
        }
        public static void ThrowIfNotNull(this ErrorEnvelope e)
        {
            try
            {
                if (e != null)
                {
                    var ex = ParseError(e);
                    throw ex;
                }
            }
            catch (Exception exp)
            {
                throw new Exception("Throw if not null", exp);
            }
        }

        private static Exception ParseError(ErrorEnvelope e)
        {
            var sb = new StringBuilder();
            try
            {
                if (e.ErrorType == "Relativity.Services.Objects.Exceptions.EventHandlerFailedException")
                {
                    sb.AppendLine("Running::Relativity.Services.Objects.Exceptions.EventHandlerFailedException");
                    return new EventHandlerFailedException(e.Message);
                }
                else if (e.ErrorType == "Relativity.Services.Exceptions.ValidationException")
                {
                    sb.AppendLine("Running::Relativity.Services.Exceptions.ValidationException");
                    return new ValidationException(e.Message);
                }
                else if (e.ErrorType == "ReasonPhrase")
                {
                    sb.AppendLine("Running::ReasonPhrase");
                    return new ReasonPhraseException(e.Message);
                }
                sb.AppendLine("SerializeObject");
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(e);
                sb.AppendLine("DONE: SerializeObject");
                sb.AppendLine(str);
                return new System.Exception(str);
            }
            catch (Exception exp)
            {
                throw new Exception(sb.ToString(), exp);
            }
        }
    }
}
