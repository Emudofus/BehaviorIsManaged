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
using System.Collections.Generic;
using System.Diagnostics;
using BiM.Behaviors.Game.Effects;

namespace BiM.Behaviors.Game.Spells
{
    public partial class Spell : INotifyPropertyChanged
    {

        public const string UNKNOWN = "<Unknown>";
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
                InitAI();
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
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Name 
        { 
            get 
            { 
                if (Template != null) return DataProvider.Instance.Get<string>(Template.nameId);
                return UNKNOWN; 
            } 
        }

        public string Description
        {
            get
            {
                if (Template != null) return DataProvider.Instance.Get<string>(Template.descriptionId);
                return UNKNOWN;
            }
        }

        public string ToString(bool detailed)
        {
            if (Template != null)
            {
                if (detailed)
                    return String.Format("{0} {1} : {2}", Name, Level, Description);
                else
                    return ToString();
            }
            return UNKNOWN;
        }

        public override string ToString()
        {
            if (Template != null)
            {
                return String.Format("{0} {1}", Name, Level);
            }
            return UNKNOWN;
        }


    }
}