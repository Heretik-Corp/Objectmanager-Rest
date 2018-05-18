using System;
using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces.Models
{
    public class ChoiceRef
    {
        public ChoiceRef() { }
        public ChoiceRef(int artifactID) => this.ArtifactId = artifactID;
        public ChoiceRef(string name) => this.Name = name;
        public ChoiceRef(IEnumerable<Guid> guids) => this.Guids = guids;
        public ChoiceRef(Guid guid) => this.Guids = new List<Guid> { guid };

        public int ArtifactId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Guids { get; set; }
    }
}
