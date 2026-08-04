using NLog;
using NLog.Config;
using NLog.Targets;

namespace Locators_for_Web_Elements.Core
{
    public static class LoggingConfigurator
    {
        public static void Configure(TafConfig config)
        {
            var nlogConfig = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} ${level:uppercase=true} ${logger} - ${message} ${exception:format=tostring}"
            };

            var fileTarget = new FileTarget("file")
            {
                FileName = $"${{basedir}}/{config.Logging.LogsDirectory}/test-log-${{shortdate}}.log",
                Layout = "${longdate} ${level:uppercase=true} ${logger} - ${message} ${exception:format=tostring}"
            };

            var minLevel = ParseLogLevel(config.Logging.MinLevel);

            nlogConfig.AddRule(minLevel, LogLevel.Fatal, consoleTarget);
            nlogConfig.AddRule(minLevel, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = nlogConfig;
        }

        private static LogLevel ParseLogLevel(string? minLevel) =>
            LogLevel.FromString(string.IsNullOrWhiteSpace(minLevel) ? "Info" : minLevel);
    }
}
