#region License GNU GPL
// GeneralShortcutBar.cs
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
using System;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class GeneralShortcutBar : ShortcutBar<GeneralShortcut>
    {
        public GeneralShortcutBar(PlayedCharacter character)
            : base(character)
        {
        }


        public override ShortcutBarEnum BarType
        {
            get { return ShortcutBarEnum.GENERAL_SHORTCUT_BAR; }
        }

        public void Add(ShortcutObjectItem item)
        {
            Add(new ItemShortcut(Character, item));
        }

        public void Add(ShortcutObjectPreset shortcut)
        {
            Add(new ItemPresetShortcut(Character, shortcut));
        }

        public void Add(ShortcutEmote shortcut)
        {
            Add(new EmoteShortcut(Character, shortcut));
        }

        public void Add(ShortcutSmiley shortcut)
        {
            Add(new SmileyShortcut(Character, shortcut));
        }

        public void Add(Protocol.Types.Shortcut shortcut)
        {
            if (shortcut is ShortcutObjectItem)
                Add(shortcut as ShortcutObjectItem);
            else if (shortcut is ShortcutObjectPreset)
                Add(shortcut as ShortcutObjectPreset);
            else if (shortcut is ShortcutEmote)
                Add(shortcut as ShortcutEmote);
            else if (shortcut is ShortcutSmiley)
                Add(shortcut as ShortcutSmiley);
        }

        public override void Update(ShortcutBarContentMessage content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Clear();
            foreach (Protocol.Types.Shortcut shortcut in content.shortcuts)
            {
                Add(shortcut);
            }
        }

        public override void Update(ShortcutBarRefreshMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if ((ShortcutBarEnum) message.barType != BarType)
                return;

            GeneralShortcut shortcut = Get(message.shortcut.slot);
            if (shortcut != null)
                shortcut.Update(message.shortcut);
            else
                Add(message.shortcut);
        }
    }
}