using ObjectManager.Rest.Interfaces.Models;
using System;

namespace ObjectManager.Rest.Common
{
    internal class RestField
    {
        public object Value { get; set; }
        public object Field { get; set; }
        public static RestField FromFieldRef(FieldRef field, object value, RestFieldParser parser)
        {
            var restField = new RestField();
            restField.Field = parser.Parse(field);
            if (value is DateTime)
            {
                value = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.ffZ");
            }
            restField.Value = value;
            return restField;

        }
    }
}
