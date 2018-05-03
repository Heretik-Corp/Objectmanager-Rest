using kCura.Relativity.Client;
using kCura.Relativity.Client.DTOs;
using Relativity.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ObjectManager.Integration.TestFixtures
{
    public class DocumentCreationSetupFixture : IDisposable
    {
        private readonly IHelper _helper;
        public IEnumerable<int> DocIds { get; private set; }
        public DocumentCreationSetupFixture(IHelper helper)
        {
            _helper = helper;
        }

        public void Create(int workspaceId, int count = 1)
        {
            using (var client = _helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
            {
                client.APIOptions.WorkspaceID = workspaceId;
                var docs = new List<Document>();
                for (var i = 0; i < count; i++)
                {
                    var file = Guid.NewGuid().ToString() + ".txt";
                    File.WriteAllText(file, string.Empty);
                    docs.Add(new Document()
                    {
                        TextIdentifier = Guid.NewGuid().ToString(),
                        RelativityNativeFileLocation = file
                    });
                }
                var result = client.Repositories.Document.Create(docs);

                if (!result.Success)
                {
                    //LAZY...
                    throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                }
                this.DocIds = result.Results.Select(x => x.Artifact.ArtifactID).ToList();
                docs.ForEach(x => File.Delete(x.RelativityNativeFileLocation));
            }

        }


        public void Dispose()
        {
            if (this.DocIds?.Any() ?? false)
            {
                using (var client = _helper.GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
                {
                    client.Repositories.Document.Delete(this.DocIds.ToArray());
                }
            }
        }
    }
}
