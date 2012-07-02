using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace BiM
{
    public class Bot
    {
        public bool Running
        {
            get;
            set;
        }

        public Bot()
        {
            Dispatcher = new MessageDispatcher<Bot>();
            Running = true;
        }

        public Bot(MessageDispatcher<Bot> messageDispatcher)
        {
            Dispatcher = messageDispatcher;
            Running = true;
        }

        public MessageDispatcher<Bot> Dispatcher { get; set; }

        public void Send(Message message)
        {
            Dispatcher.AddMessageToDispatch(message);
        }

        public void Send(NetworkMessage message, ListenerEntry dest)
        {
            message.Destinations = dest;
            message.From = ListenerEntry.Local;

            Dispatcher.AddMessageToDispatch(message);
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