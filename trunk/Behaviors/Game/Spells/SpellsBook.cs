using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Spells
{
    public class SpellsBook : INotifyPropertyChanged
    {
        public SpellsBook(PlayedCharacter owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Character = owner;
            Spells = new ObservableCollection<Spell>();
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

        public ObservableCollection<Spell> Spells
        {
            get;
            set;
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
            Spells.Clear();
            foreach (var spell in msg.spells.Select(entry => new Spell(entry)))
            {
                Spells.Add(spell);
            }

            SpellPrevisualization = msg.spellPrevisualization;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}