using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace MainSolutionTemplate.Api.WebApi.ODataSupport
{
  public class QueryToODataFilterAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    {
      var pagedResult = GetQueryToODataWrapper(actionExecutedContext);
      base.OnActionExecuted(actionExecutedContext);
        
      if (ResponseIsValid(actionExecutedContext.Response))
      {
        if (pagedResult != null && pagedResult.RequiresPagedValue)
        {

          actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, pagedResult.GetPagedResult());
        }
      }
    }

    private static IQueryToODataWrapper GetQueryToODataWrapper(HttpActionExecutedContext actionExecutedContext)
    {
      object responseObject = null;
      if (actionExecutedContext.Response != null) actionExecutedContext.Response.TryGetContentValue(out responseObject);

      var pagedResult = responseObject as IQueryToODataWrapper;
      return pagedResult;
    }

    private bool ResponseIsValid(HttpResponseMessage response)
    {
      if (response == null || response.StatusCode != HttpStatusCode.OK || !(response.Content is ObjectContent)) return false;
      return true;
    }
  }

  
}