using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Common
{
    internal class RestField
    {
        public object Value { get; set; }
        public RField Field { get; set; }
        public static RestField FromFieldRef(FieldRef field, object value, RestFieldParser parser)
        {
            var restField = new RestField();
            restField.Field = parser.Parse(field);
            restField.Value = parser.ParseValue(value);
            return restField;
        }
    }
}
