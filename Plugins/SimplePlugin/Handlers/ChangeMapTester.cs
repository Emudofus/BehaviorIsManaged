#region License GNU GPL
// ChangeMapTester.cs
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
using BiM.Behaviors;
using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class ChangeMapTester
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".move"))
            {
                message.BlockNetworkSend();// do not send this message to the server

                var splits = message.content.Split(' ');
                if (splits.Length != 2)
                {
                    bot.Character.SendMessage("syntax : .move [Right/Left/Top/Bottom]");
                    return;
                }

                MapNeighbour neighbour;
                try
                {
                    neighbour = (MapNeighbour)Enum.Parse(typeof(MapNeighbour), splits[1]);
                }
                catch (Exception)
                {
                    bot.Character.SendMessage("syntax : .move [Right/Left/Top/Bottom]");
                    return;
                }


                if (!bot.Character.ChangeMap(neighbour))
                {
                    bot.Character.SendMessage(string.Format("Cannot move to {0} !", neighbour));
                }
            }
        }
    }
}