#region License GNU GPL
// ContextHandler.cs
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
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Context
{
    public class ContextHandler
    {
        [MessageHandler(typeof(GameContextCreateMessage))]
        public static void HandleGameContextCreateMessage(Bot bot, GameContextCreateMessage message)
        {
            if (bot.Display != DisplayState.InGame)
                bot.Display = DisplayState.InGame;

            bot.Character.ChangeContext((GameContextEnum)message.context);
        }

        [MessageHandler(typeof(GameContextDestroyMessage))]
        public static void HandleGameContextDestroyMessage(Bot bot, GameContextDestroyMessage message)
        {
            bot.Character.LeaveContext();
        }

        [MessageHandler(typeof (GameContextRemoveElementMessage))]
        public static void HandleGameContextRemoveElementMessage(Bot bot, GameContextRemoveElementMessage message)
        {
            // can be on the map or in fight
            bot.Character.Context.RemoveActor(message.id);
        }
    }
}