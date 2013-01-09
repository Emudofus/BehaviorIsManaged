#region License GNU GPL
// SpellHandler.cs
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
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Fights;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Handlers.Spells
{
    public class SpellHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [MessageHandler(typeof(SpellListMessage))]
        public static void HandleSpellListMessage(Bot bot, SpellListMessage message)
        {
            bot.Character.Update(message);
        }

        [MessageHandler(typeof(GameActionFightSpellCastMessage))]
        public static void HandleGameActionFightSpellCastMessage(Bot bot, GameActionFightSpellCastMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            var fighter = bot.Character.Fight.GetFighter(message.sourceId);

            if (fighter == null)
                logger.Error("Fighter {0} cast a spell but doesn't exist", message.sourceId);
            else
            {
                fighter.NotifySpellCasted(new SpellCast(bot.Character.Fight, message));
                if (bot.Character.Fighter != null && bot.Character.Fighter.Id == message.sourceId)
                    bot.Character.SpellsBook.CastAt(message);

            }
        }

        [MessageHandler(typeof(GameActionFightSpellCooldownVariationMessage))]
        public static void HandleGameActionFightSpellCooldownVariationMessage(Bot bot, GameActionFightSpellCooldownVariationMessage message)
        {

        }

        [MessageHandler(typeof(GameActionFightNoSpellCastMessage))]
        public static void HandleGameActionFightNoSpellCastMessage(Bot bot, GameActionFightNoSpellCastMessage message)
        {
            if (bot == null || bot.Character == null || bot.Character.Fight == null)
            {
                logger.Error("Fight is not properly initialized.");
                return; // Can't handle the message
            }
            PlayedFighter fighter = bot.Character.Fighter;

            if (fighter != null)
            {
                fighter.Update(message);
            }
        }

        [MessageHandler(typeof(SpellUpgradeSuccessMessage))]
        public static void HandleSpellUpgradeSuccessMessage(Bot bot, SpellUpgradeSuccessMessage message)
        {
            bot.Character.Update(message);
        }
    }
}