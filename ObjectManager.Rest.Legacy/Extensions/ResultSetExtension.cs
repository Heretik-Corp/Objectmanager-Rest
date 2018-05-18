using kCura.Relativity.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.Legacy.Extensions
{
    internal static class ResultSetExtension
    {
        public static IEnumerable<T> EnsureSuccess<T>(this ResultSet<T> result) where T : Artifact
        {
            if (result == null)
            {
                throw new ApplicationException("An unknown error happend result object was null");
            }
            else if (!result.Success)
            {
                //ArtifactID 1058794 does not exist.'
                string message = result.Message;
                if (string.IsNullOrWhiteSpace(message) || (message ?? string.Empty).Contains("see individual results for more details"))
                {
                    message += string.Join(",", result.Results.Select(x => x.Message).Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "An unknown error happend message was null";
                }
                throw new ApplicationException(message);
            }
            return result.Results.Select(x => x.Artifact);
        }
    }
}
