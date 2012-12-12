﻿#region License GNU GPL
// EmoteShortcut.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class EmoteShortcut : GeneralShortcut
    {
        public EmoteShortcut(PlayedCharacter character, ShortcutEmote shortcut)
            : base(character, shortcut.slot)
        {
            Emote = ObjectDataManager.Instance.Get<Emoticon>(shortcut.emoteId);
        }

        public Emoticon Emote
        {
            get;
            private set;
        }

        public override void Update(Protocol.Types.Shortcut shortcut)
        {
            base.Update(shortcut);

            if (shortcut is ShortcutEmote)
                Emote = ObjectDataManager.Instance.Get<Emoticon>((shortcut as ShortcutEmote).emoteId);
        }
    }
}