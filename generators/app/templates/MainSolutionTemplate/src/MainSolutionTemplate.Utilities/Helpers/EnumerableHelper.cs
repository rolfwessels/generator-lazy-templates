using System;
using System.Collections.Generic;
using System.Linq;

namespace MainSolutionTemplate.Utilities.Helpers
{
	public static class EnumerableHelper
	{
		public static string StringJoin(this IEnumerable<object> values, string separator = ", ")
		{
			if (values == null) return null;
			var array = values.Select(x => x.ToString()).ToArray();
			return string.Join(separator, array);
		}


        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> values, Action<T> call)
		{
			if (values == null) return null;
            foreach (var value in values)
            {
                call(value);
            }
            return values;
		}

		
	}
	
	
}