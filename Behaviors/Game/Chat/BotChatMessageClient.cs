﻿#region License GNU GPL
// BotChatMessageClient.cs
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
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Chat
{
    public class BotChatMessageClient : BotChatMessage
    {
        public BotChatMessageClient()
        {
            
        }

        public BotChatMessageClient(ChatClientPrivateMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            ReceiverName = message.receiver;
            Channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE;
        }

        public BotChatMessageClient(ChatClientMultiMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum) message.channel;
        }

        public string ReceiverName
        {
            get;
            set;
        }
    }
}