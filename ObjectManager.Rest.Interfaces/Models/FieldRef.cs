using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces.Models
{
    public class FieldRef
    {
        public string FieldType { get; set; }
        public int ArtifactId { get; set; }
        public int ViewFieldId { get; set; }
        public IEnumerable<Guid> Guids { get; set; } = new List<Guid>();
        public string Name { get; set; }

        public FieldRef() { }
        public FieldRef(string fieldName)
        {
            this.Name = fieldName;
        }

        public FieldRef(Guid fieldGuid)
        {
            this.Guids = new List<Guid>
            {
                fieldGuid
            };
        }

        public FieldRef(int artifactId)
        {
            this.ArtifactId = artifactId;
        }
    }
}