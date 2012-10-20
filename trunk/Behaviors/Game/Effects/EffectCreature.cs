using System;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Effects
{
    [Serializable]
    public class EffectCreature : EffectBase
    {
        protected short m_monsterfamily;

        public EffectCreature()
        {
            
        }

        public EffectCreature(EffectCreature copy)
            : this(copy.Id, copy.MonsterFamily, copy)
        {
            
        }

        public EffectCreature(short id, short monsterfamily, EffectBase effectBase)
            : base(id, effectBase)
        {
            m_monsterfamily = monsterfamily;
        }

        public EffectCreature(ObjectEffectCreature effect)
            : base (effect)
        {
            m_monsterfamily = effect.monsterFamilyId;
        }

        public EffectCreature(EffectInstanceCreature effect)
            : base(effect)
        {
            m_monsterfamily = (short) effect.monsterFamilyId;
        }

        public override int ProtocoleId
        {
            get { return 71; }
        }

        public short MonsterFamily
        {
            get { return m_monsterfamily; }
        }

        public override object[] GetValues()
        {
            return new object[] {m_monsterfamily};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectCreature(Id, MonsterFamily);
        }

        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceCreature()
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
                monsterFamilyId = (uint) MonsterFamily,
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectCreature))
                return false;
            return base.Equals(obj) && m_monsterfamily == (obj as EffectCreature).m_monsterfamily;
        }

        public static bool operator ==(EffectCreature a, EffectCreature b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectCreature a, EffectCreature b)
        {
            return !(a == b);
        }

        public bool Equals(EffectCreature other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_monsterfamily == m_monsterfamily;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_monsterfamily.GetHashCode();
            }
        }
    }
}