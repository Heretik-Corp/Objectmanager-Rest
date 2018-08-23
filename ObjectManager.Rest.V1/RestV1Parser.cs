using System;
using System.Collections.Generic;
using System.Linq;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;

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
        protected override object ParseMultiChoice(IEnumerable<ChoiceRef> choices)
        {
            return new MultipleChoiceFieldUpdateValue
            {
                Choices = (IEnumerable<RChoice>)base.ParseMultiChoice(choices),
                Behavior = FieldUpdateBehavior.Replace
            };
        }
        protected override object ParseMultiObject(IEnumerable<RelativityObject> obj)
        {
            return new MultipleObjectFieldUpdateValue
            {
                Objects = (IEnumerable<RField>)base.ParseMultiObject(obj),
                Behavior = FieldUpdateBehavior.Replace
            };
        }
        internal class GuidRestField : RField
        {
            public GuidRestField(IEnumerable<Guid> guids) => this.Guids = guids;
            public IEnumerable<Guid> Guids { get; set; }
        }
    }
}
