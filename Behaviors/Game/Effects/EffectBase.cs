#region License GNU GPL
// EffectBase.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Effects
{
    [Serializable]
    public class EffectBase : ICloneable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string m_description;

        [NonSerialized]
        protected Effect m_template;

        private uint m_zoneMinSize;
        private uint m_zoneSize;

        public EffectBase()
        {
        }

        public EffectBase(EffectBase effect)
        {
            Id = effect.Id;
            m_template = ObjectDataManager.Instance.Get<Effect>(effect.Id);
            Targets = effect.Targets;
            Delay = effect.Delay;
            Duration = effect.Duration;
            Group = effect.Group;
            Random = effect.Random;
            Modificator = effect.Modificator;
            Trigger = effect.Trigger;
            Hidden = effect.Hidden;
            m_zoneSize = effect.m_zoneSize;
            m_zoneMinSize = effect.m_zoneMinSize;
            ZoneShape = effect.ZoneShape;
        }

        public EffectBase(short id, EffectBase effect)
        {
            Id = id;
            m_template = ObjectDataManager.Instance.Get<Effect>(id);
            Targets = effect.Targets;
            Delay = effect.Delay;
            Duration = effect.Duration;
            Group = effect.Group;
            Random = effect.Random;
            Modificator = effect.Modificator;
            Trigger = effect.Trigger;
            Hidden = effect.Hidden;
            m_zoneSize = effect.m_zoneSize;
            m_zoneMinSize = effect.m_zoneMinSize;
            ZoneShape = effect.ZoneShape;
        }

        public EffectBase(short id, int targetId, int duration, int delay, int random, int group, int modificator, bool trigger, bool hidden, uint zoneSize, uint zoneShape, uint zoneMinSize)
        {
            Id = id;
            m_template = ObjectDataManager.Instance.Get<Effect>(id);
            Targets = (SpellTargetType) targetId;
            Delay = delay;
            Duration = duration;
            Group = group;
            Random = random;
            Modificator = modificator;
            Trigger = trigger;
            Hidden = hidden;
            m_zoneSize = zoneSize;
            m_zoneMinSize = zoneMinSize;
            ZoneMinSize = zoneMinSize;
            ZoneSize = zoneSize;
            ZoneShape = (SpellShapeEnum) zoneShape;
        }

        public EffectBase(ObjectEffect effect)
        {
            Id = effect.actionId;
            try
            {
                m_template = ObjectDataManager.Instance.Get<Effect>(Id);
            }
            catch (Exception ex)
            {
                logger.Debug("Can't find effect Id {0} : {1}", Id, ex.Message);
                m_template = new Effect();
            }
        }

        public EffectBase(EffectInstance effect)
        {
            Id = (short) effect.effectId;
            m_template = ObjectDataManager.Instance.Get<Effect>(effect.effectId);
            Targets = (SpellTargetType) effect.targetId;
            Delay = effect.delay;
            Duration = effect.duration;
            Group = effect.group;
            Random = effect.random;
            Modificator = effect.modificator;
            Trigger = effect.trigger;
            Hidden = effect.hidden;
            m_zoneMinSize = effect.zoneMinSize;
            m_zoneSize = effect.zoneSize;
            ZoneShape = (SpellShapeEnum) effect.zoneShape;            
        }

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public short Id
        {
            get;
            protected set;
        }

        public string Description
        {
            get
            {
                if (m_description != null)
                    return m_description;

                var pattern = I18NDataManager.Instance.ReadText(Template.descriptionId);

                var decoder = new StringPatternDecoder(pattern, GetValues());

                int? index;
                if ((index = decoder.CheckValidity(false)) != null)
                    return string.Format("Error in pattern '{0}' at index {1}", pattern, index);

                return m_description = decoder.Decode();
            }
        }

        public uint Priority
        {
            get { return Template.effectPriority; }
        }

        public Effect Template
        {
            get { return m_template; }
            protected set { m_template = value; }
        }

        public string Operator
        {
            get { return Template.@operator; }
        }

        public SpellTargetType Targets
        {
            get;
            protected set;
        }

        public int Duration
        {
            get;
            protected set;
        }

        public int Delay
        {
            get;
            protected set;
        }

        public int Random
        {
            get;
            protected set;
        }

        public int Group
        {
            get;
            protected set;
        }

        public int Modificator
        {
            get;
            protected set;
        }

        public bool Trigger
        {
            get;
            protected set;
        }

        public bool Hidden
        {
            get;
            protected set;
        }

        public uint ZoneSize
        {
            get { return m_zoneSize >= 63 ? (byte) 63 : (byte) m_zoneSize; }
            protected set { m_zoneSize = value; }
        }

        public SpellShapeEnum ZoneShape
        {
            get;
            protected set;
        }

        public uint ZoneMinSize
        {
            get { return m_zoneMinSize >= 63 ? (byte) 63 : (byte) m_zoneMinSize; }
            protected set { m_zoneMinSize = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        public static EffectBase CreateInstance(ObjectEffect effect)
        {
            if (effect is ObjectEffectLadder)
                return new EffectLadder(effect as ObjectEffectLadder);
            if (effect is ObjectEffectCreature)
                return new EffectCreature(effect as ObjectEffectCreature);
            if (effect is ObjectEffectDate)
                return new EffectDate(effect as ObjectEffectDate);
            if (effect is ObjectEffectDice)
                return new EffectDice(effect as ObjectEffectDice);
            if (effect is ObjectEffectDuration)
                return new EffectDuration(effect as ObjectEffectDuration);
            if (effect is ObjectEffectMinMax)
                return new EffectMinMax(effect as ObjectEffectMinMax);
            if (effect is ObjectEffectMount)
                return new EffectMount(effect as ObjectEffectMount);
            if (effect is ObjectEffectString)
                return new EffectString(effect as ObjectEffectString);
            if (effect is ObjectEffectInteger)
                return new EffectInteger(effect as ObjectEffectInteger);

            return new EffectBase(effect);
        }

        public static EffectBase CreateInstance(EffectInstance effect)
        {
            if (effect is EffectInstanceLadder)
                return new EffectLadder(effect as EffectInstanceLadder);
            if (effect is EffectInstanceCreature)
                return new EffectCreature(effect as EffectInstanceCreature);
            if (effect is EffectInstanceDate)
                return new EffectDate(effect as EffectInstanceDate);
            if (effect is EffectInstanceDice)
                return new EffectDice(effect as EffectInstanceDice);
            if (effect is EffectInstanceDuration)
                return new EffectDuration(effect as EffectInstanceDuration);
            if (effect is EffectInstanceMinMax)
                return new EffectMinMax(effect as EffectInstanceMinMax);
            if (effect is EffectInstanceMount)
                return new EffectMount(effect as EffectInstanceMount);
            if (effect is EffectInstanceString)
                return new EffectString(effect as EffectInstanceString);
            if (effect is EffectInstanceInteger)
                return new EffectInteger(effect as EffectInstanceInteger);

            return new EffectBase(effect);
        }

        public virtual object[] GetValues()
        {
            return new object[0];
        }

        public virtual ObjectEffect GetObjectEffect()
        {
            return new ObjectEffect(Id);
        }

        public virtual EffectInstance GetEffectInstance()
        {
            return new EffectInstance
                       {
                           effectId = (uint) Id,
                           targetId = (int) Targets,
                           delay = Delay,
                           duration = Duration,
                           @group = Group,
                           random = Random,
                           modificator = Modificator,
                           trigger = Trigger,
                           hidden = Hidden,
                           zoneMinSize = ZoneMinSize,
                           zoneSize = ZoneSize,
                           zoneShape = (uint) ZoneShape
                       };
        }

        protected void OnDescriptionChanged()
        {
            m_description = null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EffectBase)) return false;
            return Equals((EffectBase) obj);
        }

        public static bool operator ==(EffectBase left, EffectBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectBase left, EffectBase right)
        {
            return !Equals(left, right);
        }

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}