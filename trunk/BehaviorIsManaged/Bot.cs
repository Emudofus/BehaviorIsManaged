using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace BiM
{
    public class Bot
    {
        public MessageDispatcher<Bot> Dispatcher { get; set; }

        public void Send(Message message)
        {
            Dispatcher.Dispatch(message);
        }

        public void Send(NetworkMessage message, ListenerEntry dest)
        {
            message.Destinations = dest;

            Dispatcher.Dispatch(message);
        }

        public void SendToClient(NetworkMessage message)
        {
            Send(message, ListenerEntry.Client);
        }

        public void SendToServer(NetworkMessage message)
        {
            Send(message, ListenerEntry.Server);
        }

        public void SendLocal(NetworkMessage message)
        {
            Send(message, ListenerEntry.Local);
        }
    }
}