using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Linq;

namespace ObjectManager.Rest.Common
{
    internal static class RestFieldHelpers
    {
        public static object Parse(FieldRef field)
        {
            if (field.Guids?.Any() ?? false)
            {
                return new GuidRestField(field.Guids.First());
            }
            if (field.ArtifactId > 0)
            {
                return new ArtifactIdRestField(field.ArtifactId);
            }
            if (!string.IsNullOrWhiteSpace(field.Name))
            {
                return new NameRestField(field.Name);
            }
            throw new NotSupportedException("nothing was found");
        }
    }
    internal class ArtifactIdRestField
    {
        public ArtifactIdRestField(int artifactId) => this.ArtifactId = artifactId;
        public int ArtifactId { get; set; }
    }

    internal class NameRestField
    {
        public NameRestField(string name) => this.Name = name;
        public string Name { get; set; }
    }

    internal class GuidRestField
    {
        public GuidRestField(Guid g) => this.Guid = g;
        public int ArtifactId { get; } = 0;
        public Guid Guid { get; set; }
        public int ViewFieldID { get; } = 0;
    }
}
