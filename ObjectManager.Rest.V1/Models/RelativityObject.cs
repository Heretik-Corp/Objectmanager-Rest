using ObjectManager.Rest.Common;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V1.Models
{
    internal class RelativityObjectV1
    {
        public int ArtifactId { get; set; }
        public IEnumerable<FieldValuePair> FieldValuePairs { get; set; }

        public Interfaces.RelativityObject ToCoreModel()
        {
            var ret = new Interfaces.RelativityObject();
            ret.ArtifactId = this.ArtifactId;
            ret.FieldValues = FieldValuePairs.ToList();
            return ret;
        }

        public static RelativityObjectV1 FromCoreModel(RelativityObject obj)
        {
            var ret = new RelativityObjectV1();
            ret.ArtifactId = obj.ArtifactId;
            ret.FieldValuePairs = obj.FieldValues.ToList();
            return ret;
        }
    }
}
