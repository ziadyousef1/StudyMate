using EcommerceApp.Application.Services.Interfaces.Logging;
using Serilog;

namespace EcommerceApp.Infrastructure.Services
{
    internal class SerilogLogger<T>(ILogger logger) : IAppLogger<T>
    {
        public void LogError(Exception ex, string message)
                    => logger.Error(ex, message);

        public void LogInformation(string message)
                     => logger.Information(message);

        public void LogWarning(string message)
                    => logger.Warning(message);
    }
}
