#region License GNU GPL
// PathfindersComparer.cs
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

using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    class PathfindersComparerRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatClientMultiMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".compare pathfinder"))
            {
                if (bot.HasFrame<PathfindersComparer>())
                    bot.RemoveFrame<PathfindersComparer>();
                else
                    bot.AddFrame(new PathfindersComparer(bot));
            }
        }
    }

    public class PathfindersComparer : Frame<PathfindersComparer>
    {
        public PathfindersComparer(Bot bot)
            : base(bot)
        {
        }

        [MessageHandler(typeof (GameMapMovementRequestMessage), FromFilter = ListenerEntry.Client)]
        public void HandleGameMapMovementRequestMessage(Bot bot, GameMapMovementRequestMessage message)
        {
            bot.SendToClient(new DebugClearHighlightCellsMessage());

            var clientPath = Path.BuildFromClientCompressedPath(bot.Character.Map, message.keyMovements);

            /*
            var pathfinder = new Pathfinder(bot.Character.Map, bot.Character.Map);
            var botPath = pathfinder.FindPath(bot.Character.Cell, clientPath.End, true);

            // if you see red cells it means the pathfinder is wrong and don't get the same path as the client
            bot.SendToClient(new DebugHighlightCellsMessage(Color.Red.ToArgb(), botPath.Cells.Select(entry => entry.Id).ToArray()));
            bot.SendToClient(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), clientPath.Cells.Select(entry => entry.Id).ToArray()));
            
            message.keyMovements = botPath.GetClientPathKeys();
             */

            var sw = Stopwatch.StartNew();
            PathFinder p1 = null;
            for (int i = 0; i < 100; i++)
            {
                p1 = new PathFinder(bot.Character.Map, false);
                p1.FindPath(bot.Character.Cell.Id, clientPath.End.Id);
            }
            sw.Stop();
            bot.Character.SendMessage(string.Format("FF : {0}ms {1}ticks", sw.ElapsedMilliseconds, sw.ElapsedTicks));

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {

                var p2 = new BiM.Behaviors.Game.World.Pathfinding.P.PathFinder(bot.Character.Map, false);
                p2.FindPath(new[] { bot.Character.Cell }, new[] { clientPath.End });
            }
            sw.Stop();
            bot.Character.SendMessage(string.Format("New : {0}ms {1}ticks", sw.ElapsedMilliseconds, sw.ElapsedTicks));

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var pathfinder = new Pathfinder(bot.Character.Map, bot.Character.Map);
                var botPath = pathfinder.FindPath(bot.Character.Cell, clientPath.End, true);
            }
            sw.Stop();

            bot.SendToClient(new DebugHighlightCellsMessage(Color.Red.ToArgb(), p1.PathResult.ToArray()));
            bot.SendToClient(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), clientPath.Cells.Select(entry => entry.Id).ToArray()));
            bot.Character.SendMessage(string.Format("Old : {0}ms {1}ticks", sw.ElapsedMilliseconds, sw.ElapsedTicks));
        }

        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            //bot.Character.StartMoving += OnStartMoving;
        }

        [MessageHandler(typeof (GameMapMovementCancelMessage))]
        public void HandleGameMapMovementCancelMessage(Bot bot, GameMapMovementCancelMessage message)
        {
            //bot.SendToClient(new DebugHighlightCellsMessage(Color.Violet.ToArgb(), new short[] { message.cellId }));
        }

        private void OnStartMoving(ContextActor actor, MovementBehavior movement)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            Task.Factory.StartNew(
                () => 
                {
                    var element = movement.TimedPath.GetCurrentElement();

                    bot.Character.HighlightCell(element.CurrentCell, Color.Green);

                    while(!movement.IsEnded())
                    {
                        var newElement = movement.TimedPath.GetCurrentElement();

                        if (element != newElement)
                        {
                            element = newElement;

                            bot.Character.ResetCellsHighlight();
                            bot.Character.HighlightCell(element.CurrentCell, Color.Green);
                        }

                        Thread.Sleep(30);
                    }
                });
        }
    }
}