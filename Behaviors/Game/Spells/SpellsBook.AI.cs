#region License GNU GPL
// SpellsBook.AI.cs
// 
// Copyright (C) 2012, 2013 - BehaviorIsManaged
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
using System.Diagnostics;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Effects;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;
using BiM.Protocol.Messages;
using BiM.Behaviors.Game.Items;
using BiM.Protocol.Enums;
using BiM.Protocol.Data;
using BiM.Behaviors.Data.D2O;
using System.Threading.Tasks;

namespace BiM.Behaviors.Game.Spells
{
    public partial class SpellsBook
    {

        #region Availability management
        /// <summary>
        /// Notifies the start of a new fight turn to each spell 
        /// </summary>
        public void FightStart(GameFightStartingMessage msg)
        {
            foreach (Spell spell in m_spells)
            {
                //Character.SendMessage(String.Format("Spell {0} : initialCooldown {1}, maxCastPerTurn {2}, maxCastPerTarget {3}, maxStack {4}, GlobalCoolDown {5}, minCastInterval {6}", spell, spell.LevelTemplate.initialCooldown, spell.LevelTemplate.maxCastPerTurn, spell.LevelTemplate.maxCastPerTarget, spell.LevelTemplate.maxStack, spell.LevelTemplate.globalCooldown, spell.LevelTemplate.minCastInterval), Color.Aquamarine); 
                spell.StartFight();
            }
        }

        /// <summary>
        /// Notifies the end of the fighting turn to each spell 
        /// </summary>
        public void EndTurn()
        {
            foreach (Spell spell in m_spells)
                spell.EndTurn();
        }

        /// <summary>
        /// Notifies the spell that it has been cast on a given target
        /// </summary>        
        public void CastAt(GameActionFightSpellCastMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Spell spell = GetSpell(msg.spellId);
            if (spell == null)
                throw new ArgumentException(String.Format("Spell Id {0} do not exists in the SpellsBook of {1}, with {2} entries", msg.spellId, Character.Name, m_spells.Count));
            spell.CastAt(msg.targetId);
            //Character.SendMessage(string.Format("Spell {0} cast at actor Id {1}. Still available : {2}", spell, msg.targetId, spell.IsAvailable(msg.targetId)));
        }

        public IEnumerable<Spell> GetAvailableSpells(int? TargetId = null, Spells.Spell.SpellCategory? category = null)
        {
            foreach (Spell spell in m_spells)
                if (spell.IsAvailable(TargetId, category))
                    yield return spell;
        }

        /*public bool IsProperTarget(PlayedFighter caster, Fighter target, Spell spell)
        {
            // SpellTargetType
            if (target == null) return spell.LevelTemplate.needFreeCell && spell.IsAvailable(null, null);

            foreach (var spellEffect in spell.GetEffects())
                if (EffectBase.canAffectTarget(spellEffect, spell, caster, target)) return true;
            return false;
        }*/
        #endregion Availability management

        #region Spell selection
        /*public IEnumerable<Spell> GetOrderListOfSimpleBoostSpells(PlayedFighter caster, Fighter target, Spell.SpellCategory category)
        {
            return m_spells.Where(spell => (caster.Stats.CurrentAP >= spell.LevelTemplate.apCost) && spell.IsAvailable(caster.Id, category) && IsProperTarget(caster, target, spell)).OrderByDescending(spell => spell.Level).ThenByDescending(spell => spell.LevelTemplate.minPlayerLevel);
        }

        public IEnumerable<Spell> GetOrderedAttackSpells(PlayedFighter caster, Fighter target, Spell.SpellCategory category = Spell.SpellCategory.Damages)
        {
            Debug.Assert(((category & Spell.SpellCategory.Damages) > 0), "category do not include Damage effects");
            return m_spells.Where(spell =>
                (caster.Stats.CurrentAP >= spell.LevelTemplate.apCost) &&
                spell.IsAvailable(target.Id, category) &&
                IsProperTarget(caster, target, spell))
                .OrderByDescending(spell => (int)(spell.GetSpellDamages(caster, target, Spell.SpellCategory.Damages).Damage) * (caster.Stats.CurrentAP / (int)spell.LevelTemplate.apCost));
        }*/


        public SpellTarget FindBestUsage(PlayedFighter pc, List<int> spellIds, IEnumerable<Cell> possiblePlacement = null)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            PathFinder pathFinder = new PathFinder(pc.Fight, true);
            IEnumerable<Cell> sourceCells = possiblePlacement == null ? pc.GetPossibleMoves(true, true, pathFinder) : possiblePlacement;
            IEnumerable<Fighter> actorsWithoutPC = pc.Fight.AliveActors.Where(actor => actor != pc);
            SpellTarget spellTarget = new SpellTarget();
            foreach (int spellId in spellIds)
            {
                Spell spell = m_spells.FirstOrDefault(sp => sp.Template.id == spellId);
                if (spell == null && spellId == 0)
                    spell = WeaponSpellLike();
                if (spell != null)
                {

                    if (spell.IsAvailable(null) && ((spellId != 0 && pc.CanCastSpells) || (spellId == 0 && pc.CanFight)) && spell.LevelTemplate.apCost <= pc.Stats.CurrentAP && pc.CanCastSpell(spellId))

                        if (spell != null && spell.IsAvailable(null) && spell.LevelTemplate.apCost <= pc.Stats.CurrentAP)
                        {
                            SpellTarget lastSpellTarget = spell.FindBestTarget(pc, sourceCells, actorsWithoutPC, Spell.SpellCategory.All);
                            if (lastSpellTarget != null && lastSpellTarget.Efficiency > spellTarget.Efficiency)
                            {
                                spellTarget = lastSpellTarget;
                                break; // Stop on the first spell with positive efficiency
                            }
                        }
                }
            }
            timer.Stop();
            spellTarget.TimeSpan = timer.Elapsed;
            return spellTarget;
        }

