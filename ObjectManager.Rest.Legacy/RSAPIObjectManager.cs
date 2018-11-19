﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Legacy.Extensions;
using Relativity.API;

namespace ObjectManager.Rest.Legacy
{
    internal class RSAPIObjectManager : IObjectManager
    {
        private readonly IHelper _helper;

        public RSAPIObjectManager(IHelper helper)
        {
            _helper = helper;
        }

        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.ReadAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<RelativityObject> ReadAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            //TODO: manage repo based on objectType
            using (var client = _helper.GetClient(workspaceId))
            {
                var dto = obj.ToDTODocument();
                var result = client.Repositories.Document.Read(dto).EnsureSuccess();
                var resultObject = result.First().ToRelativityObject();
                return Task.FromResult(resultObject);
            }
        }

        //private GenericRepository<T> GetRepo<T>(IRSAPIClient client) where T : kCura.Relativity.Client.DTOs.Artifact, new()
        //{
        //    return client.Repositories.Document;
        //}

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            //TODO: manage repo based on objectType
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            using (var client = _helper.GetClient(workspaceId))
            {
                var dto = obj.ToDTODocument();
                client.APIOptions.WorkspaceID = workspaceId;
                client.Repositories.Document.UpdateSingle(dto);
                return Task.FromResult(new ObjectUpdateResult());
            }
        }

        public Task<RelativityObject> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            if (obj.ObjectType == null || obj.ObjectType.ArtifactTypeID == 0)
            {
                throw new ArgumentException(ObjectManager.Rest.Properties.Messages.Object_Type_Missing);
            }
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<RelativityObject> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            if (obj.ObjectType == null || obj.ObjectType.ArtifactTypeID == 0)
            {
                throw new ArgumentException(ObjectManager.Rest.Properties.Messages.Object_Type_Missing);
            }
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<RelativityObject> CreateInternalAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
