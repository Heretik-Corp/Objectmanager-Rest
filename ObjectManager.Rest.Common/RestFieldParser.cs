using System;
using System.Collections.Generic;
using System.Linq;
using ObjectManager.Rest.Extensions;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using static ObjectManager.Rest.RField;

namespace ObjectManager.Rest
{
    internal class RestFieldParser
    {
        internal class MultipleChoiceFieldUpdateValue
        {
            public MultipleChoiceFieldUpdateValue() { }

            public IEnumerable<RChoice> Choices { get; set; }

            public string Behavior { get; set; }
        }

        internal class MultipleObjectFieldUpdateValue
        {
            public MultipleObjectFieldUpdateValue() { }

            public IEnumerable<RField> Objects { get; set; }

            public string Behavior { get; set; }
        }

        public RField Parse(FieldRef field)
        {
            if (field.Guids?.Any() ?? false)
            {
                return this.ParseGuid(field);
            }
            if (field.ArtifactId > 0)
            {
                return this.ParseArtifactId(field);
            }
            if (!string.IsNullOrWhiteSpace(field.Name))
            {
                return this.ParseName(field);
            }
            throw new NotSupportedException("nothing was found");
        }

        public object ParseValue(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is ChoiceRef)
            {
                return this.ParseChoice((ChoiceRef)value);
            }
            else if (value.IsEnumerableOf<ChoiceRef>())
            {
                return ParseMultiChoice(((IEnumerable<ChoiceRef>)value));
            }
            else if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.ffZ");
            }
            else if (value is RelativityObject)
            {
                return ParseSingleObject((RelativityObject)value);
            }
            else if (value.IsEnumerableOf<RelativityObject>())
            {
                var r = value as IEnumerable<RelativityObject>;
                return ParseMultiObject(r);
            }
            return value;
        }
        protected virtual object ParseSingleObject(RelativityObject obj)
        {
            return new RField.ArtifactIdRestField(obj.ArtifactId);
        }

        protected virtual object ParseMultiObject(IEnumerable<RelativityObject> obj)
        {
            return obj.Select(x => new RField.ArtifactIdRestField(x.ArtifactId)).ToList();
        }

        protected virtual object ParseMultiChoice(IEnumerable<ChoiceRef> choices)
        {
            return choices.Select(x => this.ParseChoice(x)).ToList();
        }

        protected virtual RChoice ParseChoice(ChoiceRef choiceRef)
        {
            var c = choiceRef;
            if (c.ArtifactId > 0)
            {
                return new RChoice.ArtifactIdChoice(c.ArtifactId);
            }
            if (c.Guids?.Any() ?? false)
            {
                return new RChoice.GuidChoice(c.Guids.First());
            }
            throw new NotSupportedException("ArtifactId or Guid must be set on choice");
        }

        protected virtual RField ParseGuid(FieldRef field)
        {
            return new GuidRestField(field.Guids.First());
        }
        protected virtual RField ParseArtifactId(FieldRef field)
        {
            return new ArtifactIdRestField(field.ArtifactId);
        }
        protected virtual RField ParseName(FieldRef field)
        {
            return new NameRestField(field.Name);
        }
    }
}
