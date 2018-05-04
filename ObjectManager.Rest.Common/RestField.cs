using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Common
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
            return RestFieldHelpers.Parse(field);
        }
    }
}
