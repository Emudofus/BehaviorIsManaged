#region License GNU GPL
// ChatHandler.cs
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
using BiM.Behaviors.Game.Chat;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Chat
{
    public class ChatHandler
    {
        [MessageHandler(typeof (ChatClientMultiMessage))]
        public static void HandleChatClientMultiMessage(Bot bot, ChatClientMultiMessage message)
        {
            bot.SendLocal(new BotChatMessageClient(message));
        }

        [MessageHandler(typeof(ChatClientPrivateMessage))]
        public static void HandleChatClientPrivateMessage(Bot bot, ChatClientPrivateMessage message)
        {
            bot.SendLocal(new BotChatMessageClient(message));
        }

        [MessageHandler(typeof(ChatServerMessage))]
        public static void HandleChatServerMessage(Bot bot, ChatServerMessage message)
        {
            bot.SendLocal(new BotChatMessageServer(message));
        }

        [MessageHandler(typeof(ChatServerCopyMessage))]
        public static void HandleChatServerCopyMessage(Bot bot, ChatServerCopyMessage message)
        {
            bot.SendLocal(new BotChatMessageServer(message));
        }

        [MessageHandler(typeof(ChatAdminServerMessage))]
        public static void HandleChatAdminServerMessage(Bot bot, ChatAdminServerMessage message)
        {
            bot.SendLocal(new BotChatMessageServer(message));
        }
    }
}