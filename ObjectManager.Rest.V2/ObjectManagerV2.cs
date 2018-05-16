﻿using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Interfaces.Extensions;
using ObjectManager.Rest.V2.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

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

        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.ReadAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectRestReadPrep.Prep(obj);
            var result = await _request.PostAsJsonAsync($"/Relativity.REST/api/Relativity.Objects/workspace/{workspaceId}/object/read", request, token);
            result.EnsureSuccess();
            var ret = await result.Content.ReadAsAsync<ReadResult>();
            return ret.Object;
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public async Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var request = RelativityObjectUpdateRestPrep.Prep(obj);
            var result = await _request.PostAsJsonAsync($"/Relativity.REST/api/Relativity.Objects/workspace/{workspaceId}/object/update", request, token);
            result.EnsureSuccess();
            var ret = await result.Content.ReadAsAsync<ObjectUpdateResult>();
            return ret;
        }
    }
}
