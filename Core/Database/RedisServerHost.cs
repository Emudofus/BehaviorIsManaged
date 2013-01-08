#region License GNU GPL
// RedisServerHost.cs
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using BiM.Core.Network;
using BiM.Core.Reflection;
using NLog;
using ServiceStack.Redis;

namespace BiM.Core.Database
{
    public class RedisServerHost : Singleton<RedisServerHost>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public RedisServerHost()
        {
            
        }

        public RedisServerHost(string exePath)
        {
            ExecutablePath = exePath;
        }

        public string ExecutablePath
        {
            get;
            set;
        }

        public Process ServerProcess
        {
            get;
            private set;
        }

        public void StartOrFindProcess()
        {
            if (!CanReachServer())
            {
                if (string.IsNullOrEmpty(ExecutablePath) || !File.Exists(ExecutablePath))
                    throw new Exception(string.Format("Redis server not started and executable {0} not found", ExecutablePath));

                ServerProcess = new Process();
                ServerProcess.StartInfo = new ProcessStartInfo(ExecutablePath)
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,

                };
                ServerProcess.EnableRaisingEvents = true;

                ServerProcess.Start();
                ServerProcess.BeginOutputReadLine();

                logger.Info("Starting {0}...", Path.GetFileName(ExecutablePath));

                // wait enough time
                if (!CanReachServer(5000))
                    throw new Exception(string.Format("Redis Server hasn't been launch correctly (Timeout:{0})", 5000));

                logger.Info("{0} started...", Path.GetFileName(ExecutablePath));
            }
            else
            {
                var processes = Process.GetProcessesByName("redis-server");

                if (processes.Length == 1)
                    ServerProcess = processes[0];
                else if (processes.Length <= 0)
                    throw new Exception("Process redis-server not found");
                else
                {
                    var client = new RedisClient("localhost");
                    var connections = IPHlpApi32Wrapper.GetAllTcpConnections();
                    var matching = connections.SingleOrDefault(x => x.LocalPort == ((IPEndPoint)client.Socket.LocalEndPoint).Port);

                    if (matching.Equals(default(MIB_TCPROW_OWNER_PID)))
                        throw new Exception("Process redis-server not found");

                    ServerProcess = Process.GetProcessById(matching.owningPid);
                }


                logger.Info("Redis process found (pid:{0})", ServerProcess.Id);
            }
        }

        public void Shutdown()
        {
            var client = new RedisClient("localhost");

            client.Shutdown();
        }

        public bool CanReachServer(int timeout = 1000)
        {
            var client = new RedisClient("localhost");
            client.ConnectTimeout = timeout;
            try
            {
                logger.Info("Pinging Redis server ...");
                client.Ping();
            }
            catch (Exception)
            {
                logger.Info("Ping failed");
                return false;
            }

            logger.Info("Ping successed");
            return true;
        }
    }
}