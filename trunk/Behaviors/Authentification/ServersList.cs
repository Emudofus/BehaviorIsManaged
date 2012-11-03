using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Core.Collections;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Authentification
{
    public class ServersList : ReadOnlyObservableCollectionMT<ServersListEntry>
    {
        public ServersListEntry this[ushort id]
        {
            get
            {
                return Items.FirstOrDefault(entry => entry.Id == id);
            }
        }

        public ServersList(ServersListMessage msg)
            : base (new ObservableCollection<ServersListEntry>(msg.servers.Select(entry => new ServersListEntry(entry))))
        {
        }

        public void Update(ServerStatusUpdateMessage msg)
        {
            var server = this[msg.server.id];

            if (server == null)
                throw new Exception(string.Format("Cannot update the list, server id {0} not found", msg.server.id));

            server.Update(msg.server);
        }
    }
}