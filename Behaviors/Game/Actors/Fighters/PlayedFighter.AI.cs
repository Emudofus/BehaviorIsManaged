#region License GNU GPL
// Spell.cs
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

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Behaviors.Game.Spells;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public partial class PlayedFighter
    {
        public IEnumerable<Spells.Spell> GetOrderListOfSimpleAttackSpells(Fighter target, bool NoRangeCheck = false)
        {
            return Character.SpellsBook.GetOrderedAttackSpells(Character, target, Spells.Spell.SpellCategory.Damages).
                Where(spell => CanCastSpell(spell, target, NoRangeCheck) && !spell.LevelTemplate.needFreeCell);
        }

        public IEnumerable<Spells.Spell> GetOrderListOfSimpleBoostSpells()
        {
            return Character.SpellsBook.GetOrderListOfSimpleBoostSpells(Character, Spell.SpellCategory.Buff, true).
                Where(spell => CanCastSpell(spell, this, true));
        }

        internal void Update(Protocol.Messages.GameMapNoMovementMessage message)
        {
            if (IsMoving())
                NotifyStopMoving(true, true);
            NotifyAck(true);
        }

        #region Action acknowledgement
        public delegate void AckHandler(bool failed);

        public event AckHandler Acknowledge;

        internal void NotifyAck(bool failed)
        {
            if (Acknowledge != null) Acknowledge(failed);
        }

        internal void Update(Protocol.Messages.GameActionAcknowledgementMessage message)
        {
            if (IsMoving())
                NotifyStopMoving(false, false);
            NotifyAck(false);

        }

        #endregion Action acknowledgement

        internal void Update(Protocol.Messages.GameActionFightNoSpellCastMessage message)
        {
            NotifyAck(true);
        }
    }
}
