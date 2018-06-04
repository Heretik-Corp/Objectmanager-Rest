using System;
using System.Collections.Generic;
using System.Linq;
using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Extensions
{
    public static class DTOExtensions
    {
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
                // seems weird but staying consistant with how other versions of object manager 
                // work so all extension methods act as expected
                return Newtonsoft.Json.JsonConvert.SerializeObject(((Choice)value).ToChoiceRef());
            }
            if (value is MultiChoiceFieldValueList)
            {
                var c = (MultiChoiceFieldValueList)value;
                return Newtonsoft.Json.JsonConvert.SerializeObject(c.Select(x => x.ToChoiceRef()).ToList());
            }
            return value;
        }

        private static object ParseValue(object value)
        {
            if (value == null)
            {
                return value;
            }
            if (value is ChoiceRef)
            {
                return ((ChoiceRef)value).ToRelativityChoice();
            }
            else if (typeof(IEnumerable<ChoiceRef>).IsAssignableFrom(value.GetType()))
            {
                return ParseMultiChoice(((IEnumerable<ChoiceRef>)value));
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
            value.Value = ParseValue(pair.Value);
            Enum.TryParse<kCura.Relativity.Client.FieldType>(pair.Field.FieldType, out var fieldType);
            value.FieldType = fieldType;
            return value;
        }

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
            choiceRef.Guids = null;
            if (choice.Guids?.Any() ?? false)
            {
                choiceRef.Guids = choice.Guids?.ToList() ?? null;
            }
            choiceRef.Name = choice.Name;
            return choiceRef;
        }
    }
}
