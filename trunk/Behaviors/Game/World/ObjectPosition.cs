using System;
using System.ComponentModel;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.World
{
    public class ObjectPosition : INotifyPropertyChanged
    {
        public ObjectPosition(Map map, Cell cell, DirectionsEnum direction)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (cell == null) throw new ArgumentNullException("cell");

            Map = map;
            Cell = cell;
            Direction = direction;
        }

        public ObjectPosition(Map map, EntityDispositionInformations disposition)
            : this(map, map.Cells[disposition.cellId], (DirectionsEnum) disposition.direction)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (disposition == null) throw new ArgumentNullException("disposition"); 
            
            Map = map;
            Cell = map.Cells[disposition.cellId];
            Direction = (DirectionsEnum)disposition.direction;
        }

        public Cell Cell
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public Map Map
        {
            get;
            set;
        }

        public static bool operator==(ObjectPosition pos1, ObjectPosition pos2)
        {
            if (Equals(pos1, null) && Equals(pos2, null))
                return true;

            if (Equals(pos1, null) || Equals(pos2, null))
                return false;

            return pos1.Equals(pos2);
        }

        public static bool operator !=(ObjectPosition pos1, ObjectPosition pos2)
        {
            return !(pos1 == pos2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!( obj is ObjectPosition ))
                return false;

            var pos = (ObjectPosition)obj;

            return pos.Cell.Id == Cell.Id &&
                pos.Direction == Direction &&
                pos.Map == Map;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = ( Cell != null ? Cell.GetHashCode() : 0 );
                result = ( result * 397 ) ^ Direction.GetHashCode();
                result = ( result * 397 ) ^ ( Map != null ? Map.GetHashCode() : 0 );
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}