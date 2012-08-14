using System;
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Spells
{
    public class Spell : INotifyPropertyChanged
    {
        private int m_level;

        public Spell(SpellItem spell)
        {
            if (spell == null) throw new ArgumentNullException("spell");
            Template = DataProvider.Instance.Get<Protocol.Data.Spell>(spell.spellId);
            Level = spell.spellLevel;
            Position = spell.position;
        }

        public Protocol.Data.Spell Template
        {
            get;
            set;
        }

        public int Level
        {
            get { return m_level; }
            set
            {
                m_level = value;
                LevelTemplate = GetLevelTemplate(m_level);
            }
        }

        public SpellLevel LevelTemplate
        {
            get;
            private set;
        }

        // note, always equal to 63
        public byte Position
        {
            get;
            set;
        }

        private SpellLevel GetLevelTemplate(int level)
        {
            if (Template.spellLevels.Count <= Level)
                return null;

            return DataProvider.Instance.Get<SpellLevel>((int) Template.spellLevels[level]);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}