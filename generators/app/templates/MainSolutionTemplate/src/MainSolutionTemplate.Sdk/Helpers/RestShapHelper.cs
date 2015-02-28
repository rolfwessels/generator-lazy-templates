using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp.Portable;

namespace MainSolutionTemplate.Sdk.Helpers
{
    public static class RestShapHelper
    {

        public static async Task<IRestResponse<T>> ExecuteAsyncWithLogging<T>(this RestClient client,
                                                                              RestRequest request) where T : new()
        {


            Uri buildUri = client.BuildUri(request);
            var started = DateTime.Now;
            var paramsSent =
                request.Parameters.Where(x => x.Name == "application/json").Select(x => x.Value).StringJoin();
            client.IgnoreResponseStatusCode = true;
            if (Logger != null) Logger(string.Format("Sent [{2} {1}] [{0}]  ]", paramsSent, buildUri, request.Method));
            var execute = await client.Execute<T>(request);
            if (Logger != null)
                Logger(string.Format("Reci [{2} {1}] [{3}] [{0}]", execute.Content(), buildUri, request.Method,
                                     DateTime.Now - started));

            return execute;
        }

        public static string Content<T>(this IRestResponse<T> result)
        {
            return Encoding.UTF8.GetString(result.RawBytes, 0, result.RawBytes.Length);

        }

        public static Action<string> Logger { get; set; }
    }
}