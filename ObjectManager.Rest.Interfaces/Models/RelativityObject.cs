using System;
using System.Collections.Generic;
using System.Linq;
using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Interfaces
{
    public class RelativityObject
    {
        public int ArtifactId { get; set; }
        public IEnumerable<FieldValuePair> FieldValues { get; set; } = new List<FieldValuePair>();

        public FieldValuePair this[string fieldName]
        {
            get
            {
                var field = this.FieldValues.FirstOrDefault(x => x?.Field?.Name?.Equals(fieldName, System.StringComparison.CurrentCultureIgnoreCase) ?? false);
                if (field == null)
                {
                    //TODO: better error message
                    throw new System.Exception("Field not loaded");
                }
                return field;
            }
        }

        public FieldValuePair this[Guid guid]
        {
            get
            {
                var field = this.FieldValues.FirstOrDefault(x => x?.Field?.Guids?.Contains(guid) ?? false);
                if (field == null)
                {
                    //TODO: better error message
                    throw new System.Exception("Field not loaded");
                }
                return field;
            }
        }
        public FieldValuePair this[int artifactId]
        {
            get
            {
                var field = this.FieldValues.FirstOrDefault(x => (x?.Field?.ArtifactId == artifactId));
                if (field == null)
                {
                    //TODO: better error message
                    throw new System.Exception("Field not loaded");
                }
                return field;
            }
        }
    }
}