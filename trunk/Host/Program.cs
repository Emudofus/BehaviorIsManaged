using System;
using System.Collections.Generic;
using BiM.Behaviors;
using BiM.Core.Logging;
using BiM.MITM;
using BiM.Protocol.Messages;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BiM.Host
{
    public class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => Console.WriteLine(eventArgs.ExceptionObject);
            
            // todo : properly initialization routine
            var target = new ColoredConsoleTarget()
            {
                Name = "coloredConsole",
                Layout = NLogHelper.LogFormatConsole
            };
            NLogHelper.AddTarget(target);
            NLogHelper.AddLogRule(new LoggingRule("*", NLog.LogLevel.Debug, target));
            BotManager.Instance.Initialize();
            NLogHelper.StartLogging();

            var mitm =
                new MITM.MITM(new MITMConfiguration
                                  {
                                    FakeAuthHost = "localhost",
                                    FakeAuthPort = 5555,
                                    FakeWorldHost = "localhost",
                                    FakeWorldPort = 5556,
                                    RealAuthHost = "213.248.126.180",
                                    RealAuthPort = 5555
                                  });

            mitm.Start();

            Console.Read();

            mitm.Stop();
        }
    }
}