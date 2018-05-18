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

        protected override RChoice ParseChoice(ChoiceRef choiceRef)
        {
            if (choiceRef.Guids?.Any() ?? false)
            {
                return new RChoice.GuidsChoice(choiceRef.Guids.ToList());
            }
            return base.ParseChoice(choiceRef);
        }
        internal class GuidRestField : RField
        {
            public GuidRestField(IEnumerable<Guid> guids) => this.Guids = guids;
            public IEnumerable<Guid> Guids { get; set; }
        }
    }
}
