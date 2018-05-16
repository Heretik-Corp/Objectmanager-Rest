using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Linq;

namespace ObjectManager.Rest.Common
{
    internal class RestFieldParser
    {
        public RField Parse(FieldRef field)
        {
            if (field.Guids?.Any() ?? false)
            {
                return this.ParseGuid(field);
            }
            if (field.ArtifactId > 0)
            {
                return this.ParseArtifactId(field);
            }
            if (!string.IsNullOrWhiteSpace(field.Name))
            {
                return this.ParseName(field);
            }
            throw new NotSupportedException("nothing was found");
        }

        protected virtual RField ParseGuid(FieldRef field)
        {
            return new GuidRestField(field.Guids.First());
        }
        protected virtual RField ParseArtifactId(FieldRef field)
        {
            return new ArtifactIdRestField(field.ArtifactId);
        }
        protected virtual RField ParseName(FieldRef field)
        {
            return new NameRestField(field.Name);
        }

    }

    internal class RField
    {

    }

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
