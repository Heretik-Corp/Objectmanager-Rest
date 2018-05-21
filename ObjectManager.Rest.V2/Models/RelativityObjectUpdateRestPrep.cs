using ObjectManager.Rest.Common;
using ObjectManager.Rest.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V2.Models
{
    internal class RelativityObjectUpdateRestPrep
    {
        internal class InternalLayoutDef
        {
            public int ArtifactId { get; set; }
        }
        internal class PrivateCallingContext
        {
            public InternalLayoutDef Layout { get; set; }

            public static PrivateCallingContext FromContext(CallingContext context)
            {
                if (context == null)
                {
                    return null;
                }
                return new PrivateCallingContext
                {
                    Layout = new InternalLayoutDef
                    {
                        ArtifactId = context.Layout.ArtifactId
                    }
                };
            }
        }
        internal class OperationOptionsRequest
        {
            public PrivateCallingContext CallingContext { get; set; }
        }

        internal class RestRequest
        {
            public RestObject Object { get; set; }
            public IEnumerable<RestField> FieldValues { get; set; }
        }
        public RestRequest Request { get; set; }
        public OperationOptionsRequest OperationOptions { get; set; }

        public static RelativityObjectUpdateRestPrep Prep(RelativityObject obj, CallingContext context)
        {
            var parser = new RestFieldParser();
            var ret = new RelativityObjectUpdateRestPrep();
            ret.Request = new RestRequest();
            ret.Request.Object = new RestObject(obj.ArtifactId);
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.FromFieldRef(x.Field, x.Value, parser)).ToList();
            ret.Request.FieldValues = fields;
            ret.OperationOptions = new OperationOptionsRequest { CallingContext = PrivateCallingContext.FromContext(context) };
            return ret;
        }
    }
}
