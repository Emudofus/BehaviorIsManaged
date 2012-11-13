#region License GNU GPL
// EffectInteger.cs
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
    public class EffectInteger : EffectBase
    {
        protected short m_value;

        public EffectInteger()
        {
            
        }

        public EffectInteger(EffectInteger copy)
            : this(copy.Id, copy.Value, copy)
        {
            
        }

        public EffectInteger(short id, short value, EffectBase effect)
            : base(id, effect)
        {
            m_value = value;
        }

        public EffectInteger(ObjectEffect effect, int value)
            : base (effect)
        {
            m_value = (short) value;
        }

        public EffectInteger(ObjectEffectInteger effect)
            : base(effect)
        {
            m_value = effect.value;
        }

        public EffectInteger(EffectInstanceInteger effect)
            : base(effect)
        {
            m_value = (short) effect.value;
        }

        public override int ProtocoleId
        {
            get { return 70; }
        }

        public short Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {Value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectInteger(Id, Value);
        }

        public bool Equals(EffectInteger other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_value == m_value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as EffectInteger);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_value.GetHashCode();
            }
        }

        public static bool operator ==(EffectInteger left, EffectInteger right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectInteger left, EffectInteger right)
        {
            return !Equals(left, right);
        }
    }
}