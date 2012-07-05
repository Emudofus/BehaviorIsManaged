using System;
using System.ComponentModel;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public class SpellModification : INotifyPropertyChanged
    {
        public SpellModification(CharacterSpellModification modification)
        {
            if (modification == null) throw new ArgumentNullException("modification");
            SpellId = modification.spellId;
            ModificationType = (CharacterSpellModificationTypeEnum) modification.modificationType;
            Value = new StatsRow(modification.value);
        }

        public int SpellId
        {
            get;
            set;
        }

        public CharacterSpellModificationTypeEnum ModificationType
        {
            get;
            set;
        }

        public StatsRow Value
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}