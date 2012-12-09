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
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;

namespace SimplePlugin.Handlers
{
    class PathfindersComparerRegister
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatClientMultiMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".compare pathfinder"))
            {
                message.BlockNetworkSend();
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


            var pathfinder = new Pathfinder(bot.Character.Map, bot.Character.Map);
            var FFpathfinder = new PathFinder(bot.Character.Map, false);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            Path botPath1= null, botPath2;
            stopwatch.Start();
            for(int i=0; i<100; i++)
                botPath1 = pathfinder.FindPath(bot.Character.Cell, clientPath.End, true);
            stopwatch.Stop();
            bot.Character.SendWarning("Dofus-like PathFinder x 100 = {0}", stopwatch.Elapsed.ToString("ss\\.fff"));
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
                FFpathfinder.FindPath(bot.Character.Cell.Id, clientPath.End.Id);
            stopwatch.Stop();
            bot.Character.SendWarning("FF PathFinder x 100 = {0}", stopwatch.Elapsed.ToString("ss\\.fff"));
            botPath2 = new Path(bot.Character.Map, FFpathfinder.GetLastPathUnpacked(0).Select(id => bot.Character.Map.Cells[id]));
            // if you see red cells it means the pathfinder is wrong and don't get the same path as the client
            bot.Character.HighlightCells(botPath2.Cells, Color.Green);
            bot.Character.HighlightCells(botPath1.Cells, Color.Red);
            bot.Character.HighlightCells(clientPath.Cells, Color.Blue);
            
            message.keyMovements = botPath1.GetClientPathKeys();
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