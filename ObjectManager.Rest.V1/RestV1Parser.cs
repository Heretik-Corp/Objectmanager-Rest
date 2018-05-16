using ObjectManager.Rest.Common;
using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V1
{
    internal class RestV1Parser : RestFieldParser
    {
        protected override RField ParseGuid(FieldRef field)
        {
            return new GuidRestField(field.Guids.ToList());
        }

        internal class GuidRestField : RField
        {
            public GuidRestField(IEnumerable<Guid> guids) => this.Guids = guids;
            public IEnumerable<Guid> Guids { get; set; }
        }
    }
}
