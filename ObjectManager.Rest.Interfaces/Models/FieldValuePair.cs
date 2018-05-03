using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces.Models
{
    public class FieldValuePair
    {
        public FieldValue Field { get; set; }
    }
    public class FieldValue
    {
        public string FieldType { get; set; }
        public int ArtifactId { get; set; }
        public int ViewFieldId { get; set; }
        public IEnumerable<Guid> Guids { get; set; }
        public string Name { get; set; }
    }
}