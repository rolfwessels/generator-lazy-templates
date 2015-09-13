using System;

namespace MainSolutionTemplate.Core.Tests.Helpers
{
    public static class TimerHelper
    {
        public static void WaitFor<T>(this T updateModels, Func<T, bool> o, int timeOut = 500)
        {
            var stopTime = DateTime.Now.AddMilliseconds(timeOut);
            bool result;
            do
            {
                result = o(updateModels);
            }
            while (!result && stopTime > DateTime.Now);
        }
    }
}