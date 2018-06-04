using System;
using kCura.EventHandler;

namespace TestEventhandlers
{
    public class PresaveEventHandlerYesNo_1 : PreSaveEventHandler
    {
        public override FieldCollection RequiredFields => new FieldCollection();

        public override Response Execute()
        {
            var dbContext = this.Helper.GetDBContext(this.Helper.GetActiveCaseID());
            var artifactId = dbContext.GetArtifactId(Guid.Parse(ArtifactHelpers.YES_NO_FIELD));
            var layoutName = dbContext.GetArtifactName(Guid.Parse("61F1A01E-15DA-4DFD-B3E6-CEF757E39F2B"));
            if (this.ActiveLayout.Name == layoutName &&
    this.ActiveArtifact.IsLoaded(artifactId) &&
    this.ActiveArtifact.GetValue(artifactId) == true)
            {
                return new Response
                {
                    Success = false,
                    Message = "Value cannot be yes 1."
                };
            }
            return new Response
            {
                Success = true
            };
        }
    }
}
