using System;
using kCura.EventHandler;

namespace TestEventhandlers
{
    public static class ArtifactHelpers
    {
        public static bool IsLoaded(this Artifact artifact, int artifactId)
        {
            return artifact.Fields[artifactId] != null;
        }
        public static bool GetValue(this Artifact artifact, int artifactId)
        {
            if (artifact.Fields[artifactId].Value.Value == null)
            {
                return false;
            }
            return (bool)artifact.Fields[artifactId].Value.Value;
        }
    }
    public class PreSaveEventhandler : PreSaveEventHandler
    {
        public override FieldCollection RequiredFields => new FieldCollection();

        public override Response Execute()
        {
            var yesNoFieldGuid = Guid.Parse("1EE3A20E-F2A9-4233-8C81-5D399A0CFF8C");
            var dbContext = this.Helper.GetDBContext(this.Helper.GetActiveCaseID());
            var artifactId = dbContext.GetArtifactId(yesNoFieldGuid);
            var layoutName = dbContext.GetArtifactName(Guid.Parse("A70ED5F2-47CF-4DED-985E-2DABE6679C5C"));

            if (this.ActiveLayout.Name == layoutName &&
                this.ActiveArtifact.IsLoaded(artifactId) &&
                this.ActiveArtifact.GetValue(artifactId) == true)
            {
                return new Response
                {
                    Success = false,
                    Message = "There is an error saving this layout yes no Field is required"
                };
            }
            return new Response
            {
                Success = true
            };
        }
    }
}
