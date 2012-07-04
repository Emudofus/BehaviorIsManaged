using System;
using System.Collections.Generic;
using BiM.MITM;
using BiM.Protocol.Messages;

namespace BiM.Host
{
    public class Program
    {
        private static Dictionary<string, Tuple<Bot, SelectedServerDataMessage>> m_tickets = new Dictionary<string, Tuple<Bot, SelectedServerDataMessage>>();
        private static string ticket;
        private static MessageReceiver messageReceiver;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => Console.WriteLine(eventArgs.ExceptionObject);

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