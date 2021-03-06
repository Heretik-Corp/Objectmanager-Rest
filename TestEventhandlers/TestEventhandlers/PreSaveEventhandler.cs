﻿using System;
using kCura.EventHandler;

namespace TestEventhandlers
{
    public class PreSaveEventhandler : PreSaveEventHandler
    {
        public override FieldCollection RequiredFields => new FieldCollection();

        public override Response Execute()
        {
            var dbContext = this.Helper.GetDBContext(this.Helper.GetActiveCaseID());
            var artifactId = dbContext.GetArtifactId(Guid.Parse(ArtifactHelpers.YES_NO_FIELD));
            var layoutName = dbContext.GetArtifactName(Guid.Parse("A70ED5F2-47CF-4DED-985E-2DABE6679C5C"));

            if (this.ActiveLayout.Name == layoutName &&
                this.ActiveArtifact.IsLoaded(artifactId) &&
                this.ActiveArtifact.GetValue(artifactId) == true)
            {
                return new Response
                {
                    Success = false,
                    Message = "There is an error saving this layout value of yes no cannot be 'Yes'."
                };
            }
            return new Response
            {
                Success = true
            };
        }
    }
}
