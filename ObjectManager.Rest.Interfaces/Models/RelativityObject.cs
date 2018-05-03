using ObjectManager.Rest.Interfaces.Models;
using System.Collections.Generic;

namespace ObjectManager.Rest.Interfaces
{
    public class RelativityObject
    {
        public int ArtifactId { get; set; }
        public IEnumerable<FieldValuePair> FieldValues { get; set; }
    }
}