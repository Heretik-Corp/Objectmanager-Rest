using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client.DTOs;
using Newtonsoft.Json.Linq;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Extensions
{
    public static class DTOExtensions
    {


        public static RelativityObject ToRelativityObject(this Document doc)
        {
            var obj = new RelativityObject();
            obj.ArtifactId = doc.ArtifactID;
            obj.FieldValues = doc.Fields.Select(x => ToFieldPair(x)).ToList();
            return obj;
        }

        public static Document ToDTODocument(this RelativityObject doc)
        {
            var retDoc = new Document();
            if (doc.ArtifactId > 0)
            {
                retDoc = new Document(doc.ArtifactId);
            }
            retDoc.Fields = doc.FieldValues.Select(x => ToFieldValue(x)).ToList();
            return retDoc;
        }

        public static Choice ToRelativityChoice(this ChoiceRef choiceRef)
        {
            if (choiceRef == null)
            {
                return null;
            }
            var choice = new Choice();
            if (choiceRef.ArtifactId > 0)
            {
                choice = new Choice(choiceRef.ArtifactId);
            }
            choice.Guids = choiceRef.Guids?.ToList() ?? new List<Guid>();
            choice.Name = choiceRef.Name;
            return choice;
        }

        public static ChoiceRef ToChoiceRef(this Choice choice)
        {
            var choiceRef = new ChoiceRef();
            choiceRef.ArtifactId = choice.ArtifactID;
            choiceRef.Guids = new List<Guid>();
            if (choice.Guids?.Any() ?? false)
            {
                choiceRef.Guids = choice.Guids?.ToList() ?? new List<Guid>();
            }
            choiceRef.Name = choice.Name;
            return choiceRef;
        }

        #region Private Parts
        private static FieldValuePair ToFieldPair(FieldValue fieldValue)
        {
            var pair = new FieldValuePair();
            pair.Value = ParseFieldValueValue(fieldValue.Value);
            var field = new FieldRef();
            field.Guids = fieldValue.Guids?.ToList() ?? new List<Guid>();
            field.ArtifactId = fieldValue.ArtifactID;
            field.Name = fieldValue.Name;
            pair.Field = field;
            return pair;

        }
        private static object ParseFieldValueValue(object value)
        {
            if (value is Choice)
            {
                return ((Choice)value).ToChoiceRef();
            }
            if (value is MultiChoiceFieldValueList)
            {
                var c = (MultiChoiceFieldValueList)value;
                return c.Select(x => x.ToChoiceRef()).ToList();
            }
            if (value is Artifact)
            {
                var v = value as Artifact;
                return new RelativityObject
                {
                    ArtifactId = v.ArtifactID
                };
            }
            if (value.IsEnumerableOf<Artifact>())
            {
                var v = value as IEnumerable<Artifact>;
                return v.Select(x => new RelativityObject
                {
                    ArtifactId = x.ArtifactID
                }).ToList();
            }
            return value;
        }
        internal class SerializedArtifact
        {
            public int ArtifactId { get; set; }
            public string Name { get; set; }
            public IEnumerable<Guid> Guids { get; set; }
        }
        private static object ParseValue(object value, kCura.Relativity.Client.FieldType fieldType)
        {
            var vType = (value?.GetType() ?? null);
            if (value == null)
            {
                return value;
            }
            if (value is ChoiceRef)
            {
                return ((ChoiceRef)value).ToRelativityChoice();
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.SingleChoice && ((vType == typeof(string)) || vType == typeof(JObject)))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ChoiceRef>(value?.ToString() ?? string.Empty).ToRelativityChoice();
            }
            else if (value.IsEnumerableOf<ChoiceRef>())
            {
                return ParseMultiChoice(((IEnumerable<ChoiceRef>)value));
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.MultipleChoice && ((vType == typeof(string)) || vType == typeof(JArray)))
            {
                return ParseMultiChoice(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChoiceRef>>(value.ToString() ?? string.Empty));
            }
            else if (value is RelativityObject)
            {
                var v = (RelativityObject)value;
                return new RDO(v.ArtifactId);
            }
            else if (value.IsEnumerableOf<RelativityObject>())
            {
                var v = (IEnumerable<RelativityObject>)value;
                return new FieldValueList<Artifact>(v.Select(x => new RDO(x.ArtifactId)).ToList());
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.SingleObject && (vType == typeof(JObject)))
            {
                var r = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedArtifact>(value?.ToString() ?? string.Empty);
                return new Artifact(r.ArtifactId)
                {
                    TextIdentifier = r.Name,
                    Guids = r.Guids?.ToList() ?? new List<Guid>()
                };
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.MultipleObject && (vType == typeof(JArray)))
            {
                var r = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SerializedArtifact>>(value?.ToString() ?? string.Empty);
                return new FieldValueList<Artifact>(r.Select(x => new Artifact(x.ArtifactId)
                {
                    TextIdentifier = x.Name,
                    Guids = x.Guids?.ToList() ?? new List<Guid>()
                }));
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.Decimal || fieldType == kCura.Relativity.Client.FieldType.Currency)
            {
                if (!decimal.TryParse(value?.ToString() ?? string.Empty, out var d))
                {
                    throw new NotSupportedException($"{value} cannot be parsed by decimal.");
                }
                return d;
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.WholeNumber)
            {
                if (!int.TryParse(value?.ToString() ?? string.Empty, out var d))
                {
                    throw new NotSupportedException($"{value} cannot be parsed by integer.");
                }
                return d;
            }
            else if (fieldType == kCura.Relativity.Client.FieldType.User)
            {
                var r = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedArtifact>(value?.ToString() ?? string.Empty);
                return new User(r.ArtifactId)
                {
                    TextIdentifier = r.Name,
                    Guids = r.Guids?.ToList() ?? new List<Guid>()
                };
            }
            return value;
        }

        private static object ParseMultiChoice(IEnumerable<ChoiceRef> value)
        {
            return new MultiChoiceFieldValueList(value.Select(x => ToRelativityChoice(x)).ToList());
        }

        private static FieldValue ToFieldValue(FieldValuePair pair)
        {
            var value = new FieldValue();
            if (pair.Field == null)
            {
                throw new NullReferenceException("No field loaded on fieldValuePair");
            }
            value.Guids = pair.Field.Guids?.ToList() ?? new List<Guid>();
            value.ArtifactID = pair.Field.ArtifactId;
            value.Name = pair.Field.Name;
            Enum.TryParse<kCura.Relativity.Client.FieldType>(pair.Field.FieldType, out var fieldType);
            value.Value = ParseValue(pair.Value, fieldType);

            value.FieldType = fieldType;
            return value;
        }
        #endregion
    }
}
