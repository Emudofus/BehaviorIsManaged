using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Regen
{
    public class RegenHandler
    {
        [MessageHandler(typeof(LifePointsRegenBeginMessage))]
        public static void HandleLifePointsRegenBeginMessage(Bot bot, LifePointsRegenBeginMessage message)
        {
            bot.Character.RegenRate = message.regenRate;
        } 
    }
}