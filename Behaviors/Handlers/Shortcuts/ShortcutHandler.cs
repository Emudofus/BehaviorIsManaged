#region License GNU GPL
// ShortcutHandler.cs
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

namespace BiM.Behaviors.Handlers.Shortcuts
{
    public class ShortcutHandler
    {
        [MessageHandler(typeof(ShortcutBarContentMessage))]
        public static void HandleShortcutBarContentMessage(Bot bot, ShortcutBarContentMessage message)
        {
            bot.Character.Update(message);
        }

        [MessageHandler(typeof (ShortcutBarRefreshMessage))]
        public static void HandleShortcutBarRefreshMessage(Bot bot, ShortcutBarRefreshMessage message)
        {
            if (message.barType == (int)bot.Character.GeneralShortcuts.BarType)
                bot.Character.GeneralShortcuts.Update(message);
            else
                bot.Character.SpellShortcuts.Update(message);
        }

        [MessageHandler(typeof(ShortcutBarRemovedMessage))]
        public static void HandleShortcutBarRemovedMessage(Bot bot, ShortcutBarRemovedMessage message)
        {
            if (message.barType == (int) bot.Character.GeneralShortcuts.BarType)
                bot.Character.GeneralShortcuts.Remove(message.slot);
            else
                bot.Character.SpellShortcuts.Remove(message.slot);

        }
    }
}