using System;
using kCura.EventHandler;
using kCura.Relativity.Client;

namespace TestEventhandlers
{
    public class PostSaveEventhandler : kCura.EventHandler.PostSaveEventHandler
    {
        public override FieldCollection RequiredFields => new FieldCollection();

        public override Response Execute()
        {
            var dbContext = this.Helper.GetDBContext(this.Helper.GetActiveCaseID());
            var artifactId = dbContext.GetArtifactId(Guid.Parse(ArtifactHelpers.LONG_TEXT_POST_SAVE));
            var layoutName = dbContext.GetArtifactName(Guid.Parse("63E66595-9E9A-4D58-9485-A3CDED8666A6"));

            if (this.ActiveLayout.Name == layoutName)
            {
                using (var client = this.Helper.GetServicesManager().CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
                {
                    client.APIOptions.WorkspaceID = this.Application.ArtifactID;
                    var result = client.Repositories
                        .Document
                        .Update(new kCura.Relativity.Client.DTOs.Document(this.ActiveArtifact.ArtifactID)
                    {
                        Fields = new System.Collections.Generic.List<kCura.Relativity.Client.DTOs.FieldValue>
                        {
                            new kCura.Relativity.Client.DTOs.FieldValue(Guid.Parse(ArtifactHelpers.LONG_TEXT_POST_SAVE), "Post save works")
                        }
                    });
                    if (!result.Success)
                    {
                        throw new Exception(result.Message);
                    }
                }
            }
            return new Response
            {
                Success = true
            };
        }
    }
}
