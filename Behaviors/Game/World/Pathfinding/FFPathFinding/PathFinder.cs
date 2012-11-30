#region License GNU GPL
// Spell.cs
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

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Drawing;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class PathFinder : ISimplePathFinder, IAdvancedPathFinder
    {
        #region Properties
        public MapMovement PathPacker;

        // When in combat, then directions are resticted to 4, and distances are manhattan distances
        private bool _isInFight;

        // Cells stores information about each square.
        private CellInfo[] _cells;

        public List<short> PathResult { get; private set; }

        //public Dictionary<int, CellInfo> StartingCells;
        //public Dictionary<int, CellInfo> ExitCells;
        public short StartingCell { get; private set; }
        public short ExitCell { get; private set; }
        private Map _map;
        #endregion Properties

        #region Constructor
        // Ctor : provides Map and mode (combat or not)
        public PathFinder(Map map, bool fight)
        {
            _isInFight = fight;
            _map = map;
            _cells = new CellInfo[map.Cells.Count];
            for (int i = 0; i < map.Cells.Count; i++)
                _cells[i] = new CellInfo(map.Cells[i]);
            PathPacker = new MapMovement(_cells);
        }

        // Ctor : provides specific cells and mode (combat or not)
        public PathFinder(Map map, IEnumerable<Cell> cells, bool fight)
        {
            _map = map;
            _isInFight = fight;
            _cells = new CellInfo[_map.Cells.Count];
            foreach (Cell cell in cells)
                _cells[cell.Id] = new CellInfo(cell);
            PathPacker = new MapMovement(_cells);
        }
        #endregion Constructor

        /// <summary>
        /// Reset old PathFinding path from the cells.
        /// </summary>
        public void ClearLogic()
        {
            // Reset some information about the cells.
            foreach (CellInfo cell in _cells)
                if (cell != null)
                {
                    cell.distanceSteps = CellInfo.DEFAULT_DISTANCE;
                    cell.isInPath1 = false;
                    cell.isInPath2 = false;
                }
            if (PathResult == null)
                PathResult = new List<short>();
            else
                PathResult.Clear();
            StartingCell = CellInfo.CELL_ERROR;
            ExitCell = CellInfo.CELL_ERROR;
        }


        #region SubArea filler
        // Used to find next unset closed area in World map and tag all corresponding cells
        private bool FindNextSubArea(byte SubMapNo)
        {
            short? FirstCellId = null;
            foreach (var cell in _cells)
            {
                if (cell.subMapId == 0 && SquareOpen(cell))
                {
                    FirstCellId = cell.cellId;
                    break;
                }
            }
            if (!FirstCellId.HasValue) return false;
            FindPath(new short[] { FirstCellId.Value }, null, false, true); // Only 1st step
            bool otherSubAreaToDetect = false;
            foreach (var cell in _cells)
            {
                // Mark each cell accessible from starting one
                if (cell.distanceSteps != CellInfo.DEFAULT_DISTANCE)
                    cell.subMapId = SubMapNo;
                else
                    if (cell.subMapId == 0 && SquareOpen(cell))
                        otherSubAreaToDetect = true;
            }
            return otherSubAreaToDetect;
        }

        // Identify each unlinked 'submaps' (sets of cells that are not linked together)  
        public byte SubMapFiller()
        {

            // Reset SubArea data 
            foreach (var cell in _cells)
            {
                // Mark each cell as from unset subarea
                cell.subMapId = 0;
            }

            byte SubAreaNo = 0;
            while (FindNextSubArea(++SubAreaNo)) ;
            return SubAreaNo;
        }
        #endregion

        #region FindPath algorithm itself

        /// <summary>
        /// Entry point for PathFinding algorithm, with one starting cell, and one exit
        /// </summary>
        public bool FindPath(short StartingCell, short ExitCell)
        {
            short[] StartingCells = new short[] { StartingCell };
            short[] ExitCells = new short[] { ExitCell };
            return FindPath(StartingCells, ExitCells, false);
        }

        /// <summary>
        /// Entry point for PathFinding algorithm, with on starting cell and several exits
        /// </summary>
        public bool FindPath(short StartingCell, short[] ExitCells, bool SelectFartherCells = false)
        {
            short[] StartingCells = new short[] { StartingCell };
            return FindPath(StartingCells, ExitCells, SelectFartherCells);
        }

        /// <summary>
        /// Entry point for PathFinding algorithm, with several starting cells, and one exit
        /// </summary>
        public bool FindPath(short[] StartingCells, short ExitCell)
        {
            short[] ExitCells = new short[] { ExitCell };
            return FindPath(StartingCells, ExitCells, false);
        }

        /// <summary>
        /// Flee away from a set of foes, starting on a given cell
        /// * REVERSED PATH FINDING *
        /// </summary>
        public bool FleeFromFoes(short MyCell, short[] FoeCells, int distance)
        {
            // Set all Foes as starting points, Exit cell as exit (not used anyway in part 1 of PathFinding)
            short[] ExitCells = new short[] { MyCell };
            FindPath(FoeCells, ExitCells, false, true);
            // Step 2
            ExitCell = ExitCells[0];
            short CurrentCell = ExitCell;
            PathResult.Add(ExitCell);
            _cells[ExitCell].isInPath1 = true;
            int NbStepLeft = distance;
            while (NbStepLeft-- > 0)
            {
                // Look through each direction and find the square
                // with the lowest number of steps marked.
                short highestPoint = CellInfo.CELL_ERROR;
                int PreviousDistance = _cells[CurrentCell].distanceSteps;
                int highest = PreviousDistance;
                foreach (CellInfo NewCell in ValidMoves(_cells[CurrentCell]))
                {
                    int count = NewCell.distanceSteps;
                    if (count > highest)
                    {
                        highest = count;
                        highestPoint = NewCell.cellId;
                    }
                }
                if (highest != PreviousDistance)
                {
                    // Mark the square as part of the path if it is the lowest
                    // number. Set the current position as the square with
                    // that number of steps.
                    PathResult.Add(highestPoint);
                    _cells[highestPoint].isInPath1 = true;
                    CurrentCell = highestPoint;
                    if (PathResult.Count > _cells.Length)
                    {
                        Debug.Assert(false, "PathFinder can't find a path - overflow");
                        break;
                    }
                }
                else
                {
                    // Can't find a longer path => stop now :(
                    break;
                }
            }
            //PathResult.Reverse(); // Reverse the path, as we started from exit
            return PathResult.Count > 0;
        }

        /// <summary>
        /// PathFinding main method
        /// </summary>
        /// <param name="StartingCells"></param>
        /// <param name="ExitCells"></param>
        /// <param name="SelectFartherCells"></param>
        /// <param name="FirstStepOnly"></param>
        /// <returns></returns>
        public bool FindPath(short[] StartingCells, short[] ExitCells, bool SelectFartherCells = false, bool FirstStepOnly = false)
        {
            Random rnd = new Random();
            ClearLogic();

            if ((StartingCells == null) || (StartingCells.Length == 0)) return false; // We need at least one starting cell
            if (!FirstStepOnly && (ExitCells == null || ExitCells.Length == 0)) return false; // We need at least one exit cell for step 2
            // PC starts at distance of 0. Set 0 to all possible starting cells
            foreach (short cell in StartingCells)
                if (_cells[cell] != null)
                    _cells[cell].distanceSteps = 0;
            //    cells[StartingCell].distanceSteps = 0;
            int NbMainLoop = 0;
            while (true)
            {
                NbMainLoop++;
                bool madeProgress = false;

                // Look at each square on the board.
                foreach (CellInfo cell in _cells)
                    if (cell != null)
                    {
                        // If the square is open, look through valid moves given
                        // the coordinates of that cell.
                        if (SquareOpen(cell))
                        {
                            int passHere = cell.distanceSteps;

                            foreach (CellInfo newCell in ValidMoves(cell))
                            {
                                int newPass = passHere;
                                if (_isInFight)
                                    newPass++;
                                else
                                    newPass += newCell.isDiagonal ? (int)(newCell.weight * 1.414) : newCell.weight;

                                if (newCell.distanceSteps > newPass)
                                {
                                    newCell.distanceSteps = newPass;
                                    madeProgress = true;
                                }
                            }
                        }
                    }
                if (!madeProgress)
                {
                    break;
                }
            }

            if (FirstStepOnly)
                return true;
            // Step 2
            // Mark the path from Exit to Starting position.
            // if several Exit cells, then get the lowest distance one = the closest from one starting cell
            // (or the highest distance one if SelectFartherCells)
            ExitCell = ExitCells[0];
            int MinDist = _cells[ExitCell].distanceSteps;
            if (SelectFartherCells)
            {
                foreach (short cell in ExitCells)
                    if (_cells[cell].distanceSteps > MinDist)
                    {
                        ExitCell = cell;
                        MinDist = _cells[cell].distanceSteps;
                    }
            }
            else
            {
                foreach (short cell in ExitCells)
                    if (_cells[cell].distanceSteps < MinDist)
                    {
                        ExitCell = cell;
                        MinDist = _cells[cell].distanceSteps;
                    }
            }
            //int no = 0;
            //Debug.WriteLine("PathFinding from {0} ({1}) to {2} ({3})", _cells[StartingCells[0]].cell, _cells[StartingCells[0]].distanceSteps, _cells[ExitCell].cell, _cells[ExitCell].distanceSteps);
            /*List<Cell> ListMax = new List<Cell>();
            List<Cell> ListInc = new List<Cell>();
            foreach (var cell in _cells)
            {
                if (cell.distanceSteps >= CellInfo.DEFAULT_DISTANCE)
                    ListMax.Add(cell.cell);
                if (cell.distanceSteps <= MinDist)
                    ListInc.Add(cell.cell);
                //else
                //    continue;
                Debug.Write(String.Format("{0} : {1}, ", cell.cell, cell.distanceSteps));
                if (no++ == 5)
                {
                    Debug.WriteLine("");
                    no = 0;
                }
            }*/
            //Debug.WriteLine("");
            //BotManager.Instance.Bots[0].Character.ResetCellsHighlight();
            //BotManager.Instance.Bots[0].Character.HighlightCells(ListMax, Color.Black);
            //BotManager.Instance.Bots[0].Character.HighlightCells(ListMax, Color.DarkGray);
            short CurrentCell = ExitCell;
            PathResult.Add(ExitCell);
            _cells[ExitCell].isInPath1 = true;
            List<short> LowestPoints = new List<short>(10);
            short lowestPoint;
            int lowest;

            while (true)
            {
                // Look through each direction and find the square
                // with the lowest number of steps marked.
                lowestPoint = CellInfo.CELL_ERROR;
                lowest = CellInfo.DEFAULT_DISTANCE;

                foreach (CellInfo NewCell in ValidMoves(_cells[CurrentCell]))
                {
                    int distance = NewCell.distanceSteps;
                    if (distance > CellInfo.DEFAULT_DISTANCE)
                    {
                        Debug.Assert(false, "Distance shouldn't be higher than DEFAULT_DISTANCE", "Distance = {0} > Max = {1}", distance, CellInfo.DEFAULT_DISTANCE);
                        continue;
                    }
                    if (distance < lowest)
                    {
                        LowestPoints.Clear();
                        lowest = distance;
                        lowestPoint = NewCell.cellId;
                    }
                    else
                        if (distance == lowest)
                        {
                            if (LowestPoints.Count == 0)
                                LowestPoints.Add(lowestPoint);
                            LowestPoints.Add(NewCell.cellId);
                        }
                }
                if (lowest == CellInfo.DEFAULT_DISTANCE) break; // Can't find a valid way :(

                if (LowestPoints.Count > 1) // Several points with same distance =>> randomly select one of them
                    lowestPoint = LowestPoints[rnd.Next(LowestPoints.Count)];

                // Mark the square as part of the path if it is the lowest
                // number. Set the current position as the square with
                // that number of steps.
                PathResult.Add(lowestPoint);
                if (PathResult.Count > _cells.Length)
                {
                    Debug.Assert(false, "PathFinder can't find a path - overflow");
                    break;
                }
                Debug.Assert(_cells[lowestPoint].isInPath1 == false, "Point already in path", "CurrentCell : {0}, Lowest : {1} - distance : {2}", _cells[CurrentCell].cell, _cells[lowestPoint].cell, lowest);
                _cells[lowestPoint].isInPath1 = true;
                CurrentCell = lowestPoint;


                if (_cells[CurrentCell].distanceSteps == 0) // Exit reached            
                {
                    StartingCell = CurrentCell;
                    // We went from closest Exit to a Starting position, so we're finished.
                    break;
                }
            }
            PathResult.Reverse();
            return CurrentCell == StartingCell;
        }

        private bool SquareOpen(CellInfo cell)
        {
            if (cell == null) return false;
            // A square is open if it is not a wall.
            if (_isInFight)
                return cell.isCombatWalkable && cell.isWalkable;
            else
                return cell.isWalkable;
        }

        CellInfo getNeighbourCell(CellInfo cell, int deltaX, int deltaY)
        {
            int NewCellId = cell.getNeighbourCell(deltaX, deltaY);
            CellInfo NewCell = NewCellId == CellInfo.CELL_ERROR ? null : _cells[NewCellId];
            if (!SquareOpen(NewCell)) return null;
            NewCell.isDiagonal = deltaX != 0 && deltaY != 0;
            return NewCell;
        }

        // Return each valid square we can move to.
        private IEnumerable<CellInfo> ValidMoves(CellInfo cell)
        {
            CellInfo newCell;
            if ((newCell = getNeighbourCell(cell, 1, 0)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, 0, 1)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, -1, 0)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, 0, -1)) != null) yield return newCell;
            if (!_isInFight)
            {
                if ((newCell = getNeighbourCell(cell, 1, 1)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, 1, -1)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, -1, 1)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, -1, -1)) != null) yield return newCell;
            }
        }

        #endregion FindPath algorithm itself

        /// <summary>
        /// Return the "flight distance" between to cells. It gives a rought indication on how far they are, 
        /// without processing full PathFinding.
        /// </summary>
        /// <param name="StartCell"></param>
        /// <param name="EndCell"></param>
        /// <param name="iscombatMap"></param>
        /// <returns></returns>
        public double GetFlightDistance(int StartCell, int EndCell)
        {
            return GetFlightDistance(_cells[StartCell], _cells[EndCell], _isInFight);
        }

        static public double GetFlightDistance(CellInfo StartCell, CellInfo EndCell, bool isInFight)
        {
            int dx = Math.Abs(StartCell.x - EndCell.x);
            int dy = Math.Abs(StartCell.y - EndCell.y);
            if (isInFight)
                return dx + dy;
            if (dx > dy)
                return dy * 1.414 /* diagonale part */ + /* straight line part */ dx - dy;
            else
                return dx * 1.414 /* diagonale part */ + /* straight line part */ dy - dx;
        }

        /// <summary>
        /// Compute the exact length of the last path. 
        /// </summary>
        /// <returns></returns>
        public double GetLengthOfLastPath()
        {
            double distance = 0.0;
            for (int i = 1; i < PathResult.Count; i++)
                distance += GetFlightDistance(PathResult[i - 1], PathResult[i]);
            return distance;
        }

        /// <summary>
        /// Gives last index of the list to be used in order to be as close as possible (but under) MinDistance from target
        /// </summary>
        /// <param name="MinDistance"></param>
        /// <returns></returns>
        private int GetIndexForDistance(int MinDistance)
        {
            if (MinDistance <= 0 || PathResult.Count <= 1) return PathResult.Count - 1;
            double distance = 0.0;
            double precDistance = 0.0;
            for (int i = PathResult.Count - 1; i > 0; i--)
            {
                distance += GetFlightDistance(PathResult[i - 1], PathResult[i]);
                if ((int)distance > MinDistance) return i - 1;
                //if (Math.Abs(distance - MinDistance) > Math.Abs(precDistance - MinDistance)) return i - 1;
                precDistance = distance;
            }
            return PathResult.Count - 1;
        }

        /// <summary>
        /// Remove final steps of the path, so that it ends at MinDistance from target (instead of 0)
        /// Warning : call this only once
        /// </summary>
        /// <param name="MinDistance"></param>
        private IEnumerable<short> TruncatePathAtMinDistance(int MinDistance)
        {
            if (MinDistance <= 0) return PathResult;

            int index = GetIndexForDistance(MinDistance);
            return PathResult.Take(index + 1);
        }

        /// <summary>
        /// Returns the last path, unpacked. Remove MinDistance last steps. 
        /// </summary>
        /// <param name="MinDistance"></param>
        /// <returns></returns>
        public short[] GetLastPathUnpacked(int MinDistance, int mp = -1)
        {
            if (mp < 0) return TruncatePathAtMinDistance(MinDistance).ToArray();
            return TruncatePathAtMinDistance(MinDistance).Take(mp + 1).ToArray();
        }

        #region IAdvancedPathFinder
        Path IAdvancedPathFinder.FindPath(IEnumerable<Cell> startCells, IEnumerable<Cell> endCells, bool outsideFight, int mp, int minDistance)
        {
            _isInFight = !outsideFight;
            if (!FindPath(startCells.Select(cell => cell.Id).ToArray(), endCells.Select(cell => cell.Id).ToArray(), false)) //startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCells.FirstOrDefault());
            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].cell));
        }

        Path IAdvancedPathFinder.FindPath(Cell startCell, IEnumerable<Cell> endCells, bool outsideFight, int mp, int minDistance)
        {
            _isInFight = !outsideFight;
            if (!FindPath(startCell.Id, endCells.Select(cell => cell.Id).ToArray(), false)) //startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);
            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].cell));
        }

        Path IAdvancedPathFinder.FindPath(Cell startCell, Cell endCell, bool outsideFight, int mp, int minDistance)
        {
            _isInFight = !outsideFight;
            if (!FindPath(startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);

            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].cell));
        }
        
        #endregion IAdvancedPathFinder

        #region ISimplePathFinder        
        Path ISimplePathFinder.FindPath(Cell startCell, Cell endCell, bool outsideFight, int mp)
        {
            _isInFight = !outsideFight;
            if (!FindPath(startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);

            return new Path(_map, GetLastPathUnpacked(0, mp).Select(cell => _cells[cell].cell));
        }
        #endregion ISimplePathFinder

        /// <summary>
        /// returns the last path after packing it 
        /// </summary>
        /// <param name="MinDistance"></param>
        /// <param name="packed"></param>
        /// <returns></returns>
        public short[] GetLastPackedPath(int MinDistance, int mp = -1)
        {
            return PathPacker.PackPath(GetLastPathUnpacked(MinDistance, mp));
        }


        private List<short> PathBackup;
        private void PushCurrentPath()
        {
            PathBackup = new List<short>(PathResult);
        }

        private void PopCurrentPath()
        {
            PathResult = PathBackup;
        }



        /// <summary>
        /// Find the cell in the middle of several cells (barycenter)
        /// </summary>
        /// <param name="alliesCells"></param>
        /// <param name="foesCells"></param>
        /// <returns></returns>
        public int MiddleCell(int[] Cells)
        {
            int CumulX = 0, CumulY = 0;
            foreach (int cell in Cells)
            {
                CumulX += _cells[cell].x;
                CumulY += _cells[cell].y;
            }
            return CellInfo.CellIdFromPos(CumulX / Cells.Length, CumulY / Cells.Length);
        }

        /// <summary>
        /// Enumerates all cells within a given distance from cellId
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="distanceMax"></param>
        /// <returns></returns>
        public int[] FindCellsAround(int cellId, int distanceMax, bool WalkableOnly, bool LOSNeeded = false)
        {
            List<int> result = new List<int>();
            if (LOSNeeded && ((_map == null))) throw new TypeAccessException("IMap is not a Map");
            int x = _cells[cellId].x;
            int y = _cells[cellId].y;
            for (int px = x - distanceMax; px <= x + distanceMax; px++)
                for (int py = y - distanceMax; py <= y + distanceMax; py++)
                    //if (px >= 0 && py >= 0 && px <= CellInfo.MAP_SIZE && py <= CellInfo.MAP_SIZE) // Within map
                    if ((Math.Abs(x - px) + Math.Abs(y - py)) <= distanceMax) // Close enough
                    {
                        int newCell = CellInfo.CellIdFromPos(px, py);
                        if (newCell != CellInfo.CELL_ERROR)
                            if (!WalkableOnly || _cells[newCell].isWalkable)
                                if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].cell, _cells[newCell].cell))
                                    result.Add(newCell);
                    }
            return result.ToArray();
        }

        /// <summary>
        /// Enumerates all cells in line (horiz or vert) and within a given distance from cellId
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="distanceMax"></param>
        /// <returns></returns>
        public short[] FindCellsInLine(short cellId, int distanceMax, bool WalkableOnly, bool LOSNeeded = false)
        {
            if (LOSNeeded && ((_map == null))) throw new TypeAccessException("IMap is not a Map");

            List<short> result = new List<short>();
            int x = _cells[cellId].x;
            int y = _cells[cellId].y;
            for (int px = x - distanceMax; px <= x + distanceMax; px++)
            {
                short newCell = CellInfo.CellIdFromPos(px, y);
                if (newCell != CellInfo.CELL_ERROR)
                    if (!WalkableOnly || _cells[newCell].isWalkable)
                        if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].cell, _cells[newCell].cell))
                            result.Add(newCell);
            }
            for (int py = y - distanceMax; py <= y + distanceMax; py++)
            {
                short newCell = CellInfo.CellIdFromPos(x, py);
                if (newCell != CellInfo.CELL_ERROR)
                    if (!WalkableOnly || _cells[newCell].isWalkable)
                        if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].cell, _cells[newCell].cell))
                            result.Add(newCell);
            }
            return result.ToArray();
        }



        /// <summary>
        /// Find the best path to fish the closest fish. 
        /// </summary>
        /// <param name="refCell"> Starting position </param>
        /// <param name="fishPositions"> Cells with the fishes </param>
        /// <param name="MaxDistanceFromFishToWalkable"></param>
        /// <returns> The cellId of the selected fish (or CellInfo.CELL_ERROR if not path found)
        /// The current path is valid, as are the starting and finish positions </returns>
        public int FindFishingSpot(short refCell, short[] fishPositions, int MaxDistanceFromFishToWalkable)
        {
            // Gather all walkable positions around each fishes in allExits
            Dictionary<short, short[]> fishWalkSpots = new Dictionary<short, short[]>();
            List<short> allExits = new List<short>();
            foreach (short fishCell in fishPositions)
            {
                short[] WalkableCellsAround = FindCellsInLine(fishCell, MaxDistanceFromFishToWalkable, true);
                fishWalkSpots[fishCell] = WalkableCellsAround;
                foreach (short cell in WalkableCellsAround)
                    if (!allExits.Contains(cell))
                        allExits.Add(cell);
            }

            if (allExits.Count < 1) return CellInfo.CELL_ERROR; // No walkable cell close enough
            // Find the best path (=> ExitCell contrains the selected end point)
            if (!FindPath(refCell, allExits.ToArray())) return CellInfo.CELL_ERROR;

            // Find the corresponding fish
            foreach (var fishStruct in fishWalkSpots)
                foreach (var cell in fishStruct.Value)
                    if (cell == ExitCell)
                        return fishStruct.Key; // returns the corresponding fish cell
            return CellInfo.CELL_ERROR;
        }
    }
}
