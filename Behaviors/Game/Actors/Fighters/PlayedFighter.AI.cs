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
using BiM.Behaviors.Game.World;
using BiM.Core.Config;
using BiM.Behaviors.Game.Stats;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public partial class PlayedFighter
    {
        public int SummonedCount { get; set; }
        public bool CanSummon()
        {
            return (Stats as PlayerStats).SummonLimit > SummonedCount;
        }
        private List<int> SummonedActors = new List<int>();

        [Configurable("DefaultRecordOnTheFly", "If true, the sniffer will record on the fly by default at start.")]
        public static bool DefaultRecordOnTheFly = true;


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

        public IEnumerable<Spells.Spell> GetOrderListOfInvocationSpells()
        {
            //Character.SendDebug("Sorted invocs : {0}", String.Join(",", Character.SpellsBook.GetOrderListOfSimpleBoostSpells(Character, Spell.SpellCategory.Invocation, false)));
            return Character.SpellsBook.GetOrderListOfSimpleBoostSpells(Character, Spell.SpellCategory.Invocation, false).
                Where(spell => CanCastSpell(spell, (Cell)null, true) && spell.LevelTemplate.needFreeCell);
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


        internal void Update(Protocol.Messages.GameFightSynchronizeMessage msg)
        {
            SummonedCount = 0;
            SummonedActors.Clear();
            foreach (var info in msg.fighters)
                if (info.stats.summoned && info.stats.summoner == this.Id && info.alive)
                    if (!SummonedActors.Contains(info.contextualId))
                    {
                        SummonedCount++;
                        SummonedActors.Add(info.contextualId);
                    }
        }

        internal void Update(Protocol.Messages.GameActionFightSummonMessage message)
        {
            if (message.summon.stats.summoned && message.summon.stats.summoner == Id)
                if (!SummonedActors.Contains(message.summon.contextualId))
                {
                    SummonedCount++;
                    SummonedActors.Add(message.summon.contextualId);
                }            
        }

        internal void SummonUpdate(Protocol.Messages.GameActionFightDeathMessage message)
        {
            if (SummonedActors.Contains(message.targetId))
            {
                SummonedActors.Remove(message.targetId);
                SummonedCount--;
            }
        }
    }
}
