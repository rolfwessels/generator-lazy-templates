using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Shared.Models;
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
            string version = GetVersion();

            configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion(version, "MainSolutionTemplate API")
                    .Description(EmbededResourceHelper.ReadResource("MainSolutionTemplate.Api.Swagger.loginForm.html",
                        typeof (SwaggerSetup).Assembly));
                c.IncludeXmlComments(String.Format(@"{0}\bin\MainSolutionTemplate.Api.XML",
                    AppDomain.CurrentDomain.BaseDirectory));
                c.OperationFilter<AddAuthorizationResponseCodes>();
            })
                .EnableSwaggerUi(c =>
                {
                    Assembly resourceAssembly = typeof (SwaggerSetup).Assembly;
                    c.InjectJavaScript(resourceAssembly, "MainSolutionTemplate.Api.Swagger.onComplete.js");
                });

        }

        #region Private Methods

        private static string GetVersion()
        {
            return typeof (UserController).Assembly.GetName().Version.ToString().Split('.').Take(3).StringJoin(".");
        }

        #endregion

        #region Instance

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


        public class AddAuthorizationResponseCodes : IOperationFilter
        {
            #region IOperationFilter Members

            public void Apply(Operation operation, SchemaRegistry dataTypeRegistry, ApiDescription apiDescription)
            {
                if (apiDescription.ActionDescriptor.GetFilters().OfType<AuthorizeAttribute>().Any())
                {
                    operation.responses.Add(ToIntDisplayValue(HttpStatusCode.Unauthorized), new Response
                        {
                            description = "Authentication required"    
                        });

                }
            }

            private static string ToIntDisplayValue(HttpStatusCode httpStatusCode)
            {
                return ((int)httpStatusCode).ToString(CultureInfo.InvariantCulture);
            }

            #endregion   
        }
    }
}