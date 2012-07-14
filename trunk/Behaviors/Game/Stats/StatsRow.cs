using System;
using System.ComponentModel;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public class StatsRow : INotifyPropertyChanged
    {
        public StatsRow()
        {
            
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

        public StatsRow(short @base)
        {
            Base = @base;
        }

        public PlayerField Field
        {
            get;
            set;
        }

        public short Base
        {
            get;
            set;
        }

        public short Equipements
        {
            get;
            set;
        }

        public short AlignBonus
        {
            get;
            set;
        }

        public short Context
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}