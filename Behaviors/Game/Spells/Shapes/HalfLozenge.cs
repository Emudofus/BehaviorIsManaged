#region License GNU GPL
// HalfLozenge.cs
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
using System.Collections.Generic;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class HalfLozenge : IShape
    {
        public HalfLozenge(byte minRadius, byte radius, DirectionsEnum direction = DirectionsEnum.DIRECTION_NORTH)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = direction;
        }

        public uint Surface
        {
            get
            {
                return (uint)Radius * 2 + 1;
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

            if (MinRadius == 0)
                result.Add(centerCell);

            for (int i = 1; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        AddCellIfValid(centerCell.X + i, centerCell.Y + i, map, result);
                        AddCellIfValid(centerCell.X + i, centerCell.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        AddCellIfValid(centerCell.X - i, centerCell.Y - i, map, result);
                        AddCellIfValid(centerCell.X + i, centerCell.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        AddCellIfValid(centerCell.X - i, centerCell.Y + i, map, result);
                        AddCellIfValid(centerCell.X - i, centerCell.Y - i, map, result);
                        break;

                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        AddCellIfValid(centerCell.X - i, centerCell.Y + i, map, result);
                        AddCellIfValid(centerCell.X + i, centerCell.Y + i, map, result);
                        break;
                }
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, Map map, IList<Cell> container)
        {
            if (!Cell.IsInMap(x, y))
                return;

            container.Add(map.Cells[x, y]);
        }
    }
}