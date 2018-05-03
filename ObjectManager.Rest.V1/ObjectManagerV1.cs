using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Rest.V1
{
    internal class ObjectManagerV1 : IObjectManager
    {
        private readonly IAuthentication _authentication;
        private readonly string _host;
        private readonly HttpClient _request;

        public ObjectManagerV1(string host, IAuthentication authentication)
        {
            _authentication = authentication;
            _host = host;
            _request = new HttpClient();
            _request.BaseAddress = new System.Uri(_host);
            _request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            _authentication.SetHeaders(_request);
            var result = await _request.GetAsync($"/Relativity.REST/api/Relativity.Objects/workspaces/{workspaceId}/objects");
            result.EnsureSuccessStatusCode();
            var ret = await result.Content.ReadAsAsync<ObjectUpdateResult>();
            return ret;
        }
    }
}
