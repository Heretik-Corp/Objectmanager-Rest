using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces.Models
{
    public class FieldValuePair
    {
        public int ArtifactId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Guids { get; set; }
    }
}