using System;
using System.Data.SqlClient;
using kCura.Relativity.Client;
using Relativity.API;

namespace ObjectManager.Rest.Tests.Integration.Common.Extensions
{
    public static class HelperExtensions
    {
        public static string GetRestUrl(this IHelper helper)
        {
            return helper.GetServicesManager().GetRESTServiceUrl().GetLeftPart(UriPartial.Authority);
        }

        public static IRSAPIClient GetRSAPIClient(this IHelper helper, int workspaceId, ExecutionIdentity identity = ExecutionIdentity.System)
        {
            var client = helper.GetServicesManager().CreateProxy<IRSAPIClient>(identity);
            client.APIOptions.WorkspaceID = workspaceId;
            return client;
        }

        public static string GetArtifactName(this IHelper helper, int workspaceId, Guid artifactGuid)
        {
            var sql = @"select TextIDentifier from eddsdbo.Artifact a
                        join ArtifactGuid ag on a.ArtifactID = ag.ArtifactID
                        where ArtifactGuid = @ag";

            return helper.GetDBContext(workspaceId).ExecuteSqlStatementAsScalar<string>(sql, new SqlParameter("@ag", artifactGuid));
        }
        public static int GetArtifactId(this IHelper helper, int workspaceId, Guid artifactGuid)
        {
            var sql = @"select a.artifactId from eddsdbo.Artifact a
                        join ArtifactGuid ag on a.ArtifactID = ag.ArtifactID
                        where ArtifactGuid = @ag";

            return helper.GetDBContext(workspaceId).ExecuteSqlStatementAsScalar<int>(sql, new SqlParameter("@ag", artifactGuid));
        }
    }
}
