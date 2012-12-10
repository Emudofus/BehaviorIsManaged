#region License GNU GPL
// ServersListEntry.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
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
            get { return m_name ?? (m_name = I18NDataManager.Instance.ReadText(Server.nameId)); }
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
            Server = ObjectDataManager.Instance.Get<Server>(server.id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}