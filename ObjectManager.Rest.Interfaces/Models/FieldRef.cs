using System;

namespace ObjectManager.Rest.Interfaces.Models
{
    public class FieldRef
    {
        public int ArtifactId { get; set; }
        public string Name { get; set; }
        public Guid? Guid { get; set; }
    }
}