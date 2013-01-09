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
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Effects;
using BiM.Core.Collections;
using BiM.Core.Reflection;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Spells
{
    public partial class SpellsBook : INotifyPropertyChanged
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
            : this(owner)
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
            if (Character.IsFighting()) return false;
            if (spell.Level > Character.Stats.SpellsPoints) return false;
            if (!HasSpell(spell.Template.id)) return false;
            if (spell.Template.spellLevels.Count <= spell.Level) return false;
            if (spell.Level == 5 && spell.LevelTemplate.minPlayerLevel + 100 > Character.Level) return false;
            return true;
        }

        public bool UpgradeSpell(Spell spell)
        {
            throw new NotImplementedException();
        }

        // Initializes full SpellsBook
        public void Update(SpellListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");

            m_spells.Clear();
            foreach (SpellItem spell in msg.spells)
                m_spells.Add(new Spell(spell));

            SpellPrevisualization = msg.spellPrevisualization;

            //FullDump();
        }

        // Learns a new spell
        public void Update(SpellUpgradeSuccessMessage message)
        {
            Spell newSpell = new Spell(message);
            Spell known = m_spells.FirstOrDefault(spell => spell.Template.id == newSpell.Template.id);
            if (known != null)
                m_spells[m_spells.IndexOf(known)] = newSpell;
            else
                m_spells.Add(newSpell);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        #region debug tool
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void FullDump()
        {
            ObjectDumper dumper = new ObjectDumper(4, true, true, (System.Reflection.BindingFlags.FlattenHierarchy));
            logger.Error("Dump of the spellbook of {0} : ", Character.Name);
            foreach (var spell in m_spells)
            {
                logger.Error("   Spell {0}", spell.ToString(true));
                foreach (var effectdice in spell.LevelTemplate.effects)
                {
                    EffectBase effect = new EffectBase(effectdice);
                    logger.Error("       Effect {0} : {1} - {2} {3:P}", effect.Description, effectdice.diceNum <= effectdice.diceSide ? effectdice.diceNum : effectdice.diceSide, effectdice.diceNum > effectdice.diceSide ? effectdice.diceNum : effectdice.diceSide, effectdice.random == 0 ? 1.0 : effectdice.random / 100.0);
                }
            }
        }

        public string GetFullDetail()
        {
            string st = string.Empty;
            foreach (var spell in m_spells)
                st += spell.GetFullDescription() + "\r\n";
            return st;
        }
        #endregion debug tool

    }
}