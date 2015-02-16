using System.Collections.Generic;
using System.Linq;

namespace MainSolutionTemplate.Core.Helpers
{
	public static class EnumerableHelper
	{
		public static string StringJoin(this IEnumerable<object> values, string separator = ", ")
		{
			if (values == null) return null;
			var array = values.Select(x => x.ToString()).ToArray();
			return string.Join(separator, array);
		}

		
	}
	
	
}