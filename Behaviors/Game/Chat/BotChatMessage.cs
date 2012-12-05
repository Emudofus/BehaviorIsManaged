#region License GNU GPL
// BotChatMessage.cs
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
using System.ComponentModel;
using BiM.Core.Messages;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Chat
{
    public abstract class BotChatMessage : Message, INotifyPropertyChanged
    {
        // note : I have to encapsulate this protocol part because ChatAbstractServerMessage and ChatAbstractClientMessage
        // are not bound, and this is not good

        public string Content
        {
            get;
            set;
        }

        public ChatActivableChannelsEnum Channel
        {
            get;
            set;
        }

        // todo
        public object Items
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}