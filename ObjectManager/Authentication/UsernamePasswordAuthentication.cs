using System;
using System.Net.Http;
using System.Text;

namespace ObjectManager.Authentication
{
    public class UsernamePasswordAuthentication : IAuthentication
    {
        private readonly string _authValue;

        public UsernamePasswordAuthentication(string username, string password)
        {
            _authValue = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
    .GetBytes($"{username}:{password}"));
        }
        public void SetHeaders(HttpClient request)
        {
            request.DefaultRequestHeaders.Add("X-CSRF-Header", string.Empty);
            request.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authValue);
            request.DefaultRequestHeaders.Add("X-Kepler-Version", "2.0");
        }
    }
}
