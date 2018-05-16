using ObjectManager.Rest.Common;
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
            ret.FieldRefs = fields ?? new List<RField> { new NameRestField("Artifact Id") };
            ret.CallingContext = context;
            return ret;
        }
    }
}
