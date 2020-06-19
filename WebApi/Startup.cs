using System.Reflection;
using System.Web.Http;
using Lab.ExchangeNet45.Application.Bootstrapping;
using Lab.ExchangeNet45.Contracts.Operacao.Commands;
using Lab.ExchangeNet45.WebApi.Utils.MediatorSimpleInjector;
using Lab.ExchangeNet45.WebApi.Utils.OwinMiddlewares;
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

            app.UseNLog().UseNLogStopwatchLogging();

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

            container.RegisterMediatR(new[]
            {
                typeof(Startup).GetTypeInfo().Assembly, // current assembly
                typeof(AddOperacaoCommand).GetTypeInfo().Assembly, // contracts assembly
                typeof(ExchangeApplicationSimpleInjectorExtensions).GetTypeInfo().Assembly // application assembly
            });

            container.RegisterExchangeApplication();

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