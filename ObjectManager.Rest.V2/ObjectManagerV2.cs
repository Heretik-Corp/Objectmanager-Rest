using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ObjectManager.Rest.Exceptions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.V2.Models;

namespace ObjectManager.Rest.V2
{
    internal class ObjectManagerV2 : IObjectManager
    {
        private readonly IAuthentication _authentication;
        private readonly string _host;
        private readonly HttpClient _request;

        public ObjectManagerV2(string host, IAuthentication authentication)
        {
            _authentication = authentication;
            _host = host;
            _request = new HttpClient();
            _request.BaseAddress = new System.Uri(_host);
            _request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _authentication.SetHeaders(_request);
        }

        #region Create
        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            ObjectTypeValidator.ValidateObjectTypeForCreate(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            ObjectTypeValidator.ValidateObjectTypeForCreate(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, token);
        }
        private async Task<ObjectCreateResult> CreateInternalAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectRestCreatePrep.Prep(obj, context);
            var result = await _request.PostAsJsonAsync($"/Relativity.REST/api/Relativity.Objects/workspace/{workspaceId}/object/create", request);
            var error = await result.EnsureSuccessAsync();
            error.ThrowIfNotNull();
            var ret = await result.Content.ReadAsAsync<ObjectCreateResult>();
            return ret;
        }

        #endregion

        #region Read
        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.ReadAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectRestReadPrep.Prep(obj, context);
            var result = await _request.PostAsJsonAsync($"/Relativity.REST/api/Relativity.Objects/workspace/{workspaceId}/object/read", request, token);
            var error = await result.EnsureSuccessAsync();
            error.ThrowIfNotNull();
            var ret = await result.Content.ReadAsAsync<ReadResult>();
            return ret.Object;
        }

        #endregion

        #region Update
        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectUpdateRestPrep.Prep(obj, context);
            var result = await _request.PostAsJsonAsync($"/Relativity.REST/api/Relativity.Objects/workspace/{workspaceId}/object/update", request, token);
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
        #endregion
    }
}
