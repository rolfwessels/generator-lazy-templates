using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Helpers
{
    public static class RestShapHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static Task<IRestResponse<T>> ExecuteAsyncWithLogging<T>(this RestClient client,
                                                                        RestRequest request) where T : new()
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();
            Method method = request.Method;
            Uri buildUri = client.BuildUri(request);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            string paramsSent =
                request.Parameters.Where(x => x.Name == "application/json").Select(x => x.Value).StringJoin();
            _log.Debug(string.Format("Sent {2} {1} [{0}]", paramsSent, buildUri, method));
            client.ExecuteAsync<T>(request, response =>
                {
                    stopwatch.Stop();
                    _log.Debug(string.Format("Response {2} {1} [{3}] [{0}]", response.Content, buildUri, method,
                                             stopwatch.ElapsedMilliseconds));
                    taskCompletionSource.SetResult(response);
                });

            return taskCompletionSource.Task;
        }
    }
}