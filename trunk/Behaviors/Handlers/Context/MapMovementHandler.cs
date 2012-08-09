using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Context
{
    public class MapMovementHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [MessageHandler(typeof (GameMapMovementConfirmMessage))]
        public static void HandleGameMapMovementConfirmMessage(Bot bot, GameMapMovementConfirmMessage message)
        {
            bot.Character.NotifyStopMoving();
        }

        [MessageHandler(typeof(GameMapMovementMessage))]
        public static void HandleGameMapMovementMessage(Bot bot, GameMapMovementMessage message)
        {
            var actor = bot.Character.Map.GetActor(message.actorId);

            if (actor == null)
                logger.Error("Actor {0} not found", message.actorId); // only a log for the moment until context are fully handled
            else
                actor.NotifyStartMoving(Path.BuildFromServerCompressedPath(bot.Character.Map, message.keyMovements));
        }
    }
}