using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using log4net;
using MainSolutionTemplate.Api.WebApi.Filters;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSolutionTemplate.Api.SignalR.Modules
{
    public class HubErrorModule : HubPipelineModule
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            MethodDescriptor method = invokerContext.MethodDescriptor;

            var exception = exceptionContext.Error;
            if (exception is ApiException)
            {
                _log.Warn(string.Format("{0}:{1} {2}", method.Hub.Name, method.Name, exception.Message));
            }
            else if (CaptureExceptionFilter.IsSomeSortOfValidationError(exception))
            {
                _log.Warn(string.Format("{0}:{1} {2}", method.Hub.Name, method.Name, exception.Message));
            }
            if (exception is ValidationException)
            {
                var validationException = (ValidationException) exception;
                _log.Warn(string.Format("{0}:{1} {2}", method.Hub.Name, method.Name, exception.Message));
                exceptionContext.Error =
                   new Exception(validationException.Errors.Select(x=>x.ErrorMessage).FirstOrDefault());
            }
            else
            {
                _log.Error(string.Format("{0}.{1}({2}) throw exception [{3}]", method.Hub.Name,
                method.Name,
                string.Join(", ", invokerContext.Args), exception.Message),
                 exception);
                exceptionContext.Error =
                    new Exception("An internal system error has occurred. The developers have been notified.");
            }
        }
    }
}