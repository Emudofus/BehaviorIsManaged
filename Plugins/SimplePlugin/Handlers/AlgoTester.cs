#region License GNU GPL
// AlgoTester.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using BiM.Behaviors;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Chat;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.MapTraveling;
using BiM.Behaviors.Game.World.MapTraveling.Transitions;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class AlgoTester
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".algo submap"))
            {
                bot.Character.ResetCellsHighlight();

                var sw = Stopwatch.StartNew();
                var submaps = DataProvider.Instance.Get<SerializableSubMap[]>(bot.Character.Map.Id);
                sw.Stop();
                bot.Character.SendMessage(string.Format("{0}ms", sw.ElapsedMilliseconds));

                foreach (var subMap in submaps)
                {
                    bot.Character.SendMessage(string.Format("[{0} mapid:{1} submap:{2}]", subMap.GlobalId, subMap.MapId, subMap.SubMapId));

                    foreach (var neighbour in subMap.Neighbours)
                    {
                        bot.Character.SendMessage(string.Format("[{0}] to [{1}] ({2})",  
                            subMap.GlobalId, neighbour.GlobalId, neighbour.Transition is MovementTransition ? (neighbour.Transition as MovementTransition).MapNeighbour : MapNeighbour.None));
                    }
                }

                message.BlockNetworkSend();
            }
            if (message.content.StartsWith(".algo los"))
            {
                var canBeSee = new List<Cell>();
                var cannotBeSee = new List<Cell>();

                Cell currentCell;
                if (bot.Character.IsFighting())
                    currentCell = bot.Character.Fighter.Cell;
                else
                    currentCell = bot.Character.Cell;

                foreach (var cell in bot.Character.Map.Cells.Where(x => x.Walkable))
                {
                    if (bot.Character.Context.CanBeSeen(currentCell, cell, !bot.Character.IsFighting()))
                    {
                        canBeSee.Add(cell);
                    }
                    else
                    {
                        cannotBeSee.Add(cell);
                    }
                }

                bot.Character.ResetCellsHighlight();

                bot.Character.HighlightCells(canBeSee, Color.Green);
                bot.Character.HighlightCells(cannotBeSee, Color.Red);


                message.BlockNetworkSend();
            }
        } 
    }
}