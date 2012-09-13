using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Movements;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin
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
            var botPath = pathfinder.FindPath(bot.Character.Position.Cell, clientPath.End, true);

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

        private static void OnStartMoving(RolePlayActor actor, MovementBehavior movement)
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