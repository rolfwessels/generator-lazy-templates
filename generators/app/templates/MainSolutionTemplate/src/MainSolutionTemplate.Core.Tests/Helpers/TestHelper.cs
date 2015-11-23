using System.Threading.Tasks;
using Moq.Language.Flow;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
    public static class TestHelper
    {
        public static void Returns<T1, T2>(this ISetup<T1, Task<T2>> setup, T2 dal) where T1 : class
        {
            setup.Returns(Task.FromResult(dal));
        }

    }
}