        List<int> IgnoredSpells = new List<int> { 5 /*Trêve*/, 59 /*Corruption*/};

        public SpellTarget FindBestUsage(PlayedFighter pc, Spell.SpellCategory category, bool withWeapon, IEnumerable<Cell> possiblePlacement = null)
        {
            SpellTarget spellTarget = new SpellTarget();
            if (pc.PCStats.CurrentAP <= 0) return spellTarget;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            PathFinder pathFinder = new PathFinder(pc.Fight, true);
            IEnumerable<Cell> sourceCells = possiblePlacement == null ? pc.GetPossibleMoves(true, true, pathFinder) : possiblePlacement;
            IEnumerable<Fighter> actorsWithoutPC = pc.Fight.AliveActors.Where(actor => actor != pc);
            List<Spell> spells = m_spells.ToList();
            if (withWeapon && ((category & (Spell.SpellCategory.Damages | Spell.SpellCategory.Healing)) != 0))
                spells.Add(WeaponSpellLike());
            //logger.Debug("***FindBestUsage {0}, {1} spells in book. {2} PA. {3}/{4} HP ***", category, spells.Count, pc.PCStats.CurrentAP, pc.PCStats.Health, pc.PCStats.MaxHealth);
            Object thisLock = new Object();
            //foreach(Spell spell in spells)
            int NbSpellsChecked = 0;
            Parallel.ForEach(spells, spell =>
            {
                NbSpellsChecked++;
                if (spell != null && !IgnoredSpells.Contains(spell.Template.id))
                {
                    int spellId = spell.Template.id;
                    if (spell.IsAvailable(null) && ((spellId != 0 && pc.CanCastSpells) || (spellId == 0 && pc.CanFight)) && spell.LevelTemplate.apCost <= pc.Stats.CurrentAP && pc.CanCastSpell(spellId))
                    {
                        if ((spell.Categories & category) > 0)
                        {
                            SpellTarget lastSpellTarget = spell.FindBestTarget(pc, sourceCells, actorsWithoutPC, category);
                            if (lastSpellTarget != null && lastSpellTarget.Efficiency > spellTarget.Efficiency)
                                //lock (thisLock)
                                    spellTarget = lastSpellTarget;
                            //if (lastSpellTarget != null)
                            //    logger.Debug("efficiency {0} = {1} ({2})", lastSpellTarget.Spell, lastSpellTarget.Efficiency, lastSpellTarget.Comment);
                                //lock (thisLock)
//                                    logger.Debug("efficiency {0} = ???", spell); 
//                            else
                                //lock (thisLock)
 //                                  logger.Debug("efficiency {0} = {1} ({2})", lastSpellTarget.Spell, lastSpellTarget.Efficiency, lastSpellTarget.Comment);
                        }
                    }
                    else
                    {
  //                      lock (thisLock)
  //                          logger.Info("{0} skipped : available={1} ({6}), canUse={2}, ApCost={3}, CanCast({4})={5}", spell, spell.IsAvailable(null), ((spellId != 0 && pc.CanCastSpells) || (spellId == 0 && pc.CanFight)), spell.LevelTemplate.apCost <= pc.Stats.CurrentAP, spellId, pc.CanCastSpell(spellId), spell.AvailabilityExplainString(null));
                    }
                }
            }
            );
            //Debug.Assert(NbSpellsChecked == spells.Count);
            timer.Stop();
            spellTarget.TimeSpan = timer.Elapsed;
            //pc.Character.SendInformation("Spell {0} selected - efficientcy : {1} - comment = {2}", spellTarget.Spell, spellTarget.Efficiency, spellTarget.Comment);
            return spellTarget;
        }

        #endregion Spell selection

        public Spell WeaponSpellLike()
        {
            Weapon weapon = Character.Inventory.GetEquippedWeapon();
            if (weapon == null)
            {
                Character.SendWarning("No weapon avalaible");
                return null;
            }
            if (!Character.CheckCriteria(weapon.criteria))
            {
                Character.SendWarning("{0} do not meet criteria {1}", Character, weapon.criteria);
                return null;
            }

            if (weapon.type == null)
            {
                weapon.type = ObjectDataManager.Instance.Get<ItemType>(weapon.typeId);
                if (weapon.type == null)
                {
                    Character.SendError(string.Format("The weapon type is unknown {0}", weapon.typeId));
                    return null;
                }
            }
            string rawZone = weapon.type.rawZone;
            Spell weaponSpellLike = new WeaponSpell(weapon);
            if (weaponSpellLike == null)
                Character.SendError("Failed to convert weapon to spell");
            return weaponSpellLike;
        }

    }
}
