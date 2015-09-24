using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Utilities.Helpers;
using Swashbuckle.Application;
using Swashbuckle.Swagger;

namespace MainSolutionTemplate.Api.Swagger
{
    public class SwaggerSetup
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
        private static SwaggerSetup _instance;

        protected SwaggerSetup(HttpConfiguration configuration)
        {

            

            string version =
                typeof (UserController).Assembly.GetName().Version.ToString().Split('.').Take(3).StringJoin(".");

            configuration.EnableSwagger(c => c.SingleApiVersion(version, "MainSolutionTemplate API"))
                .EnableSwaggerUi();

//            SwaggerSpecConfig.Customize(c =>
//                {
//                    c.ApiVersion(version);
//                    c.OperationFilter<AddStandardResponseCodes>();
//                    c.OperationFilter<AddAuthorizationResponseCodes>();
//                    
//                    c.IncludeXmlComments(String.Format(@"{0}\bin\MainSolutionTemplate.Api.XML",
//                                                       AppDomain.CurrentDomain.BaseDirectory));
//                    c.ApiInfo(new Info
//                        {
//                            Title = "MainSolutionTemplate API v" + version,
//                            Description =
//                                EmbededResourceHelper.ReadResource("MainSolutionTemplate.Api.Swagger.loginForm.html",
//                                                                   typeof (SwaggerSetup).Assembly)
//                        });
//                });
//            SwaggerUiConfig.Customize(c =>
//                {
//                   
//                    c.SupportHeaderParams = true;
//                    c.SupportedSubmitMethods = new[] {HttpMethod.Get, HttpMethod.Post, HttpMethod.Put};
//                    Assembly resourceAssembly = typeof (SwaggerSetup).Assembly;
//                    c.InjectJavaScript(resourceAssembly, "MainSolutionTemplate.Api.Swagger.onComplete.js");
//                });
        }

        #region Nested type: AddAuthorizationResponseCodes
//
//        public class AddAuthorizationResponseCodes : IOperationFilter
//        {
//            #region IOperationFilter Members
//
//            public void Apply(Operation operation, SchemaRegistry dataTypeRegistry, ApiDescription apiDescription)
//            {
//                if (apiDescription.ActionDescriptor.GetFilters().OfType<AuthorizeAttribute>().Any())
//                {
//                    operation.responses.Add(new ResponseMessage
//                        {
//                            Code = (int) HttpStatusCode.Unauthorized,
//                            Message = "Authentication required"
//                        });
//                }
//            }
//
//            #endregion
//
//            
//        }

        #endregion

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
//
        #region Nested type: AddStandardResponseCodes
//
//        public class AddStandardResponseCodes : IOperationFilter
//        {
//            #region IOperationFilter Members
//
//            public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription)
//            {
//                operation.ResponseMessages.Add(new ResponseMessage
//                    {
//                        Code = (int) HttpStatusCode.OK,
//                        Message = "Valid response code!"
//                    });
//
//                operation.ResponseMessages.Add(new ResponseMessage
//                    {
//                        Code = (int) HttpStatusCode.InternalServerError,
//                        Message = "Internal error has occured!"
//                    });
//            }
//
//            #endregion
//        }

        #endregion
    }
}