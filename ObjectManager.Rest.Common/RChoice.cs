using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest
{
    internal class RChoice
    {
        public class ArtifactIdChoice : RChoice
        {
            public ArtifactIdChoice(int artifactId) => this.ArtifactID = artifactId;
            public int ArtifactID { get; set; }
        }
        public class GuidChoice : RChoice
        {
            public GuidChoice(Guid guid) => this.Guid = guid;
            public Guid Guid { get; set; }
        }

        public class GuidsChoice : RChoice
        {
            public GuidsChoice(IEnumerable<Guid> guids) => this.Guids = guids.ToList();
            public IEnumerable<Guid> Guids { get; set; }
        }
    }


}
