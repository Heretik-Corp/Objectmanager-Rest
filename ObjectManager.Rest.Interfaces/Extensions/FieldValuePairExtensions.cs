using ObjectManager.Rest.Interfaces.Models;
using System;

namespace ObjectManager.Rest.Interfaces.Extensions
{
    public static class FieldValuePairExtensions
    {
        public static int ValueAsWholeNumber(this FieldValuePair pair)
        {
            return Convert.ToInt32(pair.Value);
        }
    }
}
