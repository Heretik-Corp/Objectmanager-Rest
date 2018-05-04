using ObjectManager.Rest.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V1.Models
{
    internal class RelativityObjectRestReadPrep
    {
        public IEnumerable<object> FieldRefs { get; set; }

        public CallingContext CallingContext { get; set; }

        public static RelativityObjectRestReadPrep PrepareForReadRequst(Interfaces.RelativityObject obj, CallingContext context)
        {
            var ret = new RelativityObjectRestReadPrep();
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.GetField(x.Field)).ToList();
            ret.FieldRefs = fields ?? new List<object> { new NameRestField("Artifact Id") };
            ret.CallingContext = context;
            return ret;
        }
    }
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
