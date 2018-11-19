using System.Collections.Generic;
using System.Linq;
using ObjectManager.Rest.Common;

namespace ObjectManager.Rest.V1.Models
{
    public class ObectTypeRef
    {
        public int DescriptorArtifactTypeID { get; set; }
    }
    internal class RelativityObjectCreateRestPrep
    {
        public ObectTypeRef ObjectTypeRef { get; set; }
        public IEnumerable<RestField> FieldValuePairs { get; set; }

        public static RelativityObjectCreateRestPrep Prep(Interfaces.RelativityObject obj)
        {
            var parser = new RestV1Parser();
            var ret = new RelativityObjectCreateRestPrep();
            ret.ObjectTypeRef = new ObectTypeRef
            {
                DescriptorArtifactTypeID = obj.ObjectType.ArtifactTypeId
            };
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.FromFieldRef(x.Field, x.Value, parser)).ToList();
            ret.FieldValuePairs = fields; //?? new List<RestField> { new NameRestField("Artifact ID") };
            return ret;
        }
    }
}
