using System;
using System.ComponentModel;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public class StatsRow : INotifyPropertyChanged
    {
        private readonly Action<StatsRow> m_onChanged;

        public StatsRow()
        {
            
        }

        public StatsRow(PlayerField field)
        {
            Field = field;
        }

        internal StatsRow(PlayerField field, Action<StatsRow> onChanged)
            : this(field)
        {
            m_onChanged = onChanged;
        }

        public StatsRow(CharacterBaseCharacteristic characteristic)
        {
            if (characteristic == null) throw new ArgumentNullException("characteristic");
            Base = characteristic.@base;
            Equipements = characteristic.objectsAndMountBonus;
            AlignBonus = characteristic.alignGiftBonus;
            Context = characteristic.contextModif;
        }

        public StatsRow(CharacterBaseCharacteristic characteristic, PlayerField field)
        {
            if (characteristic == null) throw new ArgumentNullException("characteristic");
            Base = characteristic.@base;
            Equipements = characteristic.objectsAndMountBonus;
            AlignBonus = characteristic.alignGiftBonus;
            Context = characteristic.contextModif;
            Field = field;
        }

        internal StatsRow(CharacterBaseCharacteristic characteristic, PlayerField field, Action<StatsRow> onChanged)
        {
            m_onChanged = onChanged;
        }

        public PlayerField Field
        {
            get;
            set;
        }

        public int Base
        {
            get;
            set;
        }

        public int Equipements
        {
            get;
            set;
        }

        public int AlignBonus
        {
            get;
            set;
        }

        public int Context
        {
            get;
            set;
        }

        public int Total
        {
            get { return Base + Equipements + AlignBonus + Context; }
        }

        public static int operator +(int i1, StatsRow s1)
        {
            return i1 + s1.Total;
        }

        public static int operator +(StatsRow s1, StatsRow s2)
        {
            return s1.Total + s2.Total;
        }

        public static int operator -(int i1, StatsRow s1)
        {
            return i1 - s1.Total;
        }

        public static int operator -(StatsRow s1, StatsRow s2)
        {
            return s1.Total - s2.Total;
        }

        public void Update(CharacterBaseCharacteristic characteristic)
        {
            Base = characteristic.@base;
            Equipements = characteristic.objectsAndMountBonus;
            Context = characteristic.contextModif;
            AlignBonus = characteristic.alignGiftBonus;
        }

        public void Update(int @base)
        {
            @Base = @base;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_onChanged != null)
                m_onChanged(this);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }
    }
}