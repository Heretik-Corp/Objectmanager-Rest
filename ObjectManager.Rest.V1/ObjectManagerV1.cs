using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ObjectManager.Rest.Exceptions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.V1.Models;

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
            _authentication.SetHeaders(_request);
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }
        public async Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectUpdateRestPrep.PrepareForUpdateRequst(obj);
            var result = await _request.PostAsJsonAsync($"{BASE_PATH}/{workspaceId}/objects/{obj.ArtifactId}", new
            {
                RelativityObject = request,
                CallingContext = context
            }, token);
            var error = await result.EnsureSuccessAsync();
            try
            {
                error.ThrowIfNotNull();
            }
            catch (EventHandlerFailedException ehfe)
            {
                return new ObjectUpdateResult(ehfe.Message);
            }
            var ret = await result.Content.ReadAsAsync<ObjectUpdateResult>();
            return ret;
        }
        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.ReadAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectRestReadPrep.PrepareForReadRequst(obj, context);
            var result = await _request.PostAsJsonAsync($"{BASE_PATH}/{workspaceId}/objects/{obj.ArtifactId}/read", request, token);
            var error = await result.EnsureSuccessAsync();
            error.ThrowIfNotNull();
            var ret = await result.Content.ReadAsAsync<ReadResult>();
            return ret.RelativityObject.ToCoreModel();
        }

        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            ObjectTypeValidator.ValidateObjectType(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            ObjectTypeValidator.ValidateObjectType(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, token);
        }

        private async Task<ObjectCreateResult> CreateInternalAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectCreateRestPrep.Prep(obj);
            var result = await _request.PostAsJsonAsync($"{BASE_PATH}/{workspaceId}/objects/create", new
            {
                RelativityObject = request,
                CallingContext = context
            }, token);
            var error = await result.EnsureSuccessAsync();
            try
            {
                error.ThrowIfNotNull();
            }
            catch (EventHandlerFailedException ehfe)
            {
                return new ObjectCreateResult(ehfe.Message);
            }
            var ret = await result.Content.ReadAsAsync<ReadResult>();
            return new ObjectCreateResult
            {
                Object = ret.RelativityObject.ToCoreModel(),
                EventHandlerStatuses = ret.EventHandlerStatuses
            };
        }
    }
}
