#region License GNU GPL

// Cell.cs
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
using System.Drawing;
using BiM.Behaviors.Game.World.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Game.World
{
    public class Cell : ICell, IComparable
    {
        public const int StructSize = 2 + 2 + 1 + 1 + 1 + 1;

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

        public Cell(Map map, CellData cell)
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

        public static short? GetCellFromPoint(Point point)
        {
            return GetCellFromPoint(point.X, point.Y);
        }

        public static short? GetCellFromPoint(int x, int y)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            int lowPart = (y + (x - y) / 2);
            int highPart = x - y;
            if (lowPart < 0 || lowPart >= Map.Width)
                return null;

            if (highPart < 0 || highPart > 39)
                return null;

            int result = (int)(highPart * Map.Width + lowPart);
            if (result >= Map.MapSize || result < 0)
                return null;

            return (short)result;
        }

        public static Point GetPointFromCell(short id)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            if (id < 0 || id > Map.MapSize)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + id + ").");

            Point point = s_orthogonalGridReference[id];

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

            bytes[0] = (byte)(Id >> 8);
            bytes[1] = (byte)(Id & 0xFF);

            bytes[2] = (byte)(Floor >> 8);
            bytes[3] = (byte)(Floor & 0xFF);

            bytes[4] = LosMov;
            bytes[5] = MapChangeData;
            bytes[6] = Speed;

            bytes[7] = MoveZone;

            return bytes;
        }

        public void Deserialize(byte[] data, int index = 0)
        {
            Id = (short)((data[index + 0] << 8) | data[index + 1]);

            Floor = (short)((data[index + 2] << 8) | data[index + 3]);

            LosMov = data[index + 4];
            MapChangeData = data[index + 5];
            Speed = data[index + 6];

            MoveZone = data[index + 7];
        }

        #endregion

        #region Geometry

        public uint DistanceTo(Cell cell)
        {
            return (uint)Math.Sqrt((cell.X - Point.X) * (cell.X - Point.X) + (cell.Y - Point.Y) * (cell.Y - Point.Y));
        }

        public uint ManhattanDistanceTo(Cell cell)
        {
            if (cell == null) return 255;
            return (uint)(Math.Abs(Point.X - cell.X) + Math.Abs(Point.Y - cell.Y));
        }

        public bool IsInRadius(Cell cell, int radius)
        {
            return ManhattanDistanceTo(cell) <= radius;
        }

        public bool IsInRadius(Cell cell, int minRadius, int radius)
        {
            uint dist = ManhattanDistanceTo(cell);
            return dist >= minRadius && dist <= radius;
        }

        public bool IsAdjacentTo(Cell cell, bool diagonal = true)
        {
            uint dist = diagonal ? DistanceTo(cell) : ManhattanDistanceTo(cell);

            return dist == 1;
        }

        public static bool IsInMap(Cell cell)
        {
            return IsInMap(cell.X, cell.Y);
        }

        public static bool IsInMap(int x, int y)
        {
            return x + y >= 0 && x - y >= 0 && x - y < Map.Height * 2 && x + y < Map.Width * 2;
        }

        public DirectionsEnum OrientationTo(Cell cell, Boolean diagonal = true)
        {
            int dx = cell.X - X;
            int dy = Y - cell.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            double angleInRadians = Math.Acos(dx / distance);

            double angleInDegrees = angleInRadians * 180 / Math.PI;
            double transformedAngle = angleInDegrees * (cell.Y > Y ? (-1) : (1));

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
                                 X = cell.X > Point.X ? (1) : (cell.X < Point.X ? (-1) : (0)),
                                 Y = cell.Y > Point.Y ? (1) : (cell.Y < Point.Y ? (-1) : (0))
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

        public IEnumerable<Cell> GetCellsInDirection(DirectionsEnum direction, short minDistance, short maxDistance)
        {
            for (short distance = minDistance; distance <= maxDistance; distance++)
            {
                Cell cell = GetCellInDirection(direction, distance);
                if (cell != null)
                    yield return cell;
            }
        }

        public IEnumerable<Cell> GetCellsInDirections(IEnumerable<DirectionsEnum> directions, short minDistance, short maxDistance)
        {
            foreach (DirectionsEnum direction in directions)
                foreach (Cell cell in GetCellsInDirection(direction, minDistance, maxDistance))
                    yield return cell;
        }

        public Cell GetNearestCellInDirection(DirectionsEnum direction)
        {
            return GetCellInDirection(direction, 1);
        }

        public IEnumerable<Cell> GetAdjacentCells(bool diagonals = false)
        {
            return GetAdjacentCells(IsInMap, diagonals);
        }

        public IEnumerable<Cell> GetAdjacentCells(Func<Cell, bool> predicate, bool diagonal = false)
        {
            Cell northEast = Map.Cells[Point.X, Point.Y + 1];
            if (northEast != null && IsInMap(northEast.X, northEast.Y) && predicate(northEast))
                yield return northEast;

            Cell northWest = Map.Cells[Point.X - 1, Point.Y];
            if (northWest != null && IsInMap(northWest.X, northWest.Y) && predicate(northWest))
                yield return northWest;

            Cell southEast = Map.Cells[Point.X + 1, Point.Y];
            if (southEast != null && IsInMap(southEast.X, southEast.Y) && predicate(southEast))
                yield return southEast;

            Cell southWest = Map.Cells[Point.X, Point.Y - 1];
            if (southWest != null && IsInMap(southWest.X, southWest.Y) && predicate(southWest))
                yield return southWest;

            if (diagonal)
            {
                Cell north = Map.Cells[Point.X - 1, Point.Y + 1];
                if (north != null && IsInMap(north.X, north.Y) && predicate(north))
                    yield return north;

                Cell east = Map.Cells[Point.X + 1, Point.Y + 1];
                if (east != null && IsInMap(east.X, east.Y) && predicate(east))
                    yield return east;

                Cell south = Map.Cells[Point.X + 1, Point.Y - 1];
                if (south != null && IsInMap(south.X, south.Y) && predicate(south))
                    yield return south;

                Cell west = Map.Cells[Point.X - 1, Point.Y - 1];
                if (west != null && IsInMap(west.X, west.Y) && predicate(west))
                    yield return west;
            }
        }

        public IEnumerable<Cell> GetAllCellsInRange(int minRange, int maxRange, bool ignoreThis, Func<Cell, bool> predicate)
        {
            for (int x = X - maxRange; x <= X + maxRange; x++)
                for (int y = Y - maxRange; y <= Y + maxRange; y++)
                    if (!ignoreThis || x != X || y != Y)
                    {
                        int distance = Math.Abs(x - X) + Math.Abs(y - Y);
                        if (IsInMap(x, y) && distance <= maxRange && distance >= minRange)
                        {
                            Cell cell = Map.Cells[x, y];
                            if (cell != null && (predicate == null || predicate(cell))) yield return cell;
                        }
                    }
        }

        public IEnumerable<Cell> GetAllCellsInRectangle(Cell oppositeCell, bool skipStartAndEndCells, Func<Cell, bool> predicate)
        {
            int x1 = Math.Min(oppositeCell.X, X),
                y1 = Math.Min(oppositeCell.Y, Y),
                x2 = Math.Max(oppositeCell.X, X),
                y2 = Math.Max(oppositeCell.Y, Y);
            for (int x = x1; x <= x2; x++)
                for (int y = y1; y <= y2; y++)
                    if (!skipStartAndEndCells || (!(x == X && y == Y) && !(x == oppositeCell.X && y == oppositeCell.Y)))
                    {
                        Cell cell = Map.Cells[x, y];
                        if (cell != null && (predicate == null || predicate(cell))) yield return cell;
                    }
        }

        public Cell[] GetCellsBetween(Cell cell, bool includeVertex = true)
        {
            int dx = cell.X - X;
            int dy = cell.Y - Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            double vx = dx / distance;
            double vy = dy / distance;
            int roundedDistance = (int)distance;

            var result = new Cell[includeVertex ? roundedDistance + 1 : roundedDistance - 1];
            int i = 0;
            if (includeVertex)
                result[i++] = this;

            double x = X + vx;
            double y = Y + vx;

            while (i < roundedDistance)
            {
                x += vx;
                y += vy;
                result[i++] = Map.Cells[(int)x, (int)y];
            }

            if (includeVertex)
                result[i] = cell;

            return result;
        }

        public IEnumerable<Cell> GetCellsInLine(Cell destination)
        {
            // http://playtechs.blogspot.fr/2007/03/raytracing-on-grid.html

            int dx = Math.Abs(destination.X - X);
            int dy = Math.Abs(destination.Y - Y);
            int x = X;
            int y = Y;
            int n = 1 + dx + dy;
            int vectorX = ( destination.X > X ) ? 1 : -1;
            int vectorY = ( destination.Y > Y ) ? 1 : -1;
            int error = dx - dy;
            dx *= 2;
            dy *= 2;

            for (; n > 0; --n)
            {
                yield return Map.Cells[x, y];

                if (error > 0)
                {
                    x += vectorX;
                    error -= dy;
                }
                else if (error == 0)
                {
                    x += vectorX;
                    y += vectorY;
                }
                else
                {
                    y += vectorY;
                    error += dx;
                }
            }
        }


        public bool IsChangeZone(Cell cell)
        {
            return MoveZone != cell.MoveZone && Math.Abs(Floor) == Math.Abs(cell.Floor);
        }

        #endregion
        #region Map Changement
        #endregion

        #region ICell Members

        public short Floor
        {
            get;
            private set;
        }

        public short Id
        {
            get;
            private set;
        }

        public byte LosMov
        {
            get;
            private set;
        }

        public byte MapChangeData
        {
            get;
            private set;
        }

        public byte MoveZone
        {
            get;
            private set;
        }

        public byte Speed
        {
            get;
            private set;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}[{1},{2}]", Id, X, Y);
        }
        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj is Cell) return this.Id.CompareTo((obj as Cell).Id);
            if (obj is short) return this.Id.CompareTo(obj as short?);
            throw new InvalidOperationException(string.Format("Can't compare a Cell and a {0}", obj.GetType().Name)); 
        }
        #endregion IComparable
    }
}