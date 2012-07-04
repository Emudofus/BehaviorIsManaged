using System;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace BiM.MITM
{
    public class NetworkMessageDispatcher : MessageDispatcher<Bot>
    {
        public NetworkMessageDispatcher()
        {
        }

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
            if (processor == null) throw new ArgumentNullException("processor");
            if (message == null) throw new ArgumentNullException("message");
            if (message is NetworkMessage)
                Dispatch(processor, message as NetworkMessage);
            else
                base.Dispatch(processor, message);
        }

        protected void Dispatch(Bot processor, NetworkMessage message)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            if (message == null) throw new ArgumentNullException("message");

            if (message.Destinations.HasFlag(ListenerEntry.Local))
            {
                InternalDispatch(processor, message);
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

        private void InternalDispatch(Bot processor, NetworkMessage message)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            if (message == null) throw new ArgumentNullException("message");
            var handlers = GetHandlers(message.MessageId);

            try
            {
                foreach (var handler in handlers)
                {
                    if (handler.Attribute.DestinationFilter != ListenerEntry.Undefined &&
                        handler.Attribute.DestinationFilter != message.Destinations && 
                        (handler.Attribute.DestinationFilter & message.Destinations) == ListenerEntry.Undefined)
                        continue;

                    if (handler.Attribute.FromFilter != ListenerEntry.Undefined && 
                        handler.Attribute.FromFilter != message.From &&
                        (handler.Attribute.FromFilter & message.From) == ListenerEntry.Undefined)
                        continue;

                    handler.Action(handler.Container, processor, message);

                    if (message.Canceled)
                        break;
                }
            }
            catch (Exception)
            {
                // todo : log
                throw;
            }
        }
    }
}