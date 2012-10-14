using System.Collections.Generic;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class Line : IShape
    {
        public Line(byte radius)
        {
            Radius = radius;
            Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return (uint)Radius + 1;
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

            for (int i = (int) MinRadius; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DIRECTION_WEST:
                        AddCellIfValid(centerCell.X - i, centerCell.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH:
                        AddCellIfValid(centerCell.X - i, centerCell.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_EAST:
                        AddCellIfValid(centerCell.X + i, centerCell.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                        AddCellIfValid(centerCell.X + i, centerCell.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        AddCellIfValid(centerCell.X - i, centerCell.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        AddCellIfValid(centerCell.X, centerCell.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        AddCellIfValid(centerCell.X + i, centerCell.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        AddCellIfValid(centerCell.X, centerCell.Y + i, map, result);
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
        #endregion
    }
}