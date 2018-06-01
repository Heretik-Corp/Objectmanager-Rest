using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Claims;

namespace ObjectManager.Rest.Interfaces.Authentication
{
    public class AccessTokenAuthentication : IAuthentication
    {
        public void SetHeaders(HttpClient request)
        {
            request.DefaultRequestHeaders.Add("X-CSRF-Header", string.Empty);
            var header = this.GetAuthHeader();
            request.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", header);
            request.DefaultRequestHeaders.Add("X-Kepler-Version", "2.0");
        }
        private string GetAuthHeader()
        {
            var identity = ClaimsPrincipal.Current.Identities.First();
            string headerValue = string.Empty;
            if (identity != null && identity.IsAuthenticated)
            {
                var claim = identity.Claims.FirstOrDefault(x => x.Type == "access_token");
                if (claim != null)
                {
                    headerValue = claim.Value;
                }
            }
            if (string.IsNullOrWhiteSpace(headerValue))
            {
                throw new SecurityException("User is not authenticated");
            }
            return headerValue;
        }
    }
}
