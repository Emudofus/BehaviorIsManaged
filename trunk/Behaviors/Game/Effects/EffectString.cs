#region License GNU GPL
// EffectString.cs
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
    public class EffectString : EffectBase
    {
        protected string m_value;

        public EffectString()
        {
            
        }

        public EffectString(EffectString copy)
            : this(copy.Id, copy.m_value, copy)
        {
            
        }

        public EffectString(short id, string value, EffectBase effect)
            : base(id, effect)
        {
            m_value = value;
        }

        public EffectString(ObjectEffectString effect)
            : base(effect)
        {
            m_value = effect.value;
        }

        public EffectString(EffectInstanceString effect)
            : base(effect)
        {
            m_value = effect.text;
        }

        public override int ProtocoleId
        {
            get { return 74; }
        }

        public override object[] GetValues()
        {
            return new object[] {m_value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString(Id, m_value);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceString()
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
                text = m_value
            };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectString))
                return false;
            return base.Equals(obj) && m_value == (obj as EffectString).m_value;
        }

        public static bool operator ==(EffectString a, EffectString b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectString a, EffectString b)
        {
            return !(a == b);
        }

        public bool Equals(EffectString other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.m_value, m_value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (m_value != null ? m_value.GetHashCode() : 0);
            }
        }
    }
}