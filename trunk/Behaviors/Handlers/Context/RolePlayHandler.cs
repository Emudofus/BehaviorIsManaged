using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Context
{
    public static class RolePlayHandler
    {
        [MessageHandler(typeof(CurrentMapMessage))]
        public static void HandleCurrentMapMessage(Bot bot, CurrentMapMessage message)
        {
            bot.Character.Map = new Map(message.mapId, message.mapKey);
        }

        [MessageHandler(typeof(MapComplementaryInformationsDataMessage))]
        public static void HandleMapComplementaryInformationsDataMessage(Bot bot, MapComplementaryInformationsDataMessage message)
        {
            bot.Character.Map.Update(message);
        } 
    }
}