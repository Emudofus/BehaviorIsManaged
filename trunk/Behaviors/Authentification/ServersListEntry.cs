using System;
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Core.Extensions;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Authentification
{
    public class ServersListEntry : INotifyPropertyChanged
    {
        public ServersListEntry(GameServerInformations server)
        {
            if (server == null) throw new ArgumentNullException("server");
            Update(server);
        }

        public ushort Id
        {
            get;
            set;
        }

        public Server Server
        {
            get;
            set;
        }

        private string m_name;

        public string Name
        {
            get { return m_name ?? (m_name = DataProvider.Instance.Get<string>(Server.nameId)); }
        }

        public ServerStatusEnum Status
        {
            get;
            set;
        }

        public sbyte Completion
        {
            get;
            set;
        }

        public bool IsSelectable
        {
            get;
            set;
        }

        public sbyte CharactersCount
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public void Update(GameServerInformations server)
        {
            if (server == null) throw new ArgumentNullException("server");
            Id = server.id;
            Status = (ServerStatusEnum) server.status;
            Completion = server.completion;
            IsSelectable = server.isSelectable;
            CharactersCount = server.charactersCount;
            Date = server.date.UnixTimestampToDateTime();
            Server = DataProvider.Instance.Get<Server>(server.id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}