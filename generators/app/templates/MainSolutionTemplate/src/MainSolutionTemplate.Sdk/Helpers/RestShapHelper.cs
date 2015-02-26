using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Helpers
{
  public static class RestShapHelper
  {
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static IRestResponse<T> ExecuteWithLogging<T>(this RestClient client, RestRequest request) where T : new()
    {
      var method = request.Method;
      var buildUri = client.BuildUri(request);
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      _log.Debug(string.Format("Sent {2} {1} [{0}]", request.Parameters.Where(x => x.Name == "application/json").Select(x => x.Value).StringJoin(), buildUri, method));
      var restResponse = client.Execute<T>(request);
      stopwatch.Stop();

      _log.Debug(string.Format("Result {2} {1} [{3}] [{0}]", restResponse.Content, buildUri, method, stopwatch.ElapsedMilliseconds));

      return restResponse;
    }
  }
}
