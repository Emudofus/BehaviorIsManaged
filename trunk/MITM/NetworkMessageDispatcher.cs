using System;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace BiM.MITM
{
    public class NetworkMessageDispatcher : MessageDispatcher<Bot>
    {
        public IClient Client
        {
            get;
            set;
        }

        public IClient Server
        {
            get;
            set;
        }

        protected override void Dispatch(Bot processor, Message message)
        {
            if (message is NetworkMessage)
                Dispatch(processor, message as NetworkMessage);
            else
                base.Dispatch(processor, message);
        }

        protected void Dispatch(Bot processor, NetworkMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (message.Destinations.HasFlag(ListenerEntry.Local))
            {
                base.Dispatch(processor, message);
            }

            if (Client != null && message.Destinations.HasFlag(ListenerEntry.Client))
            {
                Client.Send(message);
            }

            if (Server != null && message.Destinations.HasFlag(ListenerEntry.Server))
            {
                Server.Send(message);
            }
        }
    }
}