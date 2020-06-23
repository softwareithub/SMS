using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{
    public static class EmptyToDefaultHelper
    {
        public static T EmptyToDefault<T>(this string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
