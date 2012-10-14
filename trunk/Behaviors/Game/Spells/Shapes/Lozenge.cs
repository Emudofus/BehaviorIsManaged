using System;
using System.Collections.Generic;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class Lozenge : IShape
    {
        public Lozenge(byte minRadius, byte radius)
        {
            MinRadius = minRadius;
            Radius = radius;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return ( (uint)Radius + 1 ) * ( (uint)Radius + 1 ) + Radius * (uint)Radius;
            }
        }

        public byte MinRadius
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public byte Radius
        {
            get;
            set;
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            var result = new List<Cell>();

            if (Radius == 0)
            {
                if (MinRadius == 0)
                    result.Add(centerCell);

                return result.ToArray();
            }

            int x = (int) (centerCell.X - Radius);
            int y = 0;
            int i = 0;
            int j = 1;
            while (x <= centerCell.X + Radius)
            {
                y = -i;

                while (y <= i)
                {
                    if (MinRadius == 0 || Math.Abs(centerCell.X - x) + Math.Abs(y) >= MinRadius)
                        AddCellIfValid(x, y + centerCell.Y, map, result);

                    y++;
                }

                if (i == Radius)
                {
                    j = -j;
                }

                i = i + j;
                x++;
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, Map map, IList<Cell> container)
        {
            if (!Cell.IsInMap(x, y))
                return;

            container.Add(map.Cells[x, y]);
        }

        #endregion
    }
}