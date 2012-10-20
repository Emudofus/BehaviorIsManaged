using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Core.Collections;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Authentification
{
    public class ServersList : INotifyPropertyChanged
    {
        private readonly ObservableCollectionMT<ServersListEntry> m_collection;
        private readonly ReadOnlyObservableCollectionMT<ServersListEntry> m_readOnlyCollection;

        public ReadOnlyObservableCollectionMT<ServersListEntry> Servers
        {
            get { return m_readOnlyCollection; }
        }

        public ServersListEntry this[ushort id]
        {
            get
            {
                return m_collection.FirstOrDefault(entry => entry.Id == id);
            }
        }

        public ServersList(ServersListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            m_collection = new ObservableCollectionMT<ServersListEntry>(msg.servers.Select(entry => new ServersListEntry(entry)));
            m_readOnlyCollection = new ReadOnlyObservableCollectionMT<ServersListEntry>(m_collection);
        }

        public void Update(ServerStatusUpdateMessage msg)
        {
            var server = this[msg.server.id];

            if (server == null)
                throw new Exception(string.Format("Cannot update the list, server id {0} not found", msg.server.id));

            server.Update(msg.server);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}