using NLog;
using NLog.Config;
using NLog.Targets;

namespace BiM.Core.Logging
{
    public class NLogHelper
    {
        public static string LogFormatConsole = "[${blockcenter:length=18:inner=${callsite:className=true:methodName=false:includeSourcePath=false:nonamespace=true}}](${threadid}) ${message}";
        public static string LogFormatFile = "[${level}] <${date:format=G}> ${message}";

        public static string LogFormatErrorFile = "-------------${level} at ${date:format=G}------------- ${newline}" +
                                                  "${exception} ${newline}" +
                                                  "${callsite} -> ${newline}\t${message} ${newline}" +
                                                  "-------------${level} at ${date:format=G}------------- ${newline}";
        private static LoggingConfiguration m_configuration = new LoggingConfiguration();

        public static void AddTarget(Target target)
        {
            m_configuration.AddTarget(target.Name, target);
        }

        public static void AddLogRule(LoggingRule rule)
        {
            m_configuration.LoggingRules.Add(rule);
        }

        public static void EnableFileLogging(string directory, bool splitErrors)
        {
            if (splitErrors)
            {
                var fileTarget = new FileTarget
                                     {
                                         Name = "fileTarget",
                                         FileName = "${basedir}" + directory + "log_${date:format=dd-MM-yyyy}" + ".txt",
                                         Layout = LogFormatFile
                                     };

                var fileErrorTarget = new FileTarget
                                          {
                                              Name = "fileErrorTarget",
                                              FileName = "${basedir}" + directory +
                                                         "error_${date:format=dd-MM-yyyy}" + ".txt",
                                              Layout = LogFormatErrorFile,
                                          };

                AddTarget(fileTarget);
                AddTarget(fileErrorTarget);

                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                rule.DisableLoggingForLevel(LogLevel.Fatal);
                rule.DisableLoggingForLevel(LogLevel.Error);
                AddLogRule(rule);

                var errorRule = new LoggingRule("*", LogLevel.Warn, fileErrorTarget);
                AddLogRule(errorRule);
            }
            else
            {
                var fileTarget = new FileTarget
                        {
                            Name = "fileTarget",
                            FileName = "${basedir}" + directory + "log_${date:format=dd-MM-yyyy}" + ".txt",
                            Layout = LogFormatFile
                        };

                AddTarget(fileTarget);
            }
        }

        public static void StartLogging()
        {
            LogManager.Configuration = m_configuration;
            LogManager.EnableLogging();
        }

        public void StopLogging()
        {
            LogManager.DisableLogging();
        }
    }
}