using System.Drawing;
using BiM.Behaviors;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace FightPlugin
{
    public static class FightDetector
    {
        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.Character.FightJoined += OnFightJoined;
            bot.Character.FightLeft += OnFightLeft;
        }

        private static void OnFightJoined(PlayedCharacter character, Fight fight)
        {
            character.Fight.TurnStarted += OnTurnStarted;
        }

        private static void OnFightLeft(PlayedCharacter character, Fight fight)
        {
            character.Fight.TurnStarted -= OnTurnStarted;
        }

        private static void OnTurnStarted(Fight fight, Fighter fighter)
        {
            var bot = BotManager.Instance.GetCurrentBot();

            bot.Character.SendMessage(string.Format("This is '{0}' turn.", fighter.Name), Color.Violet);
        }
    }
}