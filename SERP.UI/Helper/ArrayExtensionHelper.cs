using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{
    public static class ArrayExtensionHelper
    {
        public static T ElementOrDefault<T>(this T[] array, int index)
        {
            if (index < array.GetLowerBound(0) || index > array.GetUpperBound(0))
            {
                return default;
            }
            return array[index];
        }
    }
}
