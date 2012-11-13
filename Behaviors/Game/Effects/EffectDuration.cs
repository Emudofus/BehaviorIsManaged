#region License GNU GPL
// EffectDuration.cs
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
    public class EffectDuration : EffectBase
    {
        protected short m_days;
        protected short m_hours;
        protected short m_minutes;

        public EffectDuration()
        {
            
        }

        public EffectDuration(EffectDuration copy)
            : this(copy.Id, copy.m_days, copy.m_hours, copy.m_minutes, copy)
        {
            
        }

        public EffectDuration(short id, short days, short hours, short minutes, EffectBase effect)
            : base(id, effect)
        {
            m_days = days;
            m_hours = hours;
            m_minutes = minutes;
        }

        public EffectDuration(ObjectEffectDuration effect)
            : base (effect)
        {
            m_days = effect.days;
            m_hours = effect.hours;
            m_minutes = effect.minutes;
        }

        public EffectDuration(EffectInstanceDuration effect)
            : base(effect)
        {
            m_days = (short) effect.days;
            m_hours = (short) effect.hours;
            m_minutes = (short) effect.minutes;
        }

        public override int ProtocoleId
        {
            get { return 75; }
        }

        public override object[] GetValues()
        {
            return new object[] {m_days,  m_hours,  m_minutes};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDuration(Id, m_days, m_hours, m_minutes);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceDuration()
            {
                effectId = (uint)Id,
                targetId = (int)Targets,
                delay = Delay,
                duration = Duration,
                group = Group,
                random = Random,
                modificator = Modificator,
                trigger = Trigger,
                hidden = Hidden,
                zoneMinSize = ZoneMinSize,
                zoneSize = ZoneSize,
                zoneShape = (uint) ZoneShape,
                days = (uint) m_days,
                hours = (uint) m_hours,
                minutes = (uint) m_minutes
            };
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan( m_days,  m_hours,  m_minutes, 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDuration))
                return false;
            return base.Equals(obj) && GetTimeSpan().Equals((obj as EffectDuration).GetTimeSpan());
        }

        public static bool operator ==(EffectDuration a, EffectDuration b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDuration a, EffectDuration b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDuration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_days == m_days && other.m_hours == m_hours &&
                   other.m_minutes == m_minutes;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_days.GetHashCode();
                result = (result*397) ^ m_hours.GetHashCode();
                result = (result*397) ^ m_minutes.GetHashCode();
                return result;
            }
        }
    }
}