#region License GNU GPL
// RolePlayHandler.cs
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
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.World;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using BiM.Behaviors.Game.Interactives;

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
            bot.Character.Map.Update(bot, message);
        }

        [MessageHandler(typeof (GameRolePlayShowActorMessage))]
        public void HandleGameRolePlayShowActorMessage(Bot bot, GameRolePlayShowActorMessage message)
        {
            bot.Character.Map.AddActor(bot, message.informations);
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
        public void HandleInteractiveUseEndedMessage(Bot bot, InteractiveUseEndedMessage message)
        {
            var interactive = bot.Character.Map.GetInteractive(message.elemId);

            if (interactive != null)
                interactive.NotifyInteractiveUseEnded();
        }

        [MessageHandler(typeof (StatedElementUpdatedMessage))]
        public void HandleStatedElementUpdatedMessage(Bot bot, StatedElementUpdatedMessage message)
        {
            InteractiveObject interactive = bot.Character.Map.GetInteractive(message.statedElement.elementId);
            if (interactive == null) return;
            string previousState = interactive.ToString();
            bot.Character.Map.Update(message);
            bot.Character.SendInformation("StatedElementUpdatedMessage : {0} => {1}", previousState, interactive);            
        }

        [MessageHandler(typeof (StatedMapUpdateMessage))]
        public void HandleStatedMapUpdateMessage(Bot bot, StatedMapUpdateMessage message)
        {
            bot.Character.Map.Update(message);
        }
    }
}