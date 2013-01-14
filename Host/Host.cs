#region License GNU GPL

// Host.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using BiM.Behaviors;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Data.Icons;
using BiM.Behaviors.Data.Maps;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Database;
using BiM.Core.I18n;
using BiM.Core.Machine;
using BiM.Core.Messages;
using BiM.Core.UI;
using BiM.Host.Plugins;
using BiM.Host.UI;
using BiM.MITM;
using NLog;

namespace BiM.Host
{
    public static class Host
    {
        private const string ConfigPath = "./config.xml";
        private static string m_dofusPath;

        [Configurable("FindDofusPathAuto", "When true the dofus path will be find automatically")]
        public static bool FindDofusPathAuto = true;

        [Configurable("DofusBasePath")]
        public static string DofusBasePath = @"C:\Program Files (x86)\Dofus 2\";

        [Configurable("DofusDataPath")]
        public static string DofusDataPath = @"app\data\common";

        [Configurable("DofusMapsD2P")]
        public static string DofusMapsD2P = @"app\content\maps\maps0.d2p";

        [Configurable("DofusI18NPath")]
        public static string DofusI18NPath = @"app\data\i18n";

        [Configurable("DofusItemIconPath")]
        public static string DofusItemIconPath = @"app\content\gfx\items\bitmap0.d2p";

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

        [Configurable("RedisServerExe")]
        public static string RedisServerExe = "./Redis/redis-server.exe";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly List<Assembly> m_hierarchy = new List<Assembly>
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

        public static event UnhandledExceptionEventHandler UnhandledException;

        public static void Initialize()
        {
            if (Initialized)
                return;

            UIManager.Instance.SetBusy(true);

            if (!Debugger.IsAttached) // the debugger handle the unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;


            UIManager.Instance.BusyMessage = "Load config ...";
            Config = new Config(ConfigPath);

            foreach (Assembly assembly in m_hierarchy)
            {
                Config.BindAssembly(assembly);
                Config.RegisterAttributes(assembly);
            }

            Config.Load();

            logger.Info("{0} loaded", Path.GetFileName(Config.FilePath));


            UIManager.Instance.BusyMessage = "Loading D2O files ...";
            ObjectDataManager.Instance.AddReaders(Path.Combine(GetDofusPath(), DofusDataPath));
            I18NDataManager.Instance.DefaultLanguage = Languages.English;

            I18NDataManager.Instance.AddReaders(Path.Combine(GetDofusPath(), DofusI18NPath));
            IconsManager.Instance.Initialize(Path.Combine(GetDofusPath(), DofusItemIconPath));


            UIManager.Instance.BusyMessage = "Starting redis server ...";
            logger.Info("Starting redis server ...");
            RedisServerHost.Instance.ExecutablePath = RedisServerExe;
            RedisServerHost.Instance.StartOrFindProcess();

            UIManager.Instance.BusyMessage = string.Format("Loading {0}...", MapsManager.MapsDataFile);
            logger.Info("Loading {0}...", MapsManager.MapsDataFile);
            ProgressionCounter progression = MapsManager.Instance.Initialize(Path.Combine(GetDofusPath(), DofusMapsD2P));
            if (progression != null)
                ExecuteProgress(progression);

            UIManager.Instance.BusyMessage = "Loading maps positions ...";
            logger.Info("Loading maps positions ...");
            progression = MapsPositionManager.Instance.Initialize();
            if (progression != null)
                ExecuteProgress(progression);

            UIManager.Instance.BusyMessage = "Loading submaps ...";
            logger.Info("Loading submaps ...");
            progression = SubMapsManager.Instance.Initialize();
            if (progression != null)
                ExecuteProgress(progression);


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

            foreach (Assembly assembly in m_hierarchy)
            {
                MessageDispatcher.RegisterSharedAssembly(assembly);
            }

            UIManager.Instance.BusyMessage = "Loading plugins ...";
            PluginManager.Instance.LoadAllPlugins();

            DispatcherTask = new DispatcherTask(new MessageDispatcher(), MITM);
            DispatcherTask.Start(); // we have to start it now to dispatch the initialization msg

            BotManager.Instance.Initialize();

            var msg = new HostInitializationMessage();
            DispatcherTask.Dispatcher.Enqueue(msg, MITM);

            msg.Wait();

            UIManager.Instance.SetBusy(false);
            Initialized = true;
        }

        private static void ExecuteProgress(ProgressionCounter progression, int refreshRate = 500)
        {
            double lastValue = progression.Value;
            while (!progression.IsEnded)
            {
                Thread.Sleep(refreshRate);
                if (Math.Abs(lastValue - progression.Value) > 0.1)
                {
                    logger.Debug("{0} : {1:0.0}/{2} Mem:{3}MB", progression.Text, progression.Value, progression.Total, GC.GetTotalMemory(false)/(1024*1024));
                    UIManager.Instance.SetBusyProgress(progression.Value, progression.Total);
                }

                lastValue = progression.Value;
            }

            GC.Collect();
        }

        public static void ChangeLanguage(Languages language)
        {
            I18NDataManager.Instance.DefaultLanguage = language;

            // todo : update the gui
        }

        public static string GetDofusPath()
        {
            if (m_dofusPath == null)
            {
                if (!FindDofusPathAuto)
                    return m_dofusPath = DofusBasePath;

                string programFiles = OSInfo.GetProgramFiles();

                if (Directory.Exists(Path.Combine(programFiles, "Dofus2")))
                    m_dofusPath = Path.Combine(programFiles, "Dofus2");
                else if (Directory.Exists(Path.Combine(programFiles, "Dofus 2")))
                    m_dofusPath = Path.Combine(programFiles, "Dofus 2");
                else
                {
                    programFiles = OSInfo.GetProgramFilesX86();

                    if (Directory.Exists(Path.Combine(programFiles, "Dofus2")))
                        m_dofusPath = Path.Combine(programFiles, "Dofus2");
                    else if (Directory.Exists(Path.Combine(programFiles, "Dofus 2")))
                        m_dofusPath = Path.Combine(programFiles, "Dofus 2");
                }
            }

            return m_dofusPath;
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

            BotManager.Instance.RemoveAll();

            if (Config != null)
                Config.Save();

            if (MITM != null)
                MITM.Stop();

            if (DispatcherTask != null)
                DispatcherTask.Stop();

            PluginManager.Instance.UnLoadAllPlugins();
            RedisServerHost.Instance.Shutdown();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            Stop();
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