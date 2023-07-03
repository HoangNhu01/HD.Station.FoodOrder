using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Extensions
{
    public static class DictionaryExtensions
    {
        public static object ToAnonymousObject(this IDictionary<string, object> dict)
        {
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            foreach (var kvp in dict)
            {
                eoColl.Add(kvp);
            }
            return eo as dynamic;
        }
    }
}
