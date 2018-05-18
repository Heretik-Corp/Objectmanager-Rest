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
            var parser = new RestV1Parser();
            var ret = new RelativityObjectRestReadPrep();
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => parser.Parse(x.Field)).ToList();
            if (!(fields?.Any() ?? false))
            {
                fields = new List<RField> { new RField.NameRestField("Artifact Id") };
            }
            ret.FieldRefs = fields;
            ret.CallingContext = context;
            return ret;
        }
    }
}
