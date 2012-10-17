using System;
using System.Collections.Generic;
using System.Drawing;
using BiM.Protocol.Enums;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Game.World
{
    public class Cell
    {
        public const int StructSize = 2 + 2 + 1 + 1 + 1 + 4;

        private static readonly Point[] s_orthogonalGridReference = new Point[Map.MapSize];
        private static bool m_initialized;

        private static readonly Point s_vectorRight = new Point(1, 1);
        private static readonly Point s_vectorDownRight = new Point(1, 0);
        private static readonly Point s_vectorDown = new Point(1, -1);
        private static readonly Point s_vectorDownLeft = new Point(0, -1);
        private static readonly Point s_vectorLeft = new Point(-1, -1);
        private static readonly Point s_vectorUpLeft = new Point(-1, 0);
        private static readonly Point s_vectorUp = new Point(-1, 1);
        private static readonly Point s_vectorUpRight = new Point(0, 1);

        public short Floor;
        public short Id;
        public byte LosMov;
        public byte MapChangeData;
        public uint MoveZone;
        public byte Speed;

        private Point? m_point;

        public Cell(Map map, DlmCellData cell)
        {
            Map = map;
            Id = cell.Id;
            Floor = cell.Floor;
            LosMov = cell.LosMov;
            MapChangeData = cell.MapChangeData;
            MoveZone = cell.MoveZone;
            Speed = cell.Speed;
        }

        public Map Map
        {
            get;
            private set;
        }

        public Point Point
        {
            get { return m_point != null ? m_point.Value : (m_point = GetPointFromCell(Id)).Value; }
        }

        public int X
        {
            get { return Point.X; }
        }

        public int Y
        {
            get { return Point.Y; }
        }

        public bool Walkable
        {
            get { return (LosMov & 1) == 1; }
        }

        public bool LineOfSight
        {
            get { return (LosMov & 2) == 2; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) == 4; }
        }

        public bool Red
        {
            get { return (LosMov & 8) == 8; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) == 16; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) == 32; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) == 64; }
        }

        public bool NonWalkableDuringRP
        {
            get { return (LosMov & 128) == 128; }
        }

        #region Point-Cell

        public static short GetCellFromPoint(Point point)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            return (short) ((point.X - point.Y)*Map.Width + point.Y + (point.X - point.Y)/2);
        }

        public static Point GetPointFromCell(short id)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            if (id < 0 || id > Map.MapSize)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + id + ").");

            var point = s_orthogonalGridReference[id];

            return point;
        }

        /// <summary>
        /// Initialize a static 2D plan that is used as reference to convert a cell to a (X,Y) point
        /// </summary>
        private static void InitializeStaticGrid()
        {
            int posX = 0;
            int posY = 0;
            int cellCount = 0;

            for (int x = 0; x < Map.Height; x++)
            {
                for (int y = 0; y < Map.Width; y++)
                    s_orthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posX++;

                for (int y = 0; y < Map.Width; y++)
                    s_orthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posY--;
            }

            m_initialized = true;
        }

        #endregion

        #region Serialization

        public byte[] Serialize()
        {
            var bytes = new byte[StructSize];

            bytes[0] = (byte) (Id >> 8);
            bytes[1] = (byte) (Id & 0xFF);

            bytes[2] = (byte) (Floor >> 8);
            bytes[3] = (byte) (Floor & 0xFF);

            bytes[4] = LosMov;
            bytes[5] = MapChangeData;
            bytes[6] = Speed;

            bytes[7] = (byte) (MoveZone >> 24);
            bytes[8] = (byte) (MoveZone >> 16);
            bytes[9] = (byte) (MoveZone >> 8);
            bytes[10] = (byte) (MoveZone & 0xFF);

            return bytes;
        }

        public void Deserialize(byte[] data, int index = 0)
        {
            Id = (short) ((data[index + 0] << 8) | data[index + 1]);

            Floor = (short) ((data[index + 2] << 8) | data[index + 3]);

            LosMov = data[index + 4];
            MapChangeData = data[index + 5];
            Speed = data[index + 6];

            MoveZone = (uint) ((data[index + 7] << 24) | (data[index + 8] << 16) | (data[index + 9] << 8) | (data[index + 10]));
        }

        #endregion

        #region Geometry

        public uint DistanceTo(Cell cell)
        {
            return (uint)Math.Sqrt(( cell.X - Point.X ) * ( cell.X - Point.X ) + ( cell.Y - Point.Y ) * ( cell.Y - Point.Y ));
        }

        public uint ManhattanDistanceTo(Cell cell)
        {
            return (uint)( Math.Abs(Point.X - cell.X) + Math.Abs(Point.Y - cell.Y) );
        }

        public bool IsInRadius(Cell cell, int radius)
        {
            return DistanceTo(cell) <= radius;
        }

        public bool IsInRadius(Cell cell, int minRadius, int radius)
        {
            var dist = DistanceTo(cell);
            return dist >= minRadius && dist <= radius;
        }

        public bool IsAdjacentTo(Cell cell)
        {
            return ManhattanDistanceTo(cell) == 1;
        }

        public static bool IsInMap(Cell cell)
        {
            return IsInMap(cell.X, cell.Y);
        }

        public static bool IsInMap(int x, int y)
        {
            return x + y >= 0 && x - y >= 0 && x - y < Map.Height*2 && x + y < Map.Width*2;
        }

        public DirectionsEnum OrientationTo(Cell cell, Boolean diagonal = true)
        {
            int dx = cell.X - X;
            int dy = Y - cell.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            double angleInRadians = Math.Acos(dx / distance);

            double angleInDegrees = angleInRadians * 180 / Math.PI;
            double transformedAngle = angleInDegrees * ( cell.Y > Y ? ( -1 ) : ( 1 ) );

            double orientation = !diagonal ? Math.Round(transformedAngle / 90) * 2 + 1 : Math.Round(transformedAngle / 45) + 1;

            if (orientation < 0)
            {
                orientation = orientation + 8;
            }

            return (DirectionsEnum)(uint)orientation;
        }

        public DirectionsEnum OrientationToAdjacent(Cell cell)
        {
            var vector = new Point
                             {
                                 X = cell.X > Point.X ? ( 1 ) : ( cell.X < Point.X ? ( -1 ) : ( 0 ) ),
                                 Y = cell.Y > Point.Y ? ( 1 ) : ( cell.Y < Point.Y ? ( -1 ) : ( 0 ) )
                             };

            if (vector == s_vectorRight)
            {
                return DirectionsEnum.DIRECTION_EAST;
            }
            if (vector == s_vectorDownRight)
            {
                return DirectionsEnum.DIRECTION_SOUTH_EAST;
            }
            if (vector == s_vectorDown)
            {
                return DirectionsEnum.DIRECTION_SOUTH;
            }
            if (vector == s_vectorDownLeft)
            {
                return DirectionsEnum.DIRECTION_SOUTH_WEST;
            }
            if (vector == s_vectorLeft)
            {
                return DirectionsEnum.DIRECTION_WEST;
            }
            if (vector == s_vectorUpLeft)
            {
                return DirectionsEnum.DIRECTION_NORTH_WEST;
            }
            if (vector == s_vectorUp)
            {
                return DirectionsEnum.DIRECTION_NORTH;
            }
            if (vector == s_vectorUpRight)
            {
                return DirectionsEnum.DIRECTION_NORTH_EAST;
            }

            return DirectionsEnum.DIRECTION_EAST;
        }

        /// <summary>
        /// Returns null if the cell is not in the map
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public Cell GetCellInDirection(DirectionsEnum direction, short step)
        {
            Point mapPoint;
            switch (direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    {
                        mapPoint = new Point(Point.X + step, Point.Y + step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    {
                        mapPoint = new Point(Point.X + step, Point.Y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH:
                    {
                        mapPoint = new Point(Point.X + step, Point.Y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    {
                        mapPoint = new Point(Point.X, Point.Y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_WEST:
                    {
                        mapPoint = new Point(Point.X - step, Point.Y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    {
                        mapPoint = new Point(Point.X - step, Point.Y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH:
                    {
                        mapPoint = new Point(Point.X - step, Point.Y + step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    {
                        mapPoint = new Point(Point.X, Point.Y + step);
                        break;
                    }
                default:
                    throw new Exception("Unknown direction : " + direction);
            }

            if (IsInMap(mapPoint.X, mapPoint.Y))
                return Map.Cells[mapPoint];
            return null;
        }

        public Cell GetNearestCellInDirection(DirectionsEnum direction)
        {
            return GetCellInDirection(direction, 1);
        }

        public IEnumerable<Cell> GetAdjacentCells()
        {
            return GetAdjacentCells(IsInMap);
        }

        public IEnumerable<Cell> GetAdjacentCells(Func<Cell, bool> predicate)
        {
            var southEast = Map.Cells[Point.X + 1, Point.Y];
            if (southEast != null && predicate(southEast))
                yield return southEast;

            var southWest =  Map.Cells[Point.X, Point.Y - 1];
            if (southWest != null && predicate(southWest))
                yield return southWest;

            var northEast =  Map.Cells[Point.X, Point.Y + 1];
            if (IsInMap(northEast.X, northEast.Y) && predicate(northEast))
                yield return northEast;

            var northWest =  Map.Cells[Point.X - 1, Point.Y];
            if (IsInMap(northWest.X, northWest.Y) && predicate(northWest))
                yield return northWest;
        }

        public bool IsChangeZone(Cell cell)
        {
            return MoveZone != cell.MoveZone && Math.Abs(Floor) == Math.Abs(cell.Floor);
        }

        #endregion

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}