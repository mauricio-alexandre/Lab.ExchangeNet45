using Owin;

namespace Lab.ExchangeNet45.WebApi.Utils.OwinMiddlewares
{
    public static class NLogStopwatchLoggingAppBuilderExtensions
    {
        public static IAppBuilder UseNLogStopwatchLogging(this IAppBuilder app)
        {
            return app.Use(typeof(NLogStopwatchLoggingMiddleware));
        }
    }
}