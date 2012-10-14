using System.Collections.Generic;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Spells.Shapes
{
    public class HalfLozenge : IShape
    {
        public HalfLozenge(byte minRadius, byte radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = DirectionsEnum.DIRECTION_NORTH;
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