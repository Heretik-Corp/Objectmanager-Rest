using Relativity.API;
using System;
using System.Data.SqlClient;

namespace TestEventhandlers
{
    public static class SQLUtilities
    {
        public static int GetArtifactId(this IDBContext dbContext, Guid artifactGuid)
        {
            var artifactId = dbContext.ExecuteSqlStatementAsScalar<int>(
            "select ArtifactId from eddsdbo.ArtifactGuid with (nolock) where ArtifactGuid = @guid",
            new SqlParameter("@guid", artifactGuid));
            return artifactId;
        }

        public static string GetArtifactName(this IDBContext dbContext, Guid artifactGuid)
        {
            var artifactName = dbContext.ExecuteSqlStatementAsScalar<string>(
            @"select a.TextIdentifier from eddsdbo.ArtifactGuid ag with (nolock)
              join eddsdbo.Artifact a with(nolock) on a.ArtifactID = ag.ArtifactID
              where ArtifactGuid = @guid",
            new SqlParameter("@guid", artifactGuid));
            return artifactName;
        }
    }
}
