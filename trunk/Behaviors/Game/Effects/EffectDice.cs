using System;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Effects
{
    [Serializable]
    public class EffectDice : EffectInteger
    {
        protected short m_diceface;
        protected short m_dicenum;

        public EffectDice()
        {
        }

        public EffectDice(EffectDice copy)
            : this(copy.Id, copy.Value, copy.DiceNum, copy.DiceFace, copy)
        {
            
        }

        public EffectDice(short id, short value, short dicenum, short diceface, EffectBase effect)
            : base(id, value, effect)
        {
            m_dicenum = dicenum;
            m_diceface = diceface;
        }

        public EffectDice(ObjectEffectDice effect)
            : base (effect, effect.diceConst)
        {
            m_diceface = effect.diceSide;
            m_dicenum = effect.diceNum;
        }

        public EffectDice(EffectInstanceDice effect)
            : base(effect)
        {
            m_dicenum = (short) effect.diceNum;
            m_diceface = (short) effect.diceSide;
        }

        public override int ProtocoleId
        {
            get { return 73; }
        }

        public short DiceNum
        {
            get { return m_dicenum; }
            set { m_dicenum = value; }
        }

        public short DiceFace
        {
            get { return m_diceface; }
            set { m_diceface = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {DiceNum, DiceFace, Value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDice(Id, DiceNum, DiceFace, Value);
        }

        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceDice()
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
                value = Value,
                diceNum = (uint) DiceNum,
                diceSide = (uint) DiceFace
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDice))
                return false;
            var b = obj as EffectDice;
            return base.Equals(obj) && m_diceface == b.m_diceface && m_dicenum == b.m_dicenum;
        }

        public static bool operator ==(EffectDice a, EffectDice b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDice a, EffectDice b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_diceface == m_diceface && other.m_dicenum == m_dicenum;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_diceface;
                result = (result*397) ^ m_dicenum;
                return result;
            }
        }
    }
}