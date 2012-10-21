using System;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Effects
{
    [Serializable]
    public class EffectBase : ICloneable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [NonSerialized]
        protected Effect m_template;

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

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

        public EffectBase()
        {
            
        }

        public EffectBase(EffectBase effect)
        {
            Id = effect.Id;
            m_template = DataProvider.Instance.Get<Effect>(effect.Id);
            Targets = (SpellTargetType)effect.Targets;
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
            m_template = DataProvider.Instance.Get<Effect>(id);
            Targets = (SpellTargetType)effect.Targets;
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
            m_template = DataProvider.Instance.Get<Effect>(id);
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
            m_template = DataProvider.Instance.Get<Effect>(Id);
        }

        public EffectBase(EffectInstance effect)
        {
            Id = (short)effect.effectId;
            m_template = DataProvider.Instance.Get<Effect>(effect.effectId);
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

        public short Id
        {
            get;
            protected set;
        }

        public EffectsEnum Name
        {
            get { return (EffectsEnum) Id; }
            set { Id = (short) value; }
        }

        public Effect Template
        {
            get
            {
                return m_template;
            }
            protected set { m_template = value; }
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

        private uint m_zoneSize;

        public uint ZoneSize
        {
            get { return m_zoneSize >= 63 ? (byte)63 : (byte)m_zoneSize; }
            protected set { m_zoneSize = value; }
        }

        public SpellShapeEnum ZoneShape
        {
            get;
            protected set;
        }      
        
        private uint m_zoneMinSize;

        public uint ZoneMinSize
        {
            get
            {
                return m_zoneMinSize >= 63 ? (byte)63 : (byte)m_zoneMinSize;
            }
            protected set
            {
                m_zoneMinSize = value;
            }
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
            return new EffectInstance()
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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}