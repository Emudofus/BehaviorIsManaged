using System;
using System.Collections.Generic;
using System.Reflection;
using BiM.Behaviors.Data;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Host.Messages;
using BiM.Host.Plugins;
using BiM.MITM;
using BiM.Protocol.Tools.D2p;
using NLog;

namespace BiM.Host
{
    public static class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string ConfigPath = "./config.xml";

        private static List<Assembly> m_hierarchy = new List<Assembly>()
        {
            Assembly.Load("BiM.Core"),
            Assembly.Load("BiM.Protocol"),
            Assembly.Load("BiM.Behaviors"),
            Assembly.Load("BiM.MITM"),
            Assembly.Load("BiM.Host"),
            // plugins come next
        };

        public static bool Running
        {
            get;
            private set;
        }

        public static MITM.MITM MITM
        {
            get;
            private set;
        }

        public static DispatcherTask DispatcherTask
        {
            get;
            private set;
        }

        public static Config Config
        {
            get;
            private set;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Initialization...");
            Initialize();

            Console.WriteLine("Starting...");
            Start();
            Console.WriteLine("Started");

            Console.Read();

            Stop();
        }

        private static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            Config = new Config(ConfigPath);
            Config.Load();

            foreach (var assembly in m_hierarchy)
            {
                Config.BindAssembly(assembly);
            }


            var d2oSource = new D2OSource();
            d2oSource.AddReaders(Config.GetStatic("DofusDataPath", @"C:\Program Files (x86)\Dofus 2\app\data\common"));
            DataProvider.Instance.AddSource(d2oSource);

            MITM = new MITM.MITM(new MITMConfiguration
                                     {
                                         FakeAuthHost = Config.GetStatic("BotAuthHost", "localhost"),
                                         FakeAuthPort = Config.GetStatic("BotAuthPort", 5555),
                                         FakeWorldHost = Config.GetStatic("BotWorldHost", "localhost"),
                                         FakeWorldPort = Config.GetStatic("BotWorldPort", 5556),
                                         RealAuthHost = Config.GetStatic("RealAuthHost", "213.248.126.180"),
                                         RealAuthPort = Config.GetStatic("RealAuthPort", 5555)
                                     });

            MessageDispatcher.DefineHierarchy(m_hierarchy);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                MessageDispatcher.RegisterAssembly(assembly);
            }

            PluginManager.Instance.LoadAllPlugins();

            DispatcherTask = new DispatcherTask(new MessageDispatcher(), MITM);
            DispatcherTask.Start(); // we have to start it now to dispatch the initialization msg

            var msg = new HostInitializationMessage();
            DispatcherTask.Dispatcher.Enqueue(msg, MITM);

            msg.Wait();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            Stop();
        }

        private static void Start()
        {
            if (Running)
                return;

            Running = true;
            MITM.Start();
        }

        private static void Stop()
        {
            if (!Running)
                return;
            Running = false;

            if (Config != null)
                Config.Save();

            if (MITM != null)
                MITM.Stop();

            if (DispatcherTask != null)
                DispatcherTask.Stop();

            PluginManager.Instance.UnLoadAllPlugins();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogUnhandledException((Exception) e.ExceptionObject);

            try
            {
                Stop();
            }
            finally
            {
                Console.WriteLine("Press enter to exit");
                Console.Read();

                Environment.Exit(-1);
            }
        }

        private static void LogUnhandledException(Exception ex)
        {
            logger.Fatal("Unhandled exception : {0}", ex);

            if (ex.InnerException != null)
                LogUnhandledException(ex.InnerException);
        }
    }
}