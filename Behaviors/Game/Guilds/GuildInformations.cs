#region License GNU GPL
// GuildInformations.cs
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

namespace BiM.Behaviors.Game.Guilds
{
    public class GuildInformations : INotifyPropertyChanged
    {
        public GuildInformations()
        {
            
        }

        public GuildInformations(Protocol.Types.GuildInformations guild)
        {
            if (guild == null) throw new ArgumentNullException("guild");

            Id = guild.guildId;
            Name = guild.guildName;
            Emblem = new GuildEmblem(guild.guildEmblem);
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public GuildEmblem Emblem
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}