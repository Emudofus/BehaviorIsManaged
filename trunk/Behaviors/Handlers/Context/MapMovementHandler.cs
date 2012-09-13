using System;
using System.Drawing;
using System.Linq;
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

            var attemptElement = bot.Character.Movement.TimedPath.GetCurrentElement();

            if (attemptElement.CurrentCell.Id != message.cellId)
            {
                var clientCell = bot.Character.Movement.TimedPath.Elements.First(entry => entry.CurrentCell.Id == message.cellId);

                // the difference is the time elapsed until the client analyse the path and start moving (~160ms) it depends also on computer hardware
                logger.Warn("Warning the client has canceled the movement but the given cell ({0}) is not the attempted one ({1})." +
                    "Estimated difference : {2}ms", message.cellId, attemptElement.CurrentCell.Id, (attemptElement.EndTime - clientCell.EndTime).TotalMilliseconds);
            }

            bot.Character.NotifyStopMoving(true);
        }

        [MessageHandler(typeof(GameMapMovementMessage))]
        public static void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
        {
            var actor = bot.Character.Map.GetActor(message.actorId);

            if (actor == null)
                logger.Error("Actor {0} not found", message.actorId); // only a log for the moment until context are fully handled
            else
            {
                var path = Path.BuildFromServerCompressedPath(bot.Character.Map, message.keyMovements);

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