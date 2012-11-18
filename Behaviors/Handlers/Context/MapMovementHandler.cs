#region License GNU GPL

// MapMovementHandler.cs
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
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Context
{
    public class MapMovementHandler
    {
        [Configurable("EstimatedMovementLag", "Refer to the estimated time elapsed until the client start moving (in ms)")]
        public static int EstimatedMovementLag = 160;


        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [MessageHandler(typeof (GameMapMovementConfirmMessage))]
        public static void HandleGameMapMovementConfirmMessage(Bot bot, GameMapMovementConfirmMessage message)
        {
            if (bot.Character.IsMoving())
                bot.Character.NotifyStopMoving(false);
        }

        [MessageHandler(typeof (GameMapMovementCancelMessage))]
        public static void HandleGameMapMovementCancelMessage(Bot bot, GameMapMovementCancelMessage message)
        {
            // always check, the client can send bad things :)
            if (!bot.Character.IsMoving())
                return;

            TimedPathElement attemptElement = bot.Character.Movement.TimedPath.GetCurrentElement();

            if (attemptElement.CurrentCell.Id != message.cellId)
            {
                TimedPathElement clientCell = bot.Character.Movement.TimedPath.Elements.First(entry => entry.CurrentCell.Id == message.cellId);

                // the difference is the time elapsed until the client analyse the path and start moving (~160ms) it depends also on computer hardware
                logger.Warn("Warning the client has canceled the movement but the given cell ({0}) is not the attempted one ({1})." +
                            "Estimated difference : {2}ms", message.cellId, attemptElement.CurrentCell.Id, (attemptElement.EndTime - clientCell.EndTime).TotalMilliseconds);
            }

            bot.Character.NotifyStopMoving(true);
        }

        [MessageHandler(typeof (GameMapMovementMessage))]
        public static void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
        {
            if (bot.Character.Context == null)
            {
                logger.Error("Context is null as processing movement");
                return;
            }

            ContextActor actor = bot.Character.Context.GetContextActor(message.actorId);

            if (actor == null)
            {
                logger.Error("Actor {0} not found", message.actorId); // only a log for the moment until context are fully handled
                return;
            }

            // just to update the position
            if (message.keyMovements.Length == 1)
            {
                actor.UpdatePosition(message.keyMovements[0]);
            }
            else
            {
                Path path = Path.BuildFromServerCompressedPath(bot.Character.Map, message.keyMovements);

                if (path.IsEmpty())
                {
                    logger.Warn("Try to start moving with an empty path");
                    return;
                }

                var movement = new MovementBehavior(path, actor.GetAdaptedVelocity(path));
                movement.Start(DateTime.Now + TimeSpan.FromMilliseconds(EstimatedMovementLag));

                actor.NotifyStartMoving(movement);
            }
        }
    }
}