using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Lab.ExchangeNet45.WebApi.Utils.ExceptionHandlings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;

namespace Lab.ExchangeNet45.WebApi
{
    public class HttpConfigurationFactory
    {
        public HttpConfiguration Create()
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            config.MapHttpAttributeRoutes();

            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());

            config
                .EnableSwagger(docs => { docs.SingleApiVersion("v1", "Lab.ExchangeNet45.WebApi"); })
                .EnableSwaggerUi(ui => { ui.DocExpansion(DocExpansion.List); });

            config.Routes.MapHttpRoute(
                name: "Swagger UI",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));

            return config;
        }
    }
}