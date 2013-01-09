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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Effects;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Spells
{
    public partial class Spell : INotifyPropertyChanged
    {
        public const string UNKNOWN = "<Unknown>";
        protected int m_level;

        /// <summary>
        /// This constructor should only be used by derived classes that override getLevelTemplate.
        /// </summary>
        /// <param name="spellTemplate"></param>
        /// <param name="level"></param>
        protected Spell()
        {

        }

        public Spell(SpellItem spell)
        {
            if (spell == null) throw new ArgumentNullException("spell");
            Template = ObjectDataManager.Instance.Get<Protocol.Data.Spell>(spell.spellId);
            Level = spell.spellLevel;
            Position = spell.position;
        }

        public Spell(Protocol.Messages.SpellUpgradeSuccessMessage message)
        {
            if (message == null) throw new ArgumentNullException("spell");
            Template = ObjectDataManager.Instance.Get<Protocol.Data.Spell>(message.spellId);
            Level = message.spellLevel;
            Position = 0; // unkown

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
                FillEffectTemplates();
                InitAI();
            }
        }

        protected void FillEffect(EffectInstanceDice effect)
        {
            if (string.IsNullOrEmpty(effect.rawZone)) return;
            effect.zoneShape = effect.rawZone[0];
            if (effect.rawZone.Length == 1) return;
            string options = effect.rawZone.Remove(0, 1);
            string[] splitted = options.Split(',');
            if (splitted.Length >= 1)
                uint.TryParse(splitted[0], out effect.zoneSize);
            if (splitted.Length >= 2)
                uint.TryParse(splitted[1], out effect.zoneMinSize);
        }

        protected void FillEffectTemplates()
        {
            foreach (var effect in LevelTemplate.effects)
                FillEffect(effect);
            foreach (var effect in LevelTemplate.criticalEffect)
                FillEffect(effect);
        }

        public SpellLevel LevelTemplate
        {
            get;
            protected set;
        }

        // note, always equal to 63
        public byte Position
        {
            get;
            set;
        }

        protected virtual SpellLevel GetLevelTemplate(int level)
        {
            if (Template.spellLevels.Count < Level)
                throw new InvalidOperationException(string.Format("Level {0} doesn't exist in spell {1}", Level, Template.id));

            SpellLevel lv = ObjectDataManager.Instance.Get<SpellLevel>((int)Template.spellLevels[level - 1]);
            if (Template.id == 158) // For Iops, Concentration effects are wrong in D2o
            {
                lv.effects[0].targetId = (int)(SpellTargetType.ALLIES_NON_SUMMON | SpellTargetType.ENEMIES_NON_SUMMON | SpellTargetType.SELF);
                lv.effects[1].targetId = (int)(SpellTargetType.ALLIES_SUMMON | SpellTargetType.ENEMIES_SUMMON);
            }
            if (Template.id == 126) // For Eni, Mot stimulant also affects the enemies (2nd effect)
            {
                lv.effects[1].targetId = (int)(SpellTargetType.ALL);
            }
            
            return lv;
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
                if (Template != null) return I18NDataManager.Instance.ReadText(Template.nameId);
                return UNKNOWN;
            }
        }

        public string Description
        {
            get
            {
                if (Template != null) return I18NDataManager.Instance.ReadText(Template.descriptionId);
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

        public string GetFullDescription()
        {
            string result = String.Format("{0}#{1} ({2})#{3} - cat {4} - {5}\r\n", Name, Template.id, Level, LevelTemplate.id, Categories, Description);
            foreach (var effect in GetEffects())
            {
                result += "    > " + effect.ToString() + "\n\r";
            }
            return result;
        }


    }
}