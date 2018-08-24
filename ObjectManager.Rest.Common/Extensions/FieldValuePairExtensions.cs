using System;
using System.Collections.Generic;
using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;

namespace ObjectManager.Rest.Extensions
{
    public static class FieldValuePairExtensions
    {
        public static int ValueAsWholeNumber(this FieldValuePair pair)
        {
            return Convert.ToInt32(pair.Value);
        }
        public static ChoiceRef ValueAsSingleChoice(this FieldValuePair pair)
        {
            if (pair.Value == null)
            {
                return null;
            }
            if (pair.Value is ChoiceRef)
            {
                return pair.Value as ChoiceRef;
            }
            var choice = Newtonsoft.Json.JsonConvert.DeserializeObject<ChoiceRef>(pair.Value.ToString());
            return choice;
        }

        public static RelativityObject ValueAsSingleObject(this FieldValuePair pair)
        {
            if (pair.Value == null)
            {
                return null;
            }
            if (pair.Value is RelativityObject)
            {
                return pair.Value as RelativityObject;
            }
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<RelativityObject>(pair.Value.ToString());
            return obj;
        }

        public static IEnumerable<RelativityObject> ValueAsMultiObject(this FieldValuePair pair)
        {
            if (pair.Value == null)
            {
                return null;
            }
            if (pair.Value.IsEnumerableOf<RelativityObject>())
            {
                return pair.Value as IEnumerable<RelativityObject>;
            }
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<RelativityObject>>(pair.Value.ToString());
            return obj;
        }

        public static IEnumerable<ChoiceRef> ValueAsMultiChoice(this FieldValuePair pair)
        {
            if (pair.Value == null)
            {
                return null;
            }
            else if (pair.Value.IsEnumerableOf<ChoiceRef>())
            {
                return pair.Value as IEnumerable<ChoiceRef>;
            }
            var choices = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChoiceRef>>(pair.Value.ToString());
            return choices;
        }

        internal static bool IsEnumerableOf<T>(this object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return typeof(IEnumerable<T>).IsAssignableFrom(obj?.GetType());
        }
    }
}
