using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static ObjectManager.Rest.RField;

namespace ObjectManager.Rest
{
    public static class FieldUpdateBehavior
    {
        public const string Replace = "Replace";
        public const string Merge = "Merge";
    }

    internal class RestFieldParser
    {
        internal class MultipleChoiceFieldUpdateValue
        {
            public MultipleChoiceFieldUpdateValue() { }

            public IEnumerable<RChoice> Choices { get; set; }

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
            if (value is ChoiceRef)
            {
                return this.ParseChoice((ChoiceRef)value);
            }
            else if (typeof(IEnumerable<ChoiceRef>).IsAssignableFrom(value.GetType()))
            {
                return ((IEnumerable<ChoiceRef>)value).Select(x => this.ParseChoice(x)).ToList();
                //return new MultipleChoiceFieldUpdateValue
                //{
                //    Behavior = FieldUpdateBehavior.Replace,
                //    Choices = 
                //};
            }
            else if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.ffZ");
            }
            return value;
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
