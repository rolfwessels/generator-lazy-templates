using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using MainSolutionTemplate.Api.WebApi.Exceptions;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Utilities.Helpers;
using log4net;

namespace MainSolutionTemplate.Api.WebApi.Filters
{
	public class CaptureExceptionFilter : ExceptionFilterAttribute
  {
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public override void OnException(HttpActionExecutedContext context)
    {
      Exception exception = context.Exception.ToFirstExceptionOfException();

      if (exception is ApiException)
      {
        RespondWithTheExceptionMessage(context, exception);
      }
      else if (IsSomeSortOfValidationError(exception))
      {
        RespondWithBadRequest(context, exception);
      }
      else
      {
        RespondWithInternalServerException(context, exception);
      }
    }

    #region Private Methods

    private void RespondWithInternalServerException(HttpActionExecutedContext context, Exception exception)
    {
      _log.Error(exception.Message, exception);
      const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
      var errorMessage = new ErrorMessage("An internal system error has occurred. The developers have been notified.");
#if DEBUG
      errorMessage.AdditionalDetail = exception.Message;
#endif
      context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
    }

    private static void RespondWithTheExceptionMessage(HttpActionExecutedContext context, Exception exception)
    {
      HttpStatusCode httpStatusCode = (exception as ApiException).HttpStatusCode;
      var errorMessage = new ErrorMessage(exception.Message);
      context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
    }

    private static void RespondWithBadRequest(HttpActionExecutedContext context, Exception exception)
    {
      const HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
      var errorMessage = new ErrorMessage(exception.Message);
      context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
    }

    private static bool IsSomeSortOfValidationError(Exception exception)
    {
      return exception is ValidationException || exception is ArgumentException ||
             exception is ArgumentOutOfRangeException || exception is ArgumentNullException;
    }

    #endregion
  }
}