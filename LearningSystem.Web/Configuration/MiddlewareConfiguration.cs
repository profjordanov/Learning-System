using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LearningSystem.Web.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static void AddLogging(this ILoggerFactory loggerFactory, IConfigurationSection loggingConfiguration)
        {
            loggerFactory.AddConsole(loggingConfiguration);
            //loggerFactory.AddFile("logs/learning-system-{Date}.log");
            loggerFactory.AddDebug();
        }
    }
}
