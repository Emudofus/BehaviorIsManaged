using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Authentification
{
    public class ServersList : INotifyPropertyChanged
    {
        private readonly ObservableCollection<ServersListEntry> m_collection;
        private readonly ReadOnlyObservableCollection<ServersListEntry> m_readOnlyCollection;

        public ReadOnlyObservableCollection<ServersListEntry> Servers
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

            m_collection = new ObservableCollection<ServersListEntry>(msg.servers.Select(entry => new ServersListEntry(entry)));
            m_readOnlyCollection = new ReadOnlyObservableCollection<ServersListEntry>(m_collection);
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