using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using BiM.Core.Collections;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public struct PathNode
    {
        public Cell Cell;
        public double F;
        public double G;
        public double H;
        public Cell Parent;
        public NodeState Status;
    }

    public enum NodeState : byte
    {
        None,
        Open,
        Closed
    }

    public class Pathfinder
    {
        public static int EstimateHeuristic = 5;
        public static int SearchLimit = 500;

        private static readonly DirectionsEnum[] Directions = new[]
            {
                DirectionsEnum.DIRECTION_SOUTH_WEST,
                DirectionsEnum.DIRECTION_NORTH_WEST,
                DirectionsEnum.DIRECTION_NORTH_EAST,
                DirectionsEnum.DIRECTION_SOUTH_EAST,
                DirectionsEnum.DIRECTION_SOUTH,
                DirectionsEnum.DIRECTION_WEST,
                DirectionsEnum.DIRECTION_NORTH,
                DirectionsEnum.DIRECTION_EAST
            };

        private static double GetHeuristic(Cell pointA, Cell pointB)
        {
            var dxy = new Point(Math.Abs(pointB.X - pointA.X), Math.Abs(pointB.Y - pointA.Y));
            var orthogonalValue = Math.Abs(dxy.X - dxy.Y);
            var diagonalValue = Math.Abs(((dxy.X + dxy.Y) -  orthogonalValue) / 2);

            return EstimateHeuristic * ( diagonalValue + orthogonalValue + dxy.X + dxy.Y );
        }

        public Pathfinder(ICellsInformationProvider cellsInformationProvider)
        {
            CellsInformationProvider = cellsInformationProvider;
        }

        public ICellsInformationProvider CellsInformationProvider
        {
            get;
            private set;
        }

        public Path FindPath(Cell startCell, Cell endCell, bool diagonal, int movementPoints = (short)-1)
        {
            var success = false;

            var matrix = new PathNode[Map.MapSize + 1];
            var openList = new PriorityQueueB<Cell>(new ComparePfNodeMatrix(matrix));
            var closedList = new List<PathNode>();

            var location = startCell;

            var counter = 0;

            if (movementPoints == 0)
                return Path.GetEmptyPath(CellsInformationProvider.Map, startCell);

            matrix[location.Id].Cell = location;
            matrix[location.Id].Parent = null;
            matrix[location.Id].G = 0;
            matrix[location.Id].F = EstimateHeuristic;
            matrix[location.Id].Status = NodeState.Open;

            openList.Push(location);
            while (openList.Count > 0)
            {
                location = openList.Pop();

                if (matrix[location.Id].Status == NodeState.Closed)
                    continue;

                if (location == endCell)
                {
                    matrix[location.Id].Status = NodeState.Closed;
                    success = true;
                    break;
                }

                if (counter > SearchLimit)
                    return Path.GetEmptyPath(CellsInformationProvider.Map, startCell);

                for (int i = 0; i < (diagonal ? 8 : 4); i++)
                {
                    var newLocation = location.GetNearestCellInDirection(Directions[i]);

                    if (newLocation == null)
                        continue;

                    if (newLocation.Id < 0 || newLocation.Id >= Map.MapSize)
                        continue;

                    if (!CellsInformationProvider.IsCellWalkable(newLocation))
                        continue;

                    double newG = matrix[location.Id].G + 1;

                    if (( matrix[newLocation.Id].Status == NodeState.Open ||
                        matrix[newLocation.Id].Status == NodeState.Closed ) &&
                        matrix[newLocation.Id].G <= newG)
                        continue;

                    matrix[newLocation.Id].Cell = newLocation;
                    matrix[newLocation.Id].Parent = location;
                    matrix[newLocation.Id].G = newG;
                    matrix[newLocation.Id].H = GetHeuristic(newLocation, endCell);
                    matrix[newLocation.Id].F = newG + matrix[newLocation.Id].H;

                    openList.Push(newLocation);
                    matrix[newLocation.Id].Status = NodeState.Open;
            }

                counter++;
                matrix[location.Id].Status = NodeState.Closed;
            }

            if (success)
            {
                var node = matrix[endCell.Id];

                while (node.Parent != null)
                {
                    closedList.Add(node);
                    node = matrix[node.Parent.Id];
                }

                closedList.Add(node);
            }

            closedList.Reverse();

            if (movementPoints > 0 && closedList.Count + 1> movementPoints)
                return new Path(CellsInformationProvider.Map, closedList.Take(movementPoints + 1).Select(entry => entry.Cell));

            return new Path(CellsInformationProvider.Map, closedList.Select(entry => entry.Cell));
        }

        #region Nested type: ComparePfNodeMatrix

        internal class ComparePfNodeMatrix : IComparer<Cell>
        {
            private readonly PathNode[] m_matrix;

            public ComparePfNodeMatrix(PathNode[] matrix)
            {
                m_matrix = matrix;
            }

            #region IComparer<ushort> Members

            public int Compare(Cell a, Cell b)
            {
                if (m_matrix[a.Id].F > m_matrix[b.Id].F)
                {
                    return 1;
                }

                if (m_matrix[a.Id].F < m_matrix[b.Id].F)
                {
                    return -1;
                }
                return 0;
            }

            #endregion
        }

        #endregion
    }

}