using System.Diagnostics;
using System.Linq;
using WindowManager.Api32;

namespace WindowManager
{
    public class ClientFinder
    {
        public static Process GetProcessUsingPort(int localPort)
        {
            var connections = IPHlpApi32Wrapper.GetAllTcpConnections();
            var entry = connections.FirstOrDefault(x => x.LocalPort == localPort);
            var process = Process.GetProcessById(entry.owningPid);

            return process;
        }
    }
}