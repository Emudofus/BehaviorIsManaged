#region License GNU GPL
// DlmSoundElement.cs
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
using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmSoundElement : DlmBasicElement, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int m_baseVolume;
        private int m_fullVolumedistance;
        private int m_maxDelayBetweenloops;
        private int m_minDelayBetweenloops;
        private int m_nullVolumedistance;
        private int m_soundId;

        public DlmSoundElement(DlmCell cell)
            : base(cell)
        {

        }

        public int BaseVolume
        {
            get { return m_baseVolume; }
            set { m_baseVolume = value; }
        }

        public int FullVolumedistance
        {
            get { return m_fullVolumedistance; }
            set { m_fullVolumedistance = value; }
        }

        public int MaxDelayBetweenloops
        {
            get { return m_maxDelayBetweenloops; }
            set { m_maxDelayBetweenloops = value; }
        }

        public int MinDelayBetweenloops
        {
            get { return m_minDelayBetweenloops; }
            set { m_minDelayBetweenloops = value; }
        }

        public int NullVolumedistance
        {
            get { return m_nullVolumedistance; }
            set { m_nullVolumedistance = value; }
        }

        public int SoundId
        {
            get { return m_soundId; }
            set { m_soundId = value; }
        }


        public new static DlmSoundElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var element = new DlmSoundElement(cell);

            element.m_soundId = reader.ReadInt();
            element.m_baseVolume = reader.ReadShort();
            element.m_fullVolumedistance = reader.ReadInt();
            element.m_nullVolumedistance = reader.ReadInt();
            element.m_minDelayBetweenloops = reader.ReadShort();
            element.m_maxDelayBetweenloops = reader.ReadShort();

            return element;
        }
    }
}