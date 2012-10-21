using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Context
{
    public class RolePlayHandler : Frame<RolePlayHandler>
    {
        public RolePlayHandler(Bot bot)
            : base(bot)
        {
        }

        [MessageHandler(typeof(CurrentMapMessage))]
        public static void HandleCurrentMapMessage(Bot bot, CurrentMapMessage message)
        {
            bot.Character.EnterMap(new Map(message.mapId, message.mapKey));
        }

        [MessageHandler(typeof(MapComplementaryInformationsDataMessage))]
        public void HandleMapComplementaryInformationsDataMessage(Bot bot, MapComplementaryInformationsDataMessage message)
        {
            bot.Character.Map.Update(message);
        }

        [MessageHandler(typeof (GameRolePlayShowActorMessage))]
        public void HandleGameRolePlayShowActorMessage(Bot bot, GameRolePlayShowActorMessage message)
        {
            bot.Character.Map.AddActor(message.informations);
        }

        [MessageHandler(typeof (InteractiveElementUpdatedMessage))]
        public void HandleInteractiveElementUpdatedMessage(Bot bot, InteractiveElementUpdatedMessage message)
        {
            bot.Character.Map.Update(message);
        }

        [MessageHandler(typeof (InteractiveMapUpdateMessage))]
        public void HandleInteractiveMapUpdateMessage(Bot bot, InteractiveMapUpdateMessage message)
        {
            bot.Character.Map.Update(message);
        }

        [MessageHandler(typeof (InteractiveUsedMessage))]
        public void HandleInteractiveUsedMessage(Bot bot, InteractiveUsedMessage message)
        {
            var interactive = bot.Character.Map.GetInteractive(message.elemId);

            if (interactive != null)
                interactive.NotifyInteractiveUsed(message);
        }

        [MessageHandler(typeof (InteractiveUseEndedMessage))]
        public static void HandleInteractiveUseEndedMessage(Bot bot, InteractiveUseEndedMessage message)
        {
            var interactive = bot.Character.Map.GetInteractive(message.elemId);

            if (interactive != null)
                interactive.NotifyInteractiveUseEnded();
        }

        [MessageHandler(typeof (StatedElementUpdatedMessage))]
        public void HandleStatedElementUpdatedMessage(Bot bot, StatedElementUpdatedMessage message)
        {
            bot.Character.Map.Update(message);
        }

        [MessageHandler(typeof (StatedMapUpdateMessage))]
        public void HandleStatedMapUpdateMessage(Bot bot, StatedMapUpdateMessage message)
        {
            bot.Character.Map.Update(message);
        }
    }
}