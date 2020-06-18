using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Lab.ExchangeNet45.WebApi.Utils.ExceptionHandlings
{
    public class NLogExceptionLogger : IExceptionLogger
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Logger.Error(context.Exception);

            return Task.FromResult(0);
        }
    }
}