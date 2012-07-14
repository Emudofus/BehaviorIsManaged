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
            Owner = owner;
            Spells = new ObservableCollection<Spell>();
        }

        public SpellsBook(PlayedCharacter owner, SpellListMessage list)
        {
            if (list == null) throw new ArgumentNullException("list");
            Owner = owner;
            Spells = new ObservableCollection<Spell>(list.spells.Select(entry => new Spell(entry)));
            SpellPrevisualization = list.spellPrevisualization;
        }

        public PlayedCharacter Owner
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
            throw new NotImplementedException();
        }

        public bool CanUpgradeSpell(Spell spell)
        {
            throw new NotImplementedException();
        }

        public bool UpgradeSpell(Spell spell)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}