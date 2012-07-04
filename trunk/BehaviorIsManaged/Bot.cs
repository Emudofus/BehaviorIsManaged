using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace BiM
{
    public class Bot
    {
        public Bot()
        {
            Dispatcher = new MessageDispatcher<Bot>();
            ConnectionType = ClientConnectionType.Disconnected;
            Running = true;
        }

        public Bot(MessageDispatcher<Bot> messageDispatcher)
        {
            Dispatcher = messageDispatcher;
            ConnectionType = ClientConnectionType.Disconnected;
            Running = true;
        }

        public MessageDispatcher<Bot> Dispatcher 
        { 
            get; 
            private set; 
        }

        public bool Running
        {
            get;
            private set;
        }

        public ClientConnectionType ConnectionType
        {
            get;
            set;
        }

        public string ConnectionTicket
        {
            get;
            set;
            }

        public void Tick()
        {
            Dispatcher.ProcessDispatching(this);
        }

        public void Send(Message message)
        {
            Dispatcher.Enqueue(message);
        }

        public void Send(NetworkMessage message, ListenerEntry dest)
        {
            message.Destinations = dest;
            message.From = ListenerEntry.Local;

            Dispatcher.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direct">If true it doesn't reprocess the message internally</param>
        public void SendToClient(NetworkMessage message, bool direct = false)
        {
            if (direct)
                Send(message, ListenerEntry.Client);
            else
                Send(message, ListenerEntry.Client | ListenerEntry.Local);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direct">If true it doesn't reprocess the message internally</param>
        public void SendToServer(NetworkMessage message, bool direct = false)
        {
            if (direct)
                Send(message, ListenerEntry.Server);
            else
                Send(message, ListenerEntry.Server | ListenerEntry.Local);
        }

        public void SendLocal(NetworkMessage message)
        {
            Send(message, ListenerEntry.Local);
        }

        public virtual void Stop()
        {
            if (!Running)
                return;

            Running = false;

            if (Dispatcher != null)
                Dispatcher.Stop();
        }

        public virtual void Resume()
        {
            if (Running)
                return;

            Running = true;

            if (Dispatcher != null)
                Dispatcher.Resume();
        }

        public virtual void Dispose()
        {
            Stop();

            if (Dispatcher != null)
                Dispatcher.Dispose();
        }
    }
}