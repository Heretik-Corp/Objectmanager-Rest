using kCura.Relativity.Client.DTOs;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Linq;

namespace ObjectManager.Rest.Legacy
{
    internal static class DTOHelpers
    {
        public static Document ConvertToDocument(RelativityObject obj)
        {
            Document dto;
            if (obj.ArtifactId > 0)
            {
                dto = new Document(obj.ArtifactId);
            }
            else
            {
                dto = new Document();
            }
            dto.Fields = obj.FieldValues?.Select(x => Convert(x.Field, x.Value)).ToList() ?? FieldValue.NoFields;
            return dto;
        }

        private static FieldValue Convert(FieldRef fieldRef, object value)
        {
            FieldValue v = null;
            if (fieldRef.Guids?.Any() ?? false)
            {
                v = new FieldValue(fieldRef.Guids.First());
            }
            else if (fieldRef.ArtifactId > 0)
            {
                v = new FieldValue(fieldRef.ArtifactId);
            }
            else if (string.IsNullOrWhiteSpace(fieldRef.Name))
            {
                v = new FieldValue(fieldRef.Name);
            }
            if (v == null)
            {
                throw new NotSupportedException();
            }
            v.Value = value;
            return v;
        }
    }
}
