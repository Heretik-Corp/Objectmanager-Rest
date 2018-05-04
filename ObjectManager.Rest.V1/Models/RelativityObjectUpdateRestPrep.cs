using ObjectManager.Rest.Common;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V1.Models
{
    internal class RelativityObjectUpdateRestPrep
    {
        public string ArtifactId { get; set; }
        public IEnumerable<RestField> FieldValuePairs { get; set; }

        public static RelativityObjectUpdateRestPrep PrepareForUpdateRequst(Interfaces.RelativityObject obj)
        {
            var ret = new RelativityObjectUpdateRestPrep();
            ret.ArtifactId = obj.ArtifactId.ToString();
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.FromFieldRef(x.Field, x.Value)).ToList();
            ret.FieldValuePairs = fields; //?? new List<RestField> { new NameRestField("Artifact ID") };
            return ret;
        }
    }
}
