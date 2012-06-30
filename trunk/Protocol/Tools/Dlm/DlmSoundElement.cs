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