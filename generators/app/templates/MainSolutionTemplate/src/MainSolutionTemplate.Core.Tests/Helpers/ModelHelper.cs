using FizzWare.NBuilder;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
	public static class ModelHelper
	{
		public static ISingleObjectBuilder<T> WithValidData<T>(ISingleObjectBuilder<T> createNew)
		{
			return createNew;
		}
	}
}