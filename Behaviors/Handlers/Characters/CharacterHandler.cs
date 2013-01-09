#region License GNU GPL
// CharacterHandler.cs
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
            if (bot.Character != null)
                bot.Character.Update(message);
        }

        [MessageHandler(typeof(SetCharacterRestrictionsMessage))]
        public static void HandleSetCharacterRestrictionsMessage(Bot bot, SetCharacterRestrictionsMessage message)
        {
            if (bot.Character != null)
                bot.Character.Update(message);
        }

        [MessageHandler(typeof(GameMapNoMovementMessage))]
        public static void HandleGameMapNoMovementMessage(Bot bot, GameMapNoMovementMessage message)
        {
            if (bot.Character != null)
                if (bot.Character.IsFighting())
                    bot.Character.Fighter.Update(message);
                else
                    bot.Character.Update(message);
        }
    }
}