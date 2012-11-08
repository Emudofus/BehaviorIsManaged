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
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class PathfindersComparer
    {
        [Configurable("AllowComparer")]
        public static bool AllowComparer = false;

        [MessageHandler(typeof (GameMapMovementRequestMessage))]
        public static void HandleGameMapMovementRequestMessage(Bot bot, GameMapMovementRequestMessage message)
        {
            if (!AllowComparer)
                return;

            bot.SendToClient(new DebugClearHighlightCellsMessage());

            var clientPath = Path.BuildFromClientCompressedPath(bot.Character.Map, message.keyMovements);


            var pathfinder = new Pathfinder(bot.Character.Map, bot.Character.Map);
            var botPath = pathfinder.FindPath(bot.Character.Cell, clientPath.End, true);

            // if you see red cells it means the pathfinder is wrong and don't get the same path as the client
            //bot.SendToClient(new DebugHighlightCellsMessage(Color.Red.ToArgb(), botPath.Cells.Select(entry => entry.Id).ToArray()));
            //bot.SendToClient(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), clientPath.Cells.Select(entry => entry.Id).ToArray()));
        }

        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.Character.StartMoving += OnStartMoving;
        }

        [MessageHandler(typeof (GameMapMovementCancelMessage))]
        public static void HandleGameMapMovementCancelMessage(Bot bot, GameMapMovementCancelMessage message)
        {
            if (!AllowComparer)
                return;

            bot.SendToClient(new DebugHighlightCellsMessage(Color.Violet.ToArgb(), new short[] { message.cellId }));
        }

        private static void OnStartMoving(ContextActor actor, MovementBehavior movement)
        {
            if (!AllowComparer)
                return;

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