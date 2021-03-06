﻿using kCura.EventHandler;

namespace TestEventhandlers
{
    public static class ArtifactHelpers
    {
        public const string YES_NO_FIELD = "1EE3A20E-F2A9-4233-8C81-5D399A0CFF8C";
        public const string LONG_TEXT_POST_SAVE = "605435EC-6555-4AC5-8A2A-6A9F448106C4";
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
}
