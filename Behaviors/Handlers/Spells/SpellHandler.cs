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
        [MessageHandler(typeof(GameFightStartMessage))]
        public static void HandleGameFightStartMessage(Bot bot, GameFightStartMessage message)
        {
            bot.Character.SpellsBook.FightStart(message);
        }
        [MessageHandler(typeof(GameFightTurnEndMessage))]
        public void HandleGameFightTurnEndMessage(Bot bot, GameFightTurnEndMessage message)
        {
            bot.Character.SpellsBook.EndTurn();

        }
        [MessageHandler(typeof(GameActionFightSpellCastMessage))]
        public void HandleGameActionFightSpellCastMessage(Bot bot, GameActionFightSpellCastMessage message)
        {
            if (bot.Character.Id == message.sourceId)
                bot.Character.SpellsBook.CastAt(message);                
        }
    }
}