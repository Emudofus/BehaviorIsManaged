using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using BiM.Protocol.Messages;

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
            return false;
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
            foreach (var spell in msg.spells.Select(entry => new Spell(entry)))
            {
                m_spells.Add(spell);
            }

            SpellPrevisualization = msg.spellPrevisualization;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}