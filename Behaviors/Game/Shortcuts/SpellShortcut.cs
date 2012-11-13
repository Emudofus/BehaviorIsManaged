#region License GNU GPL
// SpellShortcut.cs
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
using BiM.Behaviors.Game.Spells;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SpellShortcut : Shortcut
    {
        public SpellShortcut(PlayedCharacter character, ShortcutSpell shortcut)
            : base(character, shortcut.slot)
        {
            Character = character;
            SpellId = shortcut.spellId;
        }

        public int SpellId
        {
            get;
            private set;
        }

        public Spell GetSpell()
        {
            return Character.SpellsBook.GetSpell(SpellId);
        }

        public void Update(Protocol.Types.Shortcut shortcut)
        {
            Slot = shortcut.slot;
            if (shortcut is ShortcutSpell)
            {
                SpellId = (shortcut as ShortcutSpell).spellId;
            }
        }
    }
}