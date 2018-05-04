using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Interfaces.Extensions;
using ObjectManager.Rest.V1.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectManager.Rest.V1
{
    internal class ObjectManagerV1 : IObjectManager
    {
        private const string BASE_PATH = "/Relativity.REST/api/Relativity.Objects/workspaces";
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
            var request = RelativityObjectUpdateRestPrep.PrepareForUpdateRequst(obj);
            var result = await _request.PostAsJsonAsync($"{BASE_PATH}/{workspaceId}/objects/{obj.ArtifactId}", new
            {
                RelativityObject = request,
                CallingContext = context
            }, token);
            result.EnsureSuccessStatusCode();
            var ret = await result.Content.ReadAsAsync<ObjectUpdateResult>();
            return ret;
        }
        public async Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            _authentication.SetHeaders(_request);
            var request = RelativityObjectRestReadPrep.PrepareForReadRequst(obj, context);
            var result = await _request.PostAsJsonAsync($"{BASE_PATH}/{workspaceId}/objects/{obj.ArtifactId}/read", request);
            result.EnsureSuccess();
            var ret = await result.Content.ReadAsAsync<ReadResult>();
            return ret.RelativityObject.ToCoreModel();
        }


    }
}
