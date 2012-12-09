#region License GNU GPL
// PathFinder.cs
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
using System.Diagnostics;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;

namespace BiM.Behaviors.Game.World.Pathfinding.P
{
    public class PathFinder
    {
        private IMapContext m_context;
        private PathNode[] m_cells;
        private PathNode[] m_pathResult;

        public bool IsInFight
        {
            get;
            set;
        }

        /// <summary>
        /// When set to true, attempt to avoid cells adjacent to ennemies
        /// </summary>
        public bool DodgeEnnemies
        {
            get;
            set;
        }

        // Ctor : provides Map and mode (combat or not)
        public PathFinder(IMapContext map, bool fight)
        {
            IsInFight = fight;
            m_context = map;
            m_cells = new PathNode[map.Cells.Count];
            for (int i = 0; i < map.Cells.Count; i++)
                m_cells[i] = new PathNode(map.Cells[i]);
        }

        public void ClearLogic(Cell[] startingCells, Cell[] exitCells)
        {
            foreach (var cell in m_cells)
                if (cell != null)
                {
                    cell.DistanceSteps = PathNode.DEFAULT_DISTANCE;
                    cell.IsInPath = false;
                    cell.IsCloseToEnemy = false;
                }

            if (DodgeEnnemies)
            {
                // Find cells of all enemies on the map
                IEnumerable<Cell> enemyCells;
                if (IsInFight)
                {
                    PlayedFighter player = m_context.Actors.OfType<PlayedFighter>().FirstOrDefault();
                    if (player != null)
                        enemyCells = player.GetOpposedTeam().FightersAlive.Select(fighter => fighter.Cell);
                    else
                        enemyCells = null;
                }
                else
                    enemyCells = m_context.Actors.OfType<GroupMonster>().Select(fighter => fighter.Cell);

                // Remove starting en exit celles from those
                if (enemyCells != null && startingCells != null)
                {
                    if (startingCells.Length > 0)
                        enemyCells = enemyCells.Except(startingCells);
                    if (exitCells != null && exitCells.Length > 0)
                        enemyCells = enemyCells.Except(exitCells);

                    // Then for each remainding cell, set all surrouding cells as "isCloseToEnemy"
                    foreach (var cellid in enemyCells.SelectMany(cell => cell.GetAdjacentCells()).Select(cell => cell.Id))
                        if (m_cells[cellid] != null)
                            m_cells[cellid].IsCloseToEnemy = true;
                }

            }
        }



