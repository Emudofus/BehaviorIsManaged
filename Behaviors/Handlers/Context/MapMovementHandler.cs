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
using BiM.Behaviors.Game.Actors.Fighters;

namespace BiM.Behaviors.Handlers.Context
{
  public class MapMovementHandler
  {
    [Configurable("EstimatedMovementLag", "Refer to the estimated time elapsed until the client start moving (in ms)")]
    public static int EstimatedMovementLag = 160;


    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    [MessageHandler(typeof(GameMapMovementConfirmMessage))]
    public static void HandleGameMapMovementConfirmMessage(Bot bot, GameMapMovementConfirmMessage message)
    {
      if (bot.Character.IsMoving())
        bot.Character.NotifyStopMoving(false, false);
    }

    [MessageHandler(typeof(GameMapMovementCancelMessage))]
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

      bot.Character.NotifyStopMoving(true, false);
      bot.Character.SetPos(message.cellId);
    }

    [MessageHandler(typeof(GameMapNoMovementMessage))]
    public static void HandleGameMapNoMovementMessage(Bot bot, GameMapNoMovementMessage message)
    {
      // always check, the client can send bad things :)
      if (!bot.Character.IsMoving())
        return;

      bot.Character.NotifyStopMoving(true, true);
    }

    [MessageHandler(typeof(GameActionFightSlideMessage))]
    public static void HandleGameActionFightSlideMessage(Bot bot, GameActionFightSlideMessage message)
    {
        if (!bot.Character.IsFighting() || bot.Character.Fight==null)
        {
            logger.Error("GameActionFightSlideMessage has no sense out of a fight");
            return;
        }
        Fighter actor = bot.Character.Fight.GetActor(message.targetId);
        if (actor==null)
        {
            logger.Error("Actor {0} is not known.", message.targetId);
            return;
        }
        actor.SetPos(message.endCellId);
    }

    [MessageHandler(typeof(GameMapMovementMessage))]
    public static void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
    {
        if (bot.Character == null || bot.Character.Context == null)
      {
        logger.Error("Context is null as processing movement");
        return;
      }

      ContextActor actor = null;
      bool fightActor = false;
      if (bot.Character.IsFighting())
        actor = bot.Character.Fight.GetActor(message.actorId);
      if (actor == null)
        actor = bot.Character.Context.GetActor(message.actorId);
      else
        fightActor = true;
      if (actor == null)
      {
        logger.Error("Actor {0} not found (known : {1})", message.actorId, String.Join(",", fightActor ? bot.Character.Fight.Actors : bot.Character.Context.Actors)); // only a log for the moment until context are fully handled
        return;
      }

      // just to update the position. If in fight, better update immediately to be sure that the next action take the mouvement into account
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