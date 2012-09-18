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

        public double F
        {
            get { return Heuristic + Cost; }
        }

        public double Cost;
        public double Heuristic;
        public Cell Parent;
        public NodeState Status;
    }

    public enum NodeState : byte
    {
        None,
        Open,
        Closed
    }

    // todo : centralize informations between IMapDataProvider and CellsInformationsProvider
    /// <summary>
    /// This class use a derived A* algorithm to find a path between two points with the given informations.
    /// </summary>
    public class Pathfinder
    {
        private readonly Map m_map;
        private readonly IMapDataProvider m_mapDataProvider;
        private readonly bool m_throughEntities;
        private readonly bool m_useLogNodeSearch;
        public static int EstimateHeuristic = 10;
        public static int DiagonalCost = 15;
        public static int HorizontalCost = 10;
        public static int SearchLimit = 500;

        private static readonly DirectionsEnum[] Directions = new[]
            {
                DirectionsEnum.DIRECTION_WEST,
                DirectionsEnum.DIRECTION_NORTH_WEST,
                DirectionsEnum.DIRECTION_NORTH,
                DirectionsEnum.DIRECTION_SOUTH_WEST,
                DirectionsEnum.DIRECTION_NORTH_EAST,
                DirectionsEnum.DIRECTION_SOUTH,
                DirectionsEnum.DIRECTION_SOUTH_EAST,
                DirectionsEnum.DIRECTION_EAST,
            };

        private static readonly int[] DiagonalsDirections = new int[]
            {
                0, 2, 5, 7
            };
            
        private static double GetHeuristic(Cell pointA, Cell pointB)
        {
            return EstimateHeuristic * pointA.DistanceTo(pointB);
        }

        public Pathfinder(Map map, IMapDataProvider mapDataProvider, bool throughEntities = true, bool useLogNodeSearch = false)
        {
            m_map = map;
            m_mapDataProvider = mapDataProvider;
            m_throughEntities = throughEntities;
            // the dofus client use a bad linear algorithm to find the closest node.
            // if we use an other sort method the result may be different
            m_useLogNodeSearch = useLogNodeSearch;
        }
            
        public Path FindPath(Cell startCell, Cell endCell, bool allowDiagonals, int movementPoints = (short)-1)
        {
            var success = false;

            var matrix = new PathNode[Map.MapSize + 1];
            IOpenList openList = m_useLogNodeSearch ? (IOpenList)new LogOpenList(new ComparePfNodeMatrix(matrix)) : new LinearOpenList(new ComparePfNodeMatrix(matrix));
            var closedList = new List<PathNode>();

            var location = startCell;
            var counter = 0;

            if (movementPoints == 0)
                return Path.GetEmptyPath(m_map, startCell);

            matrix[location.Id].Cell = location;
            matrix[location.Id].Parent = null;
            matrix[location.Id].Cost = 0;
            matrix[location.Id].Heuristic = 0;
            matrix[location.Id].Status = NodeState.Open;

            var distToEnd = startCell.ManhattanDistanceTo(endCell);
            var endCellAux = startCell;

            openList.Push(location);
            while (openList.Count > 0)
            {
                location = openList.Pop();
                matrix[location.Id].Status = NodeState.Closed;

                if (location == endCell)
                {
                    success = true;
                    break;
                }

                if (counter > SearchLimit)
                    return Path.GetEmptyPath(m_map, startCell);

                for (int i = 0; i < 8; i++)
                {
                    var isDiagonal = DiagonalsDirections.Contains(i);

                    if (isDiagonal && !allowDiagonals)
                        continue;

                    var newLocation = location.GetNearestCellInDirection(Directions[i]);

                    if (newLocation == null)
                        continue;

                    if (newLocation.Id < 0 || newLocation.Id >= Map.MapSize)
                        continue;

                    if (matrix[newLocation.Id].Status == NodeState.Closed)
                        continue;

                    if (!m_mapDataProvider.IsCellWalkable(newLocation))
                        continue;

                    double baseCost;

                    if (newLocation == endCell)
                        baseCost = 1;
                    else
                        baseCost = GetCellCost(newLocation, m_throughEntities);

                    double cost = matrix[location.Id].Cost + baseCost * ( isDiagonal ? DiagonalCost : HorizontalCost );

                    // adjust the cost if the current cell is aligned with the start cell or the end cell
                    if (m_throughEntities)
                    {
                        bool alignedWithEnd = newLocation.X + newLocation.Y == endCell.X + endCell.Y ||
                            newLocation.X - newLocation.Y == endCell.X - endCell.Y;
                        bool alignedWithStart = newLocation.X + newLocation.Y == startCell.X + startCell.Y ||
                            newLocation.X - newLocation.Y == startCell.X - startCell.Y;

                        if (!alignedWithEnd || !alignedWithStart)
                        {
                            cost += newLocation.ManhattanDistanceTo(endCell) + newLocation.ManhattanDistanceTo(startCell);
                        }

                        // tests diagonales now
                        if (newLocation.X == endCell.X || newLocation.Y == endCell.Y)
                            cost -= 3;

                        if (alignedWithEnd || !isDiagonal)
                            cost -= 2;

                        if (newLocation.X == startCell.X || newLocation.Y == startCell.Y)
                            cost -= 3;

                        if (alignedWithStart)
                            cost -= 2;

                        var currentDistToEnd = newLocation.ManhattanDistanceTo(endCell);

                        if (currentDistToEnd < distToEnd)
                        {
                            // if aligned with end
                            if (newLocation.X == endCell.X || newLocation.Y == endCell.Y ||
                                alignedWithEnd)
                            {
                                distToEnd = currentDistToEnd;
                                endCellAux = newLocation;
                            }
                        }
                    }

                    if (matrix[newLocation.Id].Status == NodeState.Open)
                    {
                        if (matrix[newLocation.Id].Cost <= cost)
                            continue;

                        matrix[newLocation.Id].Parent = location;
                        matrix[newLocation.Id].Cost = cost;
                    }
                    else
                    {
                        matrix[newLocation.Id].Cell = newLocation;
                        matrix[newLocation.Id].Parent = location;
                        matrix[newLocation.Id].Cost = cost;
                        matrix[newLocation.Id].Heuristic = GetHeuristic(newLocation, endCell);

                        openList.Push(newLocation);
                    }

                    matrix[newLocation.Id].Status = NodeState.Open;
                }

                counter++;
            }

            if (success)
            {
                var node = matrix[endCell.Id];

                // use auxiliary end if not found
                if (node.Status != NodeState.Closed)
                    node = matrix[endCellAux.Id];

                while (node.Parent != null)
                {
                    closedList.Add(node);
                    node = matrix[node.Parent.Id];
                }

                closedList.Add(node);
            }

            closedList.Reverse();

            if (allowDiagonals)
                return CreateAndOptimisePath(closedList);
            else
            {
                if (movementPoints > 0 && closedList.Count + 1 > movementPoints)
                    return new Path(m_map, closedList.Take(movementPoints + 1).Select(entry => entry.Cell));

                return new Path(m_map, closedList.Select(entry => entry.Cell));
            }
        }

        private Path CreateAndOptimisePath(List<PathNode> nodes)
        {
            var cells = new List<Cell>();
            var len = nodes.Count;

            for (int i = 0; i < len; i++)
            {
                var node = nodes[i];
                var cell = node.Cell;

                cells.Add(cell);

                if (i + 2 < len && cell.ManhattanDistanceTo(nodes[i + 2].Cell) == 1 &&
                    !cell.IsChangeZone(nodes[i + 1].Cell) &&
                    !nodes[i + 1].Cell.IsChangeZone(nodes[i + 2].Cell))
                {
                    i++;
                }

                else if (i + 3 < len && cell.ManhattanDistanceTo(nodes[i + 3].Cell) == 2)
                {
                    var middle = new Point(cell.X + (int)Math.Round(( nodes[i + 3].Cell.X - cell.X ) / 2d),
                        cell.Y + (int)Math.Round(( nodes[i + 3].Cell.Y - cell.Y ) / 2d));

                    var middleCell = m_map.Cells[middle];

                    if (GetCellCost(middleCell, true) < 2 && m_mapDataProvider.IsCellWalkable(middleCell, false, cell))
                    {
                        cells.Add(middleCell);
                        i += 2;
                    }
                }

                else if (i + 2 < len && node.Cell.ManhattanDistanceTo(nodes[i + 2].Cell) == 2)
                {
                    var middleCell = nodes[i + 1].Cell;
                    var nextCell = nodes[i + 2].Cell;
                    var middleCell2X = m_map.Cells[cell.X, middleCell.Y];
                    var middleCell2Y = m_map.Cells[middleCell.X, cell.Y];

                    // cell aligned to nextcell but not to middle cell
                    if (((cell.X + cell.Y == nextCell.X + nextCell.Y && cell.X - cell.Y != middleCell.X - middleCell.Y) ||
                        (cell.X - cell.Y == nextCell.X - nextCell.Y && cell.X - cell.Y != middleCell.X - middleCell.Y )) && 
                        !cell.IsChangeZone(middleCell) && 
                        !middleCell.IsChangeZone(nextCell))
                    {
                        // then ignore middle cell
                        i++;
                    }

                    else if (cell.X == nextCell.X && cell.X != middleCell.X && GetCellCost(middleCell2X, true) < 2 && m_mapDataProvider.IsCellWalkable(middleCell2X, false, cell))
                    {
                        cells.Add(middleCell2X);
                        i++;
                    }
                    else if (cell.Y == nextCell.Y && cell.Y != middleCell.Y && GetCellCost(middleCell2Y, true) < 2 && m_mapDataProvider.IsCellWalkable(middleCell2Y, false, cell))
                    {
                        cells.Add(middleCell2Y);
                        i++;
                    }
                }
            }

            return new Path(m_map, cells);
        }

        private double GetCellCost(Cell cell, bool throughEntities)
        {
            var speed = cell.Speed;

            if (throughEntities)
            {
                if (m_mapDataProvider.GetContextActors(cell).Length > 0)
                    return 20;

                if (speed >= 0)
                    return 1 + 5 - speed;

                return 1 + 11 + Math.Abs(speed);
            }

            var cost = 1d;
            Cell adjCell;

            if (m_mapDataProvider.GetContextActors(cell).Length > 0)
                cost += 0.3;

            adjCell = m_map.Cells[cell.X + 1, cell.Y];
            if (adjCell != null && m_mapDataProvider.GetContextActors(adjCell).Length > 0)
                cost += 0.3;

            adjCell = m_map.Cells[cell.X, cell.Y + 1];
            if (adjCell != null && m_mapDataProvider.GetContextActors(adjCell).Length > 0)
                cost += 0.3;

            adjCell = m_map.Cells[cell.X - 1, cell.Y];
            if (adjCell != null && m_mapDataProvider.GetContextActors(adjCell).Length > 0)
                cost += 0.3;

            adjCell = m_map.Cells[cell.X, cell.Y - 1];
            if (adjCell != null && m_mapDataProvider.GetContextActors(adjCell).Length > 0)
                cost += 0.3;

            if (m_mapDataProvider.IsCellMarked(cell))
                cost += 0.2;

            return cost;
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