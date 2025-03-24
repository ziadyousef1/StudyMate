using Serilog;
using StudyMate.Services.Interfaces;

namespace StudyMate.Services.Implementaions
{
    internal class SerilogLogger<T>(Serilog.ILogger logger) :IAppLogger<T>
    {
        public void LogError(Exception ex, string message)
                    => logger.Error(ex, message);

        public void LogInformation(string message)
                     => logger.Information(message);

        public void LogWarning(string message)
                    => logger.Warning(message);
    }
}
