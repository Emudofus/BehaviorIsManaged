#region License GNU GPL
// SpellsBook.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using BiM.Protocol.Messages;
using System.Collections.Generic;
using BiM.Protocol.Types;
using NLog;
using BiM.Core.Reflection;

namespace BiM.Behaviors.Game.Spells
{
    public class SpellsBook : INotifyPropertyChanged
    {
        private ObservableCollectionMT<Spell> m_spells;
        private ReadOnlyObservableCollectionMT<Spell> m_readOnlySpells; 

        public SpellsBook(PlayedCharacter owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Character = owner;
            m_spells = new ObservableCollectionMT<Spell>();
            m_readOnlySpells = new ReadOnlyObservableCollectionMT<Spell>(m_spells);
        }

        public SpellsBook(PlayedCharacter owner, SpellListMessage list)
            : this (owner)
        {
            if (list == null) throw new ArgumentNullException("list");
            Update(list);
        }

        public PlayedCharacter Character
        {
            get;
            set;
        }

        public ReadOnlyObservableCollectionMT<Spell> Spells
        {
            get { return m_readOnlySpells; }
        }

        public bool SpellPrevisualization
        {
            get;
            set;
        }

        public bool HasSpell(int id)
        {
            return GetSpell(id) != null;
        }

        public Spell GetSpell(int id)
        {
            return Spells.FirstOrDefault(x => x.Template.id == id);
        }

        public bool CanUpgradeSpell(Spell spell)
        {
            throw new NotImplementedException();
        }

        public bool UpgradeSpell(Spell spell)
        {
            throw new NotImplementedException();
        }

        public void Update(SpellListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            m_spells.Clear();
            foreach (SpellItem spell in msg.spells)
                m_spells.Add(new Spell(spell));

            SpellPrevisualization = msg.spellPrevisualization;

            //FullDump();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Availability management
        /// <summary>
        /// Notifies the start of a new fight turn to each spell 
        /// </summary>
        public void FightStart(GameFightStartMessage msg)
        {
            foreach (Spell spell in m_spells)
                spell.StartFight();
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
        }

        public IEnumerable<Spell> GetAvailableSpells(int? targetId = null)
        {
            foreach (Spell spell in m_spells)
                if (spell.IsAvailable(targetId))
                    yield return spell; 
        }

        #endregion Availability management

        #region debug tool
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void FullDump()
        {
            ObjectDumper dumper = new ObjectDumper(4, true, true, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.FlattenHierarchy);
            logger.Error("Dump of the spellbook of {0} : ", Character.Name);
            foreach (var spell in m_spells)
            {
                logger.Error("   Spell {0}", spell.ToString(true));
                foreach (var effect in spell.LevelTemplate.effects)
                    logger.Error("       Effect {0}", dumper.Dump(effect));
                
            }
        }
        #endregion debug tool

    }
}