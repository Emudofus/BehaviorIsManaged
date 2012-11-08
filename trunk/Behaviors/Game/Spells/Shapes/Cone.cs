#region License GNU GPL
// Cone.cs
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
using System.Collections.Generic;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class Cone : IShape
    {
        public Cone(byte minRadius, byte radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return ( (uint)Radius + 1 ) * ( (uint)Radius + 1 );
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

            int i = 0;
            int j = 1;
            int y = 0;
            int x = 0;
            switch (Direction)
            {
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    x = centerCell.X;
                    while (x >= centerCell.X - Radius)
                    {
                        y = -i;
                        while (y <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(centerCell.X - x) + Math.Abs(y) >= MinRadius)
                                AddCellIfValid(x, y + centerCell.Y, map, result);

                            y++;
                        }
                        i = i + j;
                        x--;
                    }
                    break;
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    y = centerCell.Y;
                    while (y >= centerCell.Y - Radius)
                    {
                        x = -i;
                        while (x <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(x) + Math.Abs(centerCell.Y - y) >= MinRadius)
                                AddCellIfValid(x + centerCell.X, y, map, result);

                            x++;
                        }
                        i = i + j;
                        y--;
                    }
                    break;
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    x = centerCell.X;
                    while (x <= centerCell.X + Radius)
                    {
                        y = -i;
                        while (y <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(centerCell.X - x) + Math.Abs(y) >= MinRadius)
                                AddCellIfValid(x, y + centerCell.Y, map, result);

                            y++;
                        }
                        i = i + j;
                        x++;
                    }
                    break;
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    y = centerCell.Y;
                    while (y <= centerCell.Y - Radius)
                    {
                        x = -i;
                        while (x <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(x) + Math.Abs(centerCell.Y - y) >= MinRadius)
                                AddCellIfValid(x + centerCell.X, y, map, result);

                            x++;
                        }
                        i = i + j;
                        y++;
                    }
                    break;

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