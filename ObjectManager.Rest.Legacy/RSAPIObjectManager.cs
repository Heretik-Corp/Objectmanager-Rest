using System;
using System.Data.SqlClient;
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
            var objectTypeId = this.GetObjectTypeFromObject(workspaceId, obj);
            this.CheckObjectTypeSupported(objectTypeId);
            using (var client = _helper.GetClient(workspaceId))
            {
                client.APIOptions.WorkspaceID = workspaceId;
                if (objectTypeId == 10)
                {
                    var dto = obj.ToDTODocument();
                    var result = client.Repositories.Document.Read(dto).EnsureSuccess();
                    var resultObject = result.First().ToRelativityObject();
                    return Task.FromResult(resultObject);
                }
                else if (objectTypeId >= 1_000_000)
                {
                    obj.ObjectType = new ObjectType(objectTypeId);
                    var dto = obj.ToRDO();
                    var result = client.Repositories.RDO.Read(dto).EnsureSuccess();
                    var resultObject = result.First().ToRelativityObject();
                    return Task.FromResult(resultObject);
                }
                throw new NotSupportedException($"Object Type {objectTypeId} is not supported for Read.");
            }
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            return this.UpdateAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectUpdateResult> UpdateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var objectTypeId = this.GetObjectTypeFromObject(workspaceId, obj);
            this.CheckObjectTypeSupported(objectTypeId);
            using (var client = _helper.GetClient(workspaceId))
            {
                client.APIOptions.WorkspaceID = workspaceId;
                if (objectTypeId == 10)
                {
                    var dto = obj.ToDTODocument();
                    client.Repositories.Document.UpdateSingle(dto);
                }
                else if (objectTypeId >= 1_000_000)
                {
                    var dto = obj.ToRDO();
                    client.Repositories.RDO.UpdateSingle(dto);
                }
                return Task.FromResult(new ObjectUpdateResult());
            }
        }

        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context)
        {
            ObjectTypeValidator.ValidateObjectTypeForCreate(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        public Task<ObjectCreateResult> CreateAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            ObjectTypeValidator.ValidateObjectTypeForCreate(obj);
            return this.CreateInternalAsync(workspaceId, obj, context, default(CancellationToken));
        }

        private Task<ObjectCreateResult> CreateInternalAsync(int workspaceId, RelativityObject obj, CallingContext context, CancellationToken token)
        {
            var objectTypeId = this.GetObjectTypeFromObject(workspaceId, obj);
            this.CheckObjectTypeSupported(objectTypeId);
            using (var client = _helper.GetClient(workspaceId))
            {
                client.APIOptions.WorkspaceID = workspaceId;
                if (objectTypeId >= 1_000_000)
                {
                    var dto = obj.ToRDO();
                    var resultId = client.Repositories.RDO.CreateSingle(dto);
                    obj.ArtifactId = resultId;
                }
                else
                {
                    throw new NotSupportedException($"Object Type Id {objectTypeId} is not supported for create.");
                }
                return Task.FromResult(new ObjectCreateResult
                {
                    Object = obj
                });
            }
        }

        private void CheckObjectTypeSupported(int objectType)
        {
            if (objectType == 10 || objectType >= 1_000_000)
            {
                return;
            }
            throw new NotSupportedException($"Object type {objectType} is not supported.");
        }

        private int GetObjectTypeFromObject(int workspaceId, RelativityObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (obj.ObjectType != null && obj.ObjectType.ArtifactTypeId > 0)
            {
                return obj.ObjectType.ArtifactTypeId;
            }
            else if (obj.ArtifactId > 0)
            {
                var dbContext = _helper.GetDBContext(workspaceId);
                var sql = @"SELECT ArtifactTypeId from [EDDSDBO].[Artifact] a with (nolock)
                            WHERE a.ArtifactID = @aId";
                var objectTypeId = dbContext.ExecuteSqlStatementAsScalar<int?>(sql, new[] { new SqlParameter("@aId", obj.ArtifactId) });
                if (!objectTypeId.HasValue)
                {
                    throw new NotSupportedException($"Could not find objectType for artifact Id {obj.ArtifactId}");
                }
                return objectTypeId.Value;
            }
            throw new NotSupportedException("Object type cannot be determined on object by either ArtifactId or ObjectType properties, please ensure one of those are loaded");

        }
    }
}
