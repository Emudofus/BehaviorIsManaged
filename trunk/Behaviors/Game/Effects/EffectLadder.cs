using System;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Effects
{
    [Serializable]
    public class EffectLadder : EffectCreature
    {
        protected short m_monsterCount;

        public short MonsterCount
        {
            get { return m_monsterCount; }
            set { m_monsterCount = value; }
        }

        public EffectLadder()
        {
            
        }

        public EffectLadder(EffectLadder copy)
            : this (copy.Id, copy.MonsterFamily, copy.MonsterCount, copy)
        {
            
        }

        public EffectLadder(short id, short monsterfamily, short monstercount, EffectBase effect)
            : base(id, monsterfamily, effect)
        {
            m_monsterCount = monstercount;
        }

        public EffectLadder(ObjectEffectLadder effect)
            : base(effect)
        {
            m_monsterCount = (short)effect.monsterCount;
        }

        public EffectLadder(EffectInstanceLadder effect)
            : base(effect)
        {
            m_monsterCount = (short) effect.monsterCount;
        }

        public override int ProtocoleId
        {
            get { return 81; }
        }

        public override object[] GetValues()
        {
            return new object[] { m_monsterCount, m_monsterfamily };
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectLadder(Id, MonsterFamily, MonsterCount);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceLadder()
            {
                effectId = (uint)Id,
                targetId = (int)Targets,
                delay = Delay,
                duration = Duration,
                @group = Group,
                random = Random,
                modificator = Modificator,
                trigger = Trigger,
                hidden = Hidden,
                zoneMinSize = ZoneMinSize,
                zoneSize = ZoneSize,
                zoneShape = (uint) ZoneShape,
                monsterCount = (uint) m_monsterCount,
                monsterFamilyId = (uint) m_monsterfamily,
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectLadder))
                return false;
            return base.Equals(obj) && m_monsterCount == (obj as EffectLadder).m_monsterCount;
        }

        public static bool operator ==(EffectLadder a, EffectLadder b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectLadder a, EffectLadder b)
        {
            return !(a == b);
        }

        public bool Equals(EffectLadder other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_monsterCount == m_monsterCount;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_monsterCount.GetHashCode();
            }
        }
    }
}