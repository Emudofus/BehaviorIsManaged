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
#endregion
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
            if (Template.spellLevels.Count < Level)
                throw new InvalidOperationException(string.Format("Level {0} doesn't exist in spell {1}", Level, Template.id));

            return DataProvider.Instance.Get<SpellLevel>((int) Template.spellLevels[level - 1]);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}