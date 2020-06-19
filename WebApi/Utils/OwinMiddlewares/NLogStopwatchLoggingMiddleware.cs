using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Lab.ExchangeNet45.WebApi.Utils.OwinMiddlewares
{
    public class NLogStopwatchLoggingMiddleware : OwinMiddleware
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public NLogStopwatchLoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await Next.Invoke(context);
            stopwatch.Stop();
            TimeSpan time = stopwatch.Elapsed;
            Logger.Info($"Request time elapsed: {(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds}");
        }
    }
}