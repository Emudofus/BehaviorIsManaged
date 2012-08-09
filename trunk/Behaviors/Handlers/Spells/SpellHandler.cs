using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Spells
{
    public class SpellHandler
    {
        [MessageHandler(typeof(SpellListMessage))]
        public static void HandleSpellListMessage(Bot bot, SpellListMessage message)
        {
            bot.Character.Update(message);
        } 
    }
}