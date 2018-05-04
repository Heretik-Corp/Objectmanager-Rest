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
    }
}