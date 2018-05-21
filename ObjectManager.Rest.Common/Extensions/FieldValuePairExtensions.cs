﻿using ObjectManager.Rest.Interfaces.Models;
using System;
using System.Collections.Generic;

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
            var choice = Newtonsoft.Json.JsonConvert.DeserializeObject<ChoiceRef>(pair.Value.ToString());
            return choice;
        }

        public static IEnumerable<ChoiceRef> ValueAsMultiChoice(this FieldValuePair pair)
        {
            if (pair.Value == null)
            {
                return null;
            }
            var choices = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ChoiceRef>>(pair.Value.ToString());
            return choices;
        }
    }
}