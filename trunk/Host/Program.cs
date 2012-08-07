using System;
using System.Collections.Generic;
using System.Reflection;
using BiM.Behaviors.Data;
using BiM.Core.Messages;
using BiM.Host.Messages;
using BiM.Host.Plugins;
using BiM.MITM;
using NLog;

namespace BiM.Host
{
    public static class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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


            var d2oSource = new D2OSource();
            d2oSource.AddReaders(@"C:\Program Files (x86)\Dofus 2\app\data\common");
            DataProvider.Instance.AddSource(d2oSource);

            // todo : config
            MITM = new MITM.MITM(new MITMConfiguration
                                     {
                                         FakeAuthHost = "localhost",
                                         FakeAuthPort = 5555,
                                         FakeWorldHost = "localhost",
                                         FakeWorldPort = 5556,
                                         RealAuthHost = "213.248.126.180",
                                         RealAuthPort = 5555
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
            MITM.Stop();
            DispatcherTask.Stop();
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