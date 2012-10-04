using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using BiM.Behaviors.Data;
using BiM.Core.Config;
using BiM.Core.I18n;
using BiM.Core.Messages;
using BiM.Host.Messages;
using BiM.Host.Plugins;
using BiM.MITM;
using BiM.Protocol.Tools.D2p;
using NLog;

namespace BiM.Host
{
    public static class Host
    {
        [Configurable("DofusDataPath")]
        public static string DofusDataPath = @"C:\Program Files (x86)\Dofus 2\app\data\common";

        [Configurable("DofusMapsD2P")]
        public static string DofusMapsD2P = @"C:\Program Files (x86)\Dofus 2\app\content\maps\maps0.d2p";

        [Configurable("DofusI18NPath")]
        public static string DofusI18NPath = @"C:\Program Files (x86)\Dofus 2\app\data\i18n";

        [Configurable("BotAuthHost")]
        public static string BotAuthHost = "localhost";

        [Configurable("BotAuthPort")]
        public static int BotAuthPort = 5555;

        [Configurable("BotWorldHost")]
        public static string BotWorldHost = "localhost";

        [Configurable("BotWorldPort")]
        public static int BotWorldPort = 5556;

        [Configurable("RealAuthHost")]
        public static string RealAuthHost = "213.248.126.180";

        [Configurable("RealAuthPort")]
        public static int RealAuthPort = 5555;

        public static event UnhandledExceptionEventHandler UnhandledException;

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

        public static bool Initialized
        {
            get;
            private set;
        }

        public static void Initialize()
        {
            if (Initialized)
                return;

            if (!Debugger.IsAttached) // the debugger handle the unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            Config = new Config(ConfigPath);

            foreach (var assembly in m_hierarchy)
            {
                Config.BindAssembly(assembly);
                Config.RegisterAttributes(assembly);
            }

            Config.Load();

            logger.Info("{0} loaded", Path.GetFileName(Config.FilePath));


            var d2oSource = new D2OSource();
            d2oSource.AddReaders(DofusDataPath);
            DataProvider.Instance.AddSource(d2oSource);

            var maps = new D2PSource(new D2pFile(DofusMapsD2P));
            DataProvider.Instance.AddSource(maps);

            // todo : langs
            var d2iSource = new D2ISource(Languages.English);
            d2iSource.AddReaders(DofusI18NPath);
            DataProvider.Instance.AddSource(d2iSource);

            MITM = new MITM.MITM(new MITMConfiguration
                                     {
                                         FakeAuthHost = BotAuthHost,
                                         FakeAuthPort = BotAuthPort,
                                         FakeWorldHost = BotWorldHost,
                                         FakeWorldPort = BotWorldPort,
                                         RealAuthHost = RealAuthHost,
                                         RealAuthPort = RealAuthPort
                                     });

            MessageDispatcher.DefineHierarchy(m_hierarchy);

            foreach (var assembly in m_hierarchy)
            {
                MessageDispatcher.RegisterSharedAssembly(assembly);
            }

            PluginManager.Instance.LoadAllPlugins();

            DispatcherTask = new DispatcherTask(new MessageDispatcher(), MITM);
            DispatcherTask.Start(); // we have to start it now to dispatch the initialization msg

            var msg = new HostInitializationMessage();
            DispatcherTask.Dispatcher.Enqueue(msg, MITM);

            msg.Wait();

            Initialized = true;
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            Stop();
        }

        public static void Start()
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
                if (UnhandledException != null)
                {
                    UnhandledException(sender, e);
                }
                else
                {
                    Console.WriteLine("Press enter to exit");
                    Console.Read();
                }

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