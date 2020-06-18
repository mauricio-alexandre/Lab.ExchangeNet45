using System.Web.Http;
using Lab.ExchangeNet45.WebApi.Utils;
using Microsoft.Owin.Cors;
using NLog.Owin.Logging;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Lab.ExchangeNet45.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfigurationFactory().Create();

            app.UseNLog();

            app.UseSimpleInjector(config);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }
    }

    internal static class AppBuilderSimpleInjectorExtensions
    {
        /// <summary>
        /// https://simpleinjector.readthedocs.io/en/latest/owinintegration.html
        /// </summary>
        public static void UseSimpleInjector(this IAppBuilder app, HttpConfiguration config)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // register application here

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            app.Use(async (context, next) =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    await next();
                }
            });
        }
    }
}