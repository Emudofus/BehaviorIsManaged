using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Characters
{
    public class CharacterHandler
    {
        [MessageHandler(typeof (CharactersListMessage))]
        public static void HandleCharactersListMessage(Bot bot, CharactersListMessage message)
        {
            bot.ClientInformations.Update(message);
            bot.Display = DisplayState.CharacterSelection;
        }

        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.SetPlayedCharacter(new PlayedCharacter(bot, message.infos));
        }

        [MessageHandler(typeof(CharacterStatsListMessage))]
        public static void HandleCharacterStatsListMessage(Bot bot, CharacterStatsListMessage message)
        {
            bot.Character.Update(message);
        }
    }
}