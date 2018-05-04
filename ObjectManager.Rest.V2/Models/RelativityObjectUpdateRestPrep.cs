using ObjectManager.Rest.Common;
using ObjectManager.Rest.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V2.Models
{
    internal class RelativityObjectUpdateRestPrep
    {
        internal class OperationOptionsRequest
        {
            public CallingContext CallingContext { get; set; }

        }

        internal class RestRequest
        {
            public RestObject Object { get; set; }
            public IEnumerable<RestField> FieldValues { get; set; }
        }
        public RestRequest Request { get; set; }
        public OperationOptionsRequest OperationOptions { get; set; }

        public static RelativityObjectUpdateRestPrep Prep(RelativityObject obj)
        {
            var ret = new RelativityObjectUpdateRestPrep();
            ret.Request = new RestRequest();
            ret.Request.Object = new RestObject(obj.ArtifactId);
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.FromFieldRef(x.Field, x.Value)).ToList();
            ret.Request.FieldValues = fields;
            return ret;
        }
    }
}
