using System.Collections.Generic;
using System.Linq;
using ObjectManager.Rest.Common;
using ObjectManager.Rest.Interfaces;
using static ObjectManager.Rest.V2.Models.RelativityObjectUpdateRestPrep;

namespace ObjectManager.Rest.V2.Models
{
    public class ObjectType
    {
        public int ArtifactTypeId { get; set; }
    }

    internal class RelativityObjectRestCreatePrep
    {
        public class UpdateRequestObj
        {
            public ObjectType ObjectType { get; set; }
            public IEnumerable<RestField> FieldValues { get; set; }
        }

        public UpdateRequestObj Request { get; set; }
        public OperationOptionsRequest OperationOptions { get; set; }

        public static RelativityObjectRestCreatePrep Prep(RelativityObject obj, CallingContext context)
        {
            var parser = new RestFieldParser();
            var ret = new RelativityObjectRestCreatePrep();
            ret.Request = new UpdateRequestObj();
            ret.Request.ObjectType = new ObjectType
            {
                ArtifactTypeId = obj.ObjectType.ArtifactTypeId
            };
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => RestField.FromFieldRef(x.Field, x.Value, parser)).ToList();
            ret.Request.FieldValues = fields;
            ret.OperationOptions = new OperationOptionsRequest { CallingContext = PrivateCallingContext.FromContext(context) };
            return ret;
        }

    }
}
