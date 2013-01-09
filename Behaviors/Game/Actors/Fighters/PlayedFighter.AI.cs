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
using BiM.Behaviors.Game.Spells;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;
using BiM.Core.Config;
using BiM.Protocol.Data;
using BiM.Behaviors.Data.D2O;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public partial class PlayedFighter
    {
        public int SummonedCount
        {
            get
            {
                return Fight.Actors.Count(fighter => fighter.IsAlive && fighter.Summoned && fighter.Summoner == this);
            }

        }
        public bool CanSummon()
        {
            return (Stats as PlayerStats).SummonLimit > SummonedCount;
        }

        [Configurable("DefaultRecordOnTheFly", "If true, the sniffer will record on the fly by default at start.")]
        public static bool DefaultRecordOnTheFly = true;

        /*public IEnumerable<Spells.Spell> GetZoneAttackSpells()
        {
            return Character.SpellsBook.GetZoneSpells(this, Spells.Spell.SpellCategory.Damages);
        }

        public IEnumerable<Spells.Spell> GetOrderListOfSimpleAttackSpells(Fighter target, bool NoRangeCheck = false)
        {
            return Character.SpellsBook.GetOrderedAttackSpells(this, target, Spells.Spell.SpellCategory.Damages).
                Where(spell => CanCastSpell(spell, target, NoRangeCheck) && !spell.LevelTemplate.needFreeCell);
        }

        public IEnumerable<Spells.Spell> GetOrderListOfSimpleBoostSpells()
        {
            return Character.SpellsBook.GetOrderListOfSimpleBoostSpells(this, this, Spell.SpellCategory.Buff).
                Where(spell => CanCastSpell(spell, this, true));
        }*/

        public IEnumerable<Spells.Spell> GetOrderListOfInvocationSpells()
        {
            IEnumerable<Spells.Spell> spells = Character.SpellsBook.Spells.Where(spell => (Stats.CurrentAP >= spell.LevelTemplate.apCost) && spell.IsAvailable(null, BiM.Behaviors.Game.Spells.Spell.SpellCategory.Invocation) && CanCastSpell(spell, (Cell)null, true) && spell.LevelTemplate.needFreeCell).OrderByDescending(spell => spell.Level).ThenByDescending(spell => spell.LevelTemplate.minPlayerLevel);
            //Character.SendDebug("Sorted invocs : {0}", String.Join(",", spells));
            return spells;
        }

        internal void Update(Protocol.Messages.GameMapNoMovementMessage message)
        {
            if (IsMoving())
                NotifyStopMoving(true, true);
            NotifyAck(true);
        }

        internal void Update(Protocol.Messages.GameActionFightNoSpellCastMessage message)
        {
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
            NotifyAck(!message.valid);

        }

        #endregion Action acknowledgement

        /// <summary>
        /// Gives walkable cells where the character may walk within the turn.
        /// Note : it's now supposed to be reliable, using PathFinder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cell> GetPossibleMoves(bool cautious, bool ForceIncludeStartingCell = true, PathFinder pathFinder = null)
        {
            if (pathFinder == null)
                pathFinder = new PathFinder(Fight, true);
            List<Cell> cells = pathFinder.FindConnectedCells(
                Cell, true, cautious,
                cell => cell.DistanceSteps <= Stats.CurrentMP, null, Stats.CurrentMP).ToList();
            if (ForceIncludeStartingCell && !cells.Contains(Cell))
                cells.Add(Cell);
            return cells;
        }

        public BiM.Behaviors.Game.Spells.Spell FindMostEfficientAttackSpell()
        {
            BiM.Behaviors.Game.Spells.Spell bestSpell = null;
            double betterDamage = 0;

            List<BiM.Behaviors.Game.Spells.Spell> spells = Character.SpellsBook.Spells.ToList();            
            spells.Add(Character.SpellsBook.WeaponSpellLike());

            foreach (BiM.Behaviors.Game.Spells.Spell spell in spells)
                if (spell.IsAvailable(null, BiM.Behaviors.Game.Spells.Spell.SpellCategory.Damages))
                {
                    double dmg = spell.GetTotalDamageOnAllEnemies(this) / spell.LevelTemplate.apCost;
                    if (dmg > betterDamage)
                    {
                        betterDamage = dmg;
                        bestSpell = spell;
                    }
                }
            if (bestSpell == null)
                return Character.SpellsBook.Spells.Where(spell => ((spell.Categories & BiM.Behaviors.Game.Spells.Spell.SpellCategory.Damages) != 0) && spell.IsAvailable(null, null)).OrderByDescending(spell => spell.Level).ThenByDescending(spell => spell.LevelTemplate.minPlayerLevel).FirstOrDefault();
            return bestSpell;
        }

        public bool CanFight { get; private set; }
        /// <summary>
        /// Says if no state prevents from casting spells
        /// Warning: a bit time consuming
        /// </summary>
        /// <returns></returns>
        private bool canFight()
        {
            return !GetStates().Any(state => state.preventsFight);
        }

        public bool CanCastSpells { get; private set; }
        /// <summary>
        /// Says if no state prevents from casting spells
        /// Warning: a bit time consuming
        /// </summary>
        /// <returns></returns>
        private bool canCastSpells()
        {
            return !GetStates().Any(state => state.preventsSpellCast);
        }

        /// <summary>
        /// Says if no state prevents from casting spells
        /// Warning: a bit time consuming
        /// </summary>
        /// <returns></returns>
        public bool CanCastSpell(int spellId)
        {
            return !GetSpellImmunityEffects().Any(effect => effect.immuneSpellId == spellId);
        }

        /// <summary>
        /// Retrieves current states in effect on the fighter
        /// Warning: a bit time consuming
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SpellState> GetStates()
        {
            return GetBoostStateEffects().Select(effect => ObjectDataManager.Instance.Get<Protocol.Data.SpellState>(effect.stateId));
        }

        public IEnumerable<Cell> EnemiesCells
        {
            get
            {
                return GetOpposedTeam().FightersAlive.Select(fighter=>fighter.Cell);
            }
        }

        /// <summary>
        /// All cells where friends stand, not including his own cell
        /// </summary>
        public IEnumerable<Cell> FriendCells
        {
            get
            {
                return Team.FightersAlive.Where(fighter=>fighter.Id != Id).Select(fighter => fighter.Cell);
            }
        }
        
        public IEnumerable<Cell> AvailablePlacementCells
        {
            get
            {
                return Team.PlacementCells.Except(FriendCells);
            }
        }

    }
}
