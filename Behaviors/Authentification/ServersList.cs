#region License GNU GPL
// ServersList.cs
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