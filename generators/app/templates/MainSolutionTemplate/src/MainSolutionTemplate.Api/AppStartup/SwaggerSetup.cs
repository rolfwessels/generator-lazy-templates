using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Helpers;
using Swashbuckle;
using Swashbuckle.Application;
using Swashbuckle.Swagger;

namespace MainSolutionTemplate.Api.AppStartup
{
  public class SwaggerSetup
  {
    private static bool _isInitialized;
    private static readonly object _locker = new object();
    private static SwaggerSetup _instance;

    protected SwaggerSetup(HttpConfiguration configuration)
    {
      Bootstrapper.Init(configuration);

      var version = typeof(UserController).Assembly.GetName().Version.ToString().Split('.').Take(3).StringJoin(".");

      SwaggerSpecConfig.Customize(c =>
      {
        c.ApiVersion(version);
        c.OperationFilter<AddStandardResponseCodes>();
        c.OperationFilter<AddAuthorizationResponseCodes>();
        c.IncludeXmlComments(String.Format(@"{0}\bin\MainSolutionTemplate.Api.XML",AppDomain.CurrentDomain.BaseDirectory));
        c.ApiInfo(new Info()
        {
          Title = "MainSolutionTemplate API v" + version,
          Description = "Restful api for MainSolutionTemplate"
        });
      });

      
    }

    public class AddStandardResponseCodes : IOperationFilter
    {
      public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription)
      {
        operation.ResponseMessages.Add(new ResponseMessage
        {
          Code = (int)HttpStatusCode.OK,
          Message = "Valid response code!"
        });

        operation.ResponseMessages.Add(new ResponseMessage
        {
          Code = (int)HttpStatusCode.InternalServerError,
          Message = "Internal error has occured!"
        });
      }
    }

    public class AddAuthorizationResponseCodes : IOperationFilter
    {
      public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription)
      {
        if (apiDescription.ActionDescriptor.GetFilters().OfType<AuthorizeAttribute>().Any())
        {
          operation.ResponseMessages.Add(new ResponseMessage
          {
            Code = (int)HttpStatusCode.Unauthorized,
            Message = "Authentication required"
          });
        }
      }
    }


    #region Initialize

    public static void Initialize(HttpConfiguration configuration)
    {
      if (_isInitialized) return;
      lock (_locker)
      {
        if (!_isInitialized)
        {
          _instance = new SwaggerSetup(configuration);
          _isInitialized = true;
        }
      }
    }

    #endregion
  }
}