        /// <summary>
        /// PathFinding main method
        /// </summary>
        /// <param name="startingCells"></param>
        /// <param name="exitCells"></param>
        /// <param name="selectFartherCells"></param>
        /// <param name="firstStepOnly"></param>
        /// <returns></returns>
        public bool FindPath(Cell[] startingCells, Cell[] exitCells, bool selectFartherCells = false, bool firstStepOnly = false)
        {
            var rnd = new Random();
            ClearLogic(startingCells, exitCells);

            if (( startingCells == null ) || ( startingCells.Length == 0 ))
                return false; // We need at least one starting cell
            if (!firstStepOnly && ( exitCells == null || exitCells.Length == 0 ))
                return false; // We need at least one exit cell for step 2
            // PC starts at distance of 0. Set 0 to all possible starting cells
            var changed = new List<PathNode>();
            List<PathNode> changing;

            foreach (var cell in startingCells)
                if (m_cells[cell.Id] != null)
                {
                    m_cells[cell.Id].DistanceSteps = 0;
                    changed.Add(m_cells[cell.Id]);
                    if (exitCells != null && !selectFartherCells && exitCells.Any(ecell => ecell == cell))
                    {
                        m_pathResult = new[] { m_cells[cell.Id] };
                        return true; // Empty path : starting cell = exit cell
                    }
                }

            int maxDistance = CellInfo.DEFAULT_DISTANCE;

            while (changed.Count > 0)
            {
                changing = new List<PathNode>();
                // Look at each square on the board.
                foreach (var current in changed)
                {
                    Debug.Assert(( current != null && current.DistanceSteps < PathNode.DEFAULT_DISTANCE ));

                    foreach (var newCell in ValidMoves(current, false))
                    {
                        int newPass = current.DistanceSteps;
                        if (IsInFight)
                            newPass++;
                        else
                            newPass += newCell.IsDiagonal ? (int)( newCell.Weight * 1.414 ) : newCell.Weight;

                        if (newCell.DistanceSteps > newPass)
                        {
                            newCell.DistanceSteps = newPass;
                            changing.Add(newCell);
                            if (!firstStepOnly && !selectFartherCells && exitCells.Any(cell => newCell.Cell == cell))
                                maxDistance = newPass;
                        }
                    }
                }
                changed = changing;
            }

            if (firstStepOnly)
                return true;

            // Step 2
            // Mark the path from Exit to Starting position.
            // if several Exit cells, then get the lowest distance one = the closest from one starting cell
            // (or the highest distance one if selectFartherCells)
            var exit = exitCells[0];
            int minDist = m_cells[exit.Id].DistanceSteps;
            if (selectFartherCells)
            {
                foreach (var cell in exitCells)
                    if (m_cells[cell.Id].DistanceSteps > minDist)
                    {
                        exit = cell;
                        minDist = m_cells[cell.Id].DistanceSteps;
                    }
            }
            else
            {
                foreach (var cell in exitCells)
                    if (m_cells[cell.Id].DistanceSteps < minDist)
                    {
                        exit = cell;
                        minDist = m_cells[cell.Id].DistanceSteps;
                    }
            }

            var currentCell = exit.Id;
            var result = new List<PathNode>();
            result.Add(m_cells[exit.Id]);
            m_cells[exit.Id].IsInPath = true;
            var lowestPoints = new List<short>(10);
            short lowestPoint;
            int lowest;
            int start = -1;

            while (true)
            {
                // Look through each MapNeighbour and find the square
                // with the lowest number of steps marked.
                lowestPoint = -1;
                lowest = PathNode.DEFAULT_DISTANCE;

                foreach (var newCell in ValidMoves(m_cells[currentCell], true))
                {
                    int distance = newCell.DistanceSteps;
                    if (distance > CellInfo.DEFAULT_DISTANCE)
                    {
                        Debug.Assert(false, "Distance shouldn't be higher than DEFAULT_DISTANCE", "Distance = {0} > Max = {1}", distance, CellInfo.DEFAULT_DISTANCE);
                        continue;
                    }
                    if (distance < lowest)
                    {
                        lowestPoints.Clear();
                        lowest = distance;
                        lowestPoint = newCell.Cell.Id;
                    }
                    else
                        if (distance == lowest)
                        {
                            if (lowestPoints.Count == 0)
                                lowestPoints.Add(lowestPoint);
                            lowestPoints.Add(newCell.Cell.Id);
                        }
                }
                if (lowest == CellInfo.DEFAULT_DISTANCE) break; // Can't find a valid way :(

                if (lowestPoints.Count > 1) // Several points with same distance =>> randomly select one of them
                    lowestPoint = lowestPoints[rnd.Next(lowestPoints.Count)];

                // Mark the square as part of the path if it is the lowest
                // number. Set the current position as the square with
                // that number of steps.
                result.Add(m_cells[lowestPoint]);
                if (result.Count > m_cells.Length)
                {
                    Debug.Assert(false, "PathFinder can't find a path - overflow");
                    break;
                }
                m_cells[lowestPoint].IsInPath = true;
                currentCell = lowestPoint;


                if (m_cells[currentCell].DistanceSteps == 0) // Exit reached            
                {
                    start = currentCell;
                    // We went from closest Exit to a Starting position, so we're finished.
                    break;
                }
            }
            result.Reverse();
            m_pathResult = result.ToArray();
            return currentCell == start;
        }

        private bool SquareOpen(PathNode cell, PathNode originCell = null)
        {
            if (cell == null)
                return false;
            // A square is open if it is not a wall.

            if (IsInFight)
                return cell.IsCombatWalkable &&
                    m_context.IsCellWalkable(cell.Cell, false, originCell == null ? null : originCell.Cell) && !cell.IsCloseToEnemy;

            return m_context.IsCellWalkable(cell.Cell, !DodgeEnnemies, originCell == null ? null : originCell.Cell);
        }

        private PathNode GetCellFromPos(int x, int y)
        {
            int lowPart = ( y + ( x - y ) / 2 );
            int highPart = x - y;

            if (lowPart < 0 || lowPart >= Map.Width)
                return null;

            if (highPart < 0 || highPart > 39)
                return null;

            var result = highPart * Map.Width + lowPart;

            if (result >= Map.MapSize || result < 0)
                return null;

            return m_cells[(short)( ( x - y ) * Map.Width + y + ( x - y ) / 2 )];
        }

        private PathNode GetNeighbourCell(PathNode cell, int deltaX, int deltaY, bool fast)
        {
            var neighbour = GetCellFromPos(cell.Cell.X + deltaX, cell.Cell.Y + deltaY);

            if (fast || neighbour == null || neighbour.DistanceSteps == 0)
                return neighbour;

            if (!SquareOpen(neighbour, cell))
                return null;

            neighbour.IsDiagonal = deltaX != 0 && deltaY != 0;

            return neighbour;
        }

        private IEnumerable<PathNode> ValidMoves(PathNode cell, bool fast)
        {
            PathNode newCell;
            if (( newCell = GetNeighbourCell(cell, 1, 0, fast) ) != null) yield return newCell;
            if (( newCell = GetNeighbourCell(cell, 0, 1, fast) ) != null) yield return newCell;
            if (( newCell = GetNeighbourCell(cell, -1, 0, fast) ) != null) yield return newCell;
            if (( newCell = GetNeighbourCell(cell, 0, -1, fast) ) != null) yield return newCell;
            if (!IsInFight)
            {
                if (( newCell = GetNeighbourCell(cell, 1, 1, fast) ) != null) yield return newCell;
                if (( newCell = GetNeighbourCell(cell, 1, -1, fast) ) != null) yield return newCell;
                if (( newCell = GetNeighbourCell(cell, -1, 1, fast) ) != null) yield return newCell;
                if (( newCell = GetNeighbourCell(cell, -1, -1, fast) ) != null) yield return newCell;
            }
        }

        /// <summary>
        /// Return the "flight distance" between to cells. It gives a rought indication on how far they are, 
        /// without processing full PathFinding.
        /// </summary>
        /// <returns></returns>
        public double GetFlightDistance(int start, int end)
        {
            return GetFlightDistance(m_cells[start], m_cells[end], IsInFight);
        }

        public double GetFlightDistance(PathNode start, PathNode end)
        {
            return GetFlightDistance(start, end, IsInFight);
        }

        private static double GetFlightDistance(PathNode start, PathNode end, bool isInFight)
        {
            int dx = Math.Abs(start.X - end.X);
            int dy = Math.Abs(start.Y - end.Y);
            if (isInFight)
                return dx + dy;
            if (dx > dy)
                return dy * 1.414 /* diagonale part */ + /* straight line part */ dx - dy;
            else
                return dx * 1.414 /* diagonale part */ + /* straight line part */ dy - dx;
        }

        /// <summary>
        /// Gives last index of the list to be used in order to be as close as possible (but under) MinDistance from target
        /// </summary>
        /// <param name="minDistance"></param>
        /// <returns></returns>
        private int GetIndexForDistance(int minDistance)
        {
            if (minDistance <= 0 || m_pathResult.Length <= 1)
                return m_pathResult.Length - 1;
            double distance = 0.0;
            double precDistance = 0.0;
            for (int i = m_pathResult.Length - 1; i > 0; i--)
            {
                distance += GetFlightDistance(m_pathResult[i - 1], m_pathResult[i]);
                if ((int)distance > minDistance) return i - 1;
                //if (Math.Abs(distance - MinDistance) > Math.Abs(precDistance - MinDistance)) return i - 1;
                precDistance = distance;
            }
            return m_pathResult.Length - 1;
        }

        /// <summary>
        /// Remove final steps of the path, so that it ends at MinDistance from target (instead of 0)
        /// Warning : call this only once
        /// </summary>
        /// <param name="minDistance"></param>
        private PathNode[] TruncatePathAtMinDistance(int minDistance)
        {
            if (minDistance <= 0)
                return m_pathResult;

            int index = GetIndexForDistance(minDistance);
            return m_pathResult.Take(index + 1).ToArray();
        }

        /// <summary>
        /// Find the cell in the middle of several cells (barycenter)
        /// </summary>
        /// <returns></returns>
        private PathNode MiddleCell(int[] cells)
        {
            int cumulX = 0, cumulY = 0;
            foreach (int cell in cells)
            {
                cumulX += m_cells[cell].X;
                cumulY += m_cells[cell].Y;
            }
            return GetCellFromPos(cumulX / cells.Length, cumulY / cells.Length);
        }


    }
}