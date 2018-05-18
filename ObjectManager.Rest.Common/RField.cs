using System;

namespace ObjectManager.Rest
{
    internal class RField
    {
        internal class ArtifactIdRestField : RField
        {
            public ArtifactIdRestField(int artifactId) => this.ArtifactID = artifactId;
            public int ArtifactID { get; set; }
        }

        internal class NameRestField : RField
        {
            public NameRestField(string name) => this.Name = name;
            public string Name { get; set; }
        }

        internal class GuidRestField : RField
        {
            public GuidRestField(Guid g) => this.Guid = g;
            public Guid Guid { get; set; }
            public int ViewFieldID { get; } = 0;
        }
    }


}
