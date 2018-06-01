using System;
using kCura.EventHandler;

namespace TestEventhandlers
{
    public class PreLoadEventhandler : kCura.EventHandler.PreLoadEventHandler
    {
        public override FieldCollection RequiredFields { get; }

        public override Response Execute()
        {
            var textFieldGuid = Guid.Parse("5D7F851C-2432-4B24-A49B-BB53E68ECFC6");
            var dbContext = this.Helper.GetDBContext(this.Helper.GetActiveCaseID());
            var artifactId = dbContext.GetArtifactId(textFieldGuid);
            var layoutName = dbContext.GetArtifactName(Guid.Parse("F59829F7-CCAF-4F1C-8439-FA7EB33A8ECE"));

            if (this.ActiveLayout.Name == layoutName && this.ActiveArtifact.IsLoaded(artifactId))
            {
                this.ActiveArtifact.Fields[artifactId].Value.Value = "pre load value";
            }

            return new Response
            {
                Success = true
            };
        }
    }
}
