using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V1.Models
{
    internal class RestField
    {
        public string Value { get; set; }
        public object Field { get; set; }
        public static RestField FromFieldRef(FieldRef field, string value)
        {
            var restField = new RestField();
            restField.Field = GetField(field);
            restField.Value = value;
            return restField;

        }
        public static object GetField(FieldRef field)
        {
            if (field.Guids?.Any() ?? false)
            {
                return new GuidRestField(field.Guids.First());
            }
            if (field.ArtifactId > 0)
            {
                return new ArtifactIdRestField(field.ArtifactId);
            }
            if (!string.IsNullOrWhiteSpace(field.Name))
            {
                return new NameRestField(field.Name);
            }
            throw new NotSupportedException("nothing was found");
        }
    }

    internal class ArtifactIdRestField
    {
        public ArtifactIdRestField(int artifactId) => this.ArtifactId = artifactId;
        public int ArtifactId { get; set; }
    }

    internal class NameRestField
    {
        public NameRestField(string name) => this.Name = name;
        public string Name { get; set; }
    }

    internal class GuidRestField
    {
        public GuidRestField(Guid g) => this.Guid = g;
        public Guid Guid { get; set; }
    }



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
