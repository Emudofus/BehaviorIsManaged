#region License GNU GPL
// ChatMessageServer.cs
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
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Interfaces;
using BiM.Behaviors.Game.World;
using BiM.Core.Extensions;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Chat
{
    public class ChatMessageServer : ChatMessage
    {
        public ChatMessageServer()
        {
            
        }

        public ChatMessageServer(ChatServerMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum)message.channel;
            SentTime = message.timestamp.UnixTimestampToDateTime();
            FingerPrint = message.fingerprint;
            SenderId = message.senderId;
            SenderName = message.senderName;
            SenderAccountId = message.senderAccountId;
        }

        public ChatMessageServer(ChatServerCopyMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum)message.channel;
            SentTime = message.timestamp.UnixTimestampToDateTime();
            ReceiverId = message.receiverId;
            ReceiverName = message.receiverName;
            FingerPrint = message.fingerprint;
            SenderId = ReceiverId;
            SenderName = ReceiverName;
            Copy = true;
        }

        public ChatMessageServer(ChatAdminServerMessage message)
            : this((ChatServerMessage)message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Admin = true;
        }

        public ContextActor TryGetSender(IMapContext context)
        {
            return context.Actors.FirstOrDefault(entry => ( entry is INamed && ( entry as INamed ).Name == SenderName ) || entry.Id == SenderId);
        }

        public DateTime SentTime
        {
            get;
            set;
        }

        public string FingerPrint
        {
            get;
            set;
        }

        public int SenderId
        {
            get;
            set;
        }

        public string SenderName
        {
            get;
            set;
        }

        public int SenderAccountId
        {
            get;
            set;
        }

        public bool Copy
        {
            get;
            set;
        }

        public int ReceiverId
        {
            get;
            set;
        }

        public string ReceiverName
        {
            get;
            set;
        }

        public bool Admin
        {
            get;
            set;
        }
    }
}