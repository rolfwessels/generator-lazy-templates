using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using FluentValidation;
using GoogleAnalyticsTracker.WebApi2;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Utilities.Helpers;
using log4net;
using System.Linq;

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
            else if (exception is ValidationException)
            {
                RespondWithValidationRequest(context, exception as ValidationException);
            }
            else
            {
                RespondWithInternalServerException(context, exception);
            }
        }

        #region Private Methods

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

        public static bool IsSomeSortOfValidationError(Exception exception)
        {
            return exception is System.ComponentModel.DataAnnotations.ValidationException ||
                   exception is ArgumentException ||
                   exception is ArgumentOutOfRangeException || exception is ArgumentNullException;
        }

        private void RespondWithValidationRequest(HttpActionExecutedContext context,
                                                  ValidationException validationException)
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            var errorMessage = new ErrorMessage(validationException.Errors.Select(x=>x.ErrorMessage).FirstOrDefault());
            context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
        }

        private void RespondWithInternalServerException(HttpActionExecutedContext context, Exception exception)
        {
            var resolve = IocApi.Instance.Resolve<Tracker>();
            resolve.TrackEventAsync("Exception", context.Request.RequestUri.PathAndQuery,
                                    new Dictionary<string, string>() { { "Message", exception.Message }, { "StackTrace", exception.StackTrace } });
            _log.Error(exception.Message, exception);
            const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            var errorMessage =
                new ErrorMessage("An internal system error has occurred. The developers have been notified.");

#if DEBUG
            errorMessage.AdditionalDetail = exception.Message;
#endif
            context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
        }

       

        #endregion
    }
}