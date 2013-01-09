#region License GNU GPL
// EffectDice.cs
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
            : base(effect, effect.diceConst)
        {
            m_diceface = effect.diceSide;
            m_dicenum = effect.diceNum;
        }

        public EffectDice(EffectInstanceDice effect)
            : base(effect)
        {
            m_dicenum = (short)effect.diceNum;
            m_diceface = (short)effect.diceSide;
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
            return new object[] { DiceNum, DiceFace, Value };
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
                zoneShape = (uint)ZoneShape,
                value = Value,
                diceNum = (uint)DiceNum,
                diceSide = (uint)DiceFace
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

            if (((object)a == null) || ((object)b == null))
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
                result = (result * 397) ^ m_diceface;
                result = (result * 397) ^ m_dicenum;
                return result;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1}-{2}, Targets: {3}, Area:{4} - {5}", (EffectsEnum)Id, DiceNum, DiceFace, Targets, AreaDesc(), Description);
        }

    }
}