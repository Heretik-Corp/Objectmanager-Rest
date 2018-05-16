using ObjectManager.Rest.Interfaces.Models;
using System;

namespace ObjectManager.Rest.Common
{
    internal class RestField
    {
        public object Value { get; set; }
        public object Field { get; set; }
        public static RestField FromFieldRef(FieldRef field, object value)
        {
            var restField = new RestField();
            restField.Field = GetField(field);
            if (value is DateTime)
            {
                value = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.ffZ");
            }
            restField.Value = value;
            return restField;

        }
        public static object GetField(FieldRef field)
        {
            return RestFieldHelpers.Parse(field);
        }
    }
}
