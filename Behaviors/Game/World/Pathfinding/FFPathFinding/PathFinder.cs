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
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class PathFinder : ISimplePathFinder, IAdvancedPathFinder
    {
        #region Properties


        public MapMovement PathPacker;

        // When in combat, then MapNeighbours are resticted to 4, and distances are manhattan distances
        private bool _isInFight;
        private bool _isCautious;

        // Cells stores information about each square.
        private CellInfo[] _cells;

        public List<short> PathResult { get; private set; }

        //public Dictionary<int, CellInfo> startingCells;
        //public Dictionary<int, CellInfo> exitCells;
        public short StartingCell { get; private set; }
        public short ExitCell { get; private set; }
        private IMapContext _map;
        #endregion Properties

        #region Constructor
        // Ctor : provides Map and mode (combat or not). Beware, use a Fight context un fight, and a Map out of fights
        public PathFinder(IMapContext map, bool fight)
        {
            Debug.Assert(fight && map is Fight || !fight && map is IMap);
            _isInFight = fight;
            _map = map;
            _cells = new CellInfo[map.Cells.Count];
            for (int i = 0; i < map.Cells.Count; i++)
                _cells[i] = new CellInfo(map.Cells[i]);
            PathPacker = new MapMovement(_cells);
        }

        // Ctor : provides specific cells and mode (combat or not)
        public PathFinder(IMapContext map, IEnumerable<Cell> cells, bool fight)
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
        public void ClearLogic(short[] startingCells, short[] exitCells)
        {
            // Reset some information about the cells.
            foreach (CellInfo cell in _cells)
                if (cell != null)
                {
                    cell.DistanceSteps = CellInfo.DEFAULT_DISTANCE;
                    cell.IsInPath = cell.IsCloseToEnemy = false;
                    cell.IsOpen = null;
                }
            PathResult = new List<short>();
            StartingCell = CellInfo.CELL_ERROR;
            ExitCell = CellInfo.CELL_ERROR;
            if (_isCautious || _isInFight)
            {
                // Find cells of all enemies on the map
                IEnumerable<Cell> enemyCells;
                if (_isInFight)
                {
                    PlayedFighter player = _map.Actors.OfType<PlayedFighter>().FirstOrDefault();
                    if (player != null)
                        enemyCells = player.GetOpposedTeam().FightersAlive.Select(fighter => fighter.Cell);
                    else
                        enemyCells = null;
                }
                else
                    enemyCells = _map.Actors.OfType<GroupMonster>().Select(fighter => fighter.Cell);

                // Remove (/starting and) exit cells from those
                if (enemyCells != null && startingCells != null)
                {
                    //if (startingCells.Length > 0)
                    //    enemyCells = enemyCells.Except(startingCells.Select(cellid => _cells[cellid].Cell));
                    if (exitCells != null && exitCells.Length > 0)
                        enemyCells = enemyCells.Except(exitCells.Select(cellid => _cells[cellid].Cell));

                    // Then for each remainding cell, set all surrouding cells as "isCloseToEnemy"
                    PlayedFighter playedFighter = null;
                    if (_map is Fight)
                    {
                        playedFighter = (_map as Fight).CurrentPlayer as PlayedFighter;
                        //if (playedFighter != null)
                        //    playedFighter.Character.ResetCellsHighlight();
                    }
                    foreach (var cellid in enemyCells.Where(cell => cell != null).SelectMany(cell => cell.GetAdjacentCells()).Select(cell => cell.Id))
                        if (_cells[cellid] != null)
                        {
                            //if (playedFighter != null)
                            //    playedFighter.Character.HighlightCell(cellid, System.Drawing.Color.Black);
                            _cells[cellid].IsCloseToEnemy = true;
                        }
                }

            }
        }

        #region Connexions detector
        public delegate bool FilterCells(CellInfo cell);
        public delegate int OrderingCells(CellInfo cell);
        /// <summary>
        /// This function retreive all cells that can be reached from startingCell. You can include specific filter on CellInfo and also order them as you want (default order is by increasing distance from 
        /// </summary>
        /// <param name="startingCell"></param>
        /// <param name="inFight">true if you're in fight map</param>
        /// <param name="cautious">true when you exclude all path that are throught actors or close to enemies</param>
        /// <param name="filter">You can set here any filter on CellInfos, like criteria on distance from startingCell</param>
        /// <param name="sorter">You tell here how you want the cells to be sorted</param>
        /// <returns></returns>
        public IEnumerable<Cell> FindConnectedCells(Cell startingCell, bool inFight, bool cautious, FilterCells filter = null, OrderingCells sorter = null, int maxDistance = CellInfo.DEFAULT_DISTANCE)
        {
            _isCautious = cautious;
            _isInFight = inFight;
            FindPath(new short[] { startingCell.Id }, null, false, true, maxDistance); // Only 1st step
            var set1 = _cells.Where(cell => cell.DistanceSteps <= maxDistance);
            var count1 = set1.Count();
            var set2 = set1.Where(cell => filter == null || filter(cell));
            var count2 = set2.Count();
            // Avoid to come in a trap if we're not in a trap at start. In all cases, favours cells not trapped if possible.
            return _cells.Where(cell => cell != null && cell.DistanceSteps <= maxDistance && (filter == null || filter(cell)) && /*(_map.IsTrapped(startingCell.Id) || */!_map.IsTrapped(cell.CellId)/*)*/).OrderBy(cell => (sorter == null ? cell.DistanceSteps : sorter(cell)) /*+ (_map.IsTrapped(cell.CellId)?100:0)*/).Select(cell => cell.Cell);
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
            _cells[ExitCell].IsInPath = true;
            int NbStepLeft = distance;
            while (NbStepLeft-- > 0)
            {
                // Look through each MapNeighbour and find the square
                // with the lowest number of steps marked.
                short highestPoint = CellInfo.CELL_ERROR;
                int PreviousDistance = _cells[CurrentCell].DistanceSteps;
                int highest = PreviousDistance;
                foreach (CellInfo NewCell in ValidMoves(_cells[CurrentCell], false))
                {
                    int count = NewCell.DistanceSteps;
                    if (count > highest)
                    {
                        highest = count;
                        highestPoint = NewCell.CellId;
                    }
                }
                if (highest != PreviousDistance)
                {
                    // Mark the square as part of the path if it is the lowest
                    // number. Set the current position as the square with
                    // that number of steps.
                    PathResult.Add(highestPoint);
                    _cells[highestPoint].IsInPath = true;
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
        /// <param name="startingCells"></param>
        /// <param name="exitCells"></param>
        /// <param name="selectFartherCells"></param>
        /// <param name="firstStepOnly"></param>
        /// <returns></returns>
        public bool FindPath(short[] startingCells, short[] exitCells, bool selectFartherCells = false, bool firstStepOnly = false, int maxDistanceParam = CellInfo.DEFAULT_DISTANCE)
        {
            Random rnd = new Random();
            if ((startingCells == null) || (startingCells.Length == 0)) return false; // We need at least one starting stCell
            if (!firstStepOnly && (exitCells == null || exitCells.Length == 0)) return false; // We need at least one exit stCell for step 2

            // PC starts at distance of 0. Set 0 to all possible starting cells
            CellInfo[] changed = new CellInfo[560];
            int changedPtr = 0;
            CellInfo[] changing = new CellInfo[560];
            int changingPtr = 0;

            bool optimizerActiv = !firstStepOnly && !selectFartherCells; // This strong optimization may fail to find a path. In that case, the non-optimized algorithm is run 

            while (true)
            {
                ClearLogic(startingCells, exitCells);
                uint EstimatedDistance = CellInfo.DEFAULT_DISTANCE;
                Cell bestStartingCell = null;
                Cell bestEndingCell = null;

                foreach (short stCell in startingCells)
                    if (_cells[stCell] != null)
                    {
                        _cells[stCell].DistanceSteps = 0;
                        changed[changedPtr++] = _cells[stCell];
                        if (!firstStepOnly && !selectFartherCells)
                            foreach (short exCell in exitCells)
                            {
                                if (exCell == stCell)
                                {
                                    PathResult = new List<short> { stCell };
                                    return true; // Empty path : starting stCell = exit stCell
                                }
                                if (optimizerActiv)
                                {
                                    uint distance = _cells[stCell].Cell.ManhattanDistanceTo(_cells[exCell].Cell);
                                    if (distance < EstimatedDistance)
                                    {
                                        bestStartingCell = _cells[stCell].Cell;
                                        bestEndingCell = _cells[exCell].Cell;
                                        EstimatedDistance = distance;
                                    }
                                }
                            }
                    }
                //    cells[StartingCell].distanceSteps = 0;
                int maxDistance = maxDistanceParam; // We won't search over this distance - this optimization is OK in all cases
                if (optimizerActiv && bestStartingCell == null || bestEndingCell == null)
                    optimizerActiv = false;

                while (changedPtr > 0)
                {
                    changingPtr = 0;
                    // Look at each square on the board.
                    while (changedPtr > 0)
                    {
                        CellInfo curCell = changed[--changedPtr];
                        if (curCell.IsCloseToEnemy && _isCautious)
                            continue; // Cautious mode (in or out of fight) : Can't move from a cell near an ennemy

                        if (curCell.DistanceSteps < maxDistance)
                        {
                            if (optimizerActiv) // Strong optimisation
                            {
                                uint lastEstimatedDistance = curCell.Cell.ManhattanDistanceTo(bestEndingCell);
                                uint startDistance = curCell.Cell.ManhattanDistanceTo(bestStartingCell);
                                if (startDistance + lastEstimatedDistance > EstimatedDistance) continue;
                            }
                            //Debug.Assert((curCell != null && curCell.DistanceSteps < CellInfo.DEFAULT_DISTANCE));
                            short[] cellNeighbours = neighbours[curCell.CellId];
                            for (short i = 0; i < cellNeighbours.Length; i++)
                            {
                                CellInfo newCell = _cells[cellNeighbours[i]];
                                if (newCell == null) continue;
                                if (newCell.DistanceSteps != 0 && !SquareOpen(newCell, null)) continue;
                                //uint currentDistance = newCell.Cell.ManhattanDistanceTo(_cells[exitCells[0]].Cell);                            
                                //if (currentDistance >= EstimatedDistance || currentDistance >= lastEstimatedDistance) continue;

                                int newPass = curCell.DistanceSteps;
                                if (curCell.IsCloseToEnemy)
                                    newPass++; // Penality when close of an ennemy (same in fight and RP map)
                                if (_isInFight)
                                    newPass++;
                                else
                                    newPass += newCell.Weight;

                                if (newCell.DistanceSteps > newPass)
                                {
                                    newCell.DistanceSteps = newPass;
                                    changing[changingPtr++] = newCell;
                                    if (!firstStepOnly && !selectFartherCells && newPass < maxDistance && exitCells.Any(id => newCell.CellId == id))
                                        maxDistance = newPass; // We won't search on distance over closest exit
                                }
                            }
                            if (_isInFight) continue;
                            cellNeighbours = diagNeighbours[curCell.CellId];
                            for (short i = 0; i < cellNeighbours.Length; i++) // Process diagonals
                            {
                                CellInfo newCell = _cells[cellNeighbours[i]];
                                if (newCell == null) continue;
                                if (newCell.DistanceSteps != 0 && !SquareOpen(newCell, null)) continue;

                                int newPass = curCell.DistanceSteps;
                                if (curCell.IsCloseToEnemy)
                                    newPass++; // Penality when close of an ennemy (same in fight and RP map)

                                if (_isInFight)
                                    newPass++;
                                else
                                    newPass += (int)(newCell.Weight * 1.414);

                                if (newCell.DistanceSteps > newPass)
                                {
                                    newCell.DistanceSteps = newPass;
                                    changing[changingPtr++] = newCell;
                                    if (!firstStepOnly && !selectFartherCells && newPass < maxDistance && exitCells.Any(id => newCell.CellId == id))
                                        maxDistance = newPass;  // We won't search on distance over closest exit
                                }
                            }
                        }
                    }
                    CellInfo[] tmpChanged = changed;
                    changed = changing;
                    changedPtr = changingPtr;
                    changing = tmpChanged;
                }
                if (firstStepOnly)
                    return true;
                // Step 2
                // Mark the path from Exit to Starting position.
                // if several Exit cells, then get the lowest distance one = the closest from one starting cell
                // (or the highest distance one if selectFartherCells)
                ExitCell = exitCells[0];
                int MinDist = _cells[ExitCell].DistanceSteps;
                foreach (short cell in exitCells)
                    if ((selectFartherCells && _cells[cell].DistanceSteps > MinDist) || (!selectFartherCells && _cells[cell].DistanceSteps < MinDist))
                    {
                        ExitCell = cell;
                        MinDist = _cells[cell].DistanceSteps;
                    }

                if (optimizerActiv == false || MinDist < CellInfo.DEFAULT_DISTANCE) break; // No need to run a second unoptimized algorithm
                else
                    optimizerActiv = false;
            }
            //int no = 0;
            //Debug.WriteLine("PathFinding from {0} ({1}) to {2} ({3})", _cells[startingCells[0]].cell, _cells[startingCells[0]].distanceSteps, _cells[ExitCell].cell, _cells[ExitCell].distanceSteps);
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
            _cells[ExitCell].IsInPath = true;
            CellInfo[] lowestPoints = changed; // No ned to alloc a new one, this one won't be used anymore
            int lowestPointsPtr = 0;
            short lowestPoint;
            int lowest;

            while (true)
            {
                // Look through each MapNeighbour and find the square
                // with the lowest number of steps marked.
                lowestPoint = CellInfo.CELL_ERROR;
                lowest = CellInfo.DEFAULT_DISTANCE;
                short[] neighbours = GetNeighbours(CurrentCell, _isInFight);
                for (short i = 0; i < neighbours.Length; i++)
                {
                    //foreach (CellInfo newCell in ValidMoves(curCell, false))
                    CellInfo newCell = _cells[neighbours[i]];
                    if (newCell == null || (newCell.IsCloseToEnemy && _isCautious && newCell.DistanceSteps != 0)) continue; // In cautious mode, don't come close to an enemy

                    //for (CellInfo NewCell in ValidMoves(_cells[CurrentCell], true))
                    //{
                    int distance = newCell.DistanceSteps;
                    /*if (distance > CellInfo.DEFAULT_DISTANCE)
                    {
                        Debug.Assert(false, "Distance shouldn't be higher than DEFAULT_DISTANCE", "Distance = {0} > Max = {1}", distance, CellInfo.DEFAULT_DISTANCE);
                        continue;
                    }*/
                    if (distance < lowest)
                    {
                        lowestPointsPtr = 1;
                        lowest = distance;
                        lowestPoints[0] = newCell;
                    }
                    else
                        if (distance == lowest)
                            lowestPoints[lowestPointsPtr++] = newCell;
                }
                if (lowest == CellInfo.DEFAULT_DISTANCE) break; // Can't find a valid way :(

                if (lowestPointsPtr > 1) // Several points with same distance =>> randomly select one of them
                    lowestPoint = lowestPoints[rnd.Next(lowestPointsPtr)].CellId;
                else
                    lowestPoint = lowestPoints[0].CellId;

                // Mark the square as part of the path if it is the lowest
                // number. Set the current position as the square with
                // that number of steps.
                PathResult.Add(lowestPoint);
                if (PathResult.Count > _cells.Length)
                {
                    Debug.Assert(false, "PathFinder can't find a path - overflow");
                    break;
                }
                //Debug.Assert(_cells[lowestPoint].IsInPath == false, "Point already in path", "CurrentCell : {0}, Lowest : {1} - distance : {2}, path : {3}", _cells[CurrentCell].Cell, _cells[lowestPoint].Cell, lowest, string.Join(",", _cells.Where(stCell => stCell.IsInPath)));
                _cells[lowestPoint].IsInPath = true;
                CurrentCell = lowestPoint;


                if (_cells[CurrentCell].DistanceSteps == 0) // Exit reached            
                {
                    StartingCell = CurrentCell;
                    // We went from closest Exit to a Starting position, so we're finished.
                    break;
                }
            }
            PathResult.Reverse();
            return CurrentCell == StartingCell;
        }

        private bool SquareOpen(CellInfo cell, CellInfo originCell = null)
        {
            if (cell == null) return false;

            if (cell.IsOpen == null)
                if (_isInFight)
                    cell.IsOpen = cell.IsCombatWalkable && _map.IsCellWalkable(cell.Cell, false, originCell == null ? null : originCell.Cell)/* && !cell.IsCloseToEnemy*/;
                else
                    cell.IsOpen = _map.IsCellWalkable(cell.Cell, !_isCautious, originCell == null ? null : originCell.Cell)/* && (!_isCautious || !cell.IsCloseToEnemy)*/;
            return cell.IsOpen.Value;
        }

        CellInfo getNeighbourCell(CellInfo cell, int deltaX, int deltaY, bool fast)
        {
            int NewCellId = cell.GetNeighbourCell(deltaX, deltaY);
            CellInfo NewCell = NewCellId == CellInfo.CELL_ERROR ? null : _cells[NewCellId];
            if (fast || NewCell == null || NewCell.DistanceSteps == 0) return NewCell;
            if (!SquareOpen(NewCell, cell)) return null;
            NewCell.IsDiagonal = deltaX != 0 && deltaY != 0;
            return NewCell;
        }

        // Return each valid square we can move to.
        private IEnumerable<CellInfo> ValidMoves(CellInfo cell, bool fast)
        {
            CellInfo newCell;
            if ((newCell = getNeighbourCell(cell, 1, 0, fast)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, 0, 1, fast)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, -1, 0, fast)) != null) yield return newCell;
            if ((newCell = getNeighbourCell(cell, 0, -1, fast)) != null) yield return newCell;
            if (!_isInFight)
            {
                if ((newCell = getNeighbourCell(cell, 1, 1, fast)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, 1, -1, fast)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, -1, 1, fast)) != null) yield return newCell;
                if ((newCell = getNeighbourCell(cell, -1, -1, fast)) != null) yield return newCell;
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
            int dx = Math.Abs(StartCell.X - EndCell.X);
            int dy = Math.Abs(StartCell.Y - EndCell.Y);
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
        Path IAdvancedPathFinder.FindPath(IEnumerable<Cell> startCells, IEnumerable<Cell> endCells, bool outsideFight, int mp, int minDistance, bool cautiousMode)
        {
            _isInFight = !outsideFight;
            _isCautious = cautiousMode;
            if (!FindPath(startCells.Select(cell => cell.Id).ToArray(), endCells.Select(cell => cell.Id).ToArray(), false)) //startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCells.FirstOrDefault());
            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].Cell));
        }

        Path IAdvancedPathFinder.FindPath(Cell startCell, IEnumerable<Cell> endCells, bool outsideFight, int mp, int minDistance, bool cautiousMode)
        {
            _isInFight = !outsideFight;
            _isCautious = cautiousMode;
            if (!FindPath(startCell.Id, endCells.Select(cell => cell.Id).ToArray(), false)) //startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);
            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].Cell));
        }

        Path IAdvancedPathFinder.FindPath(Cell startCell, Cell endCell, bool outsideFight, int mp, int minDistance, bool cautiousMode)
        {
            _isInFight = !outsideFight;
            _isCautious = cautiousMode;
            if (!FindPath(startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);

            return new Path(_map, GetLastPathUnpacked(minDistance, mp).Select(cell => _cells[cell].Cell));
        }

        #endregion IAdvancedPathFinder

        #region ISimplePathFinder
        Path ISimplePathFinder.FindPath(Cell startCell, Cell endCell, bool outsideFight, int mp)
        {
            _isInFight = !outsideFight;
            if (!FindPath(startCell.Id, endCell.Id))
                return Path.GetEmptyPath(_map, startCell);

            return new Path(_map, GetLastPathUnpacked(0, mp).Select(cell => _cells[cell].Cell));
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
                CumulX += _cells[cell].X;
                CumulY += _cells[cell].Y;
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
            int x = _cells[cellId].X;
            int y = _cells[cellId].Y;
            for (int px = x - distanceMax; px <= x + distanceMax; px++)
                for (int py = y - distanceMax; py <= y + distanceMax; py++)
                    //if (px >= 0 && py >= 0 && px <= CellInfo.MAP_SIZE && py <= CellInfo.MAP_SIZE) // Within map
                    if ((Math.Abs(x - px) + Math.Abs(y - py)) <= distanceMax) // Close enough
                    {
                        int newCell = CellInfo.CellIdFromPos(px, py);
                        if (newCell != CellInfo.CELL_ERROR)
                            if (!WalkableOnly || _cells[newCell].IsWalkable)
                                if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].Cell, _cells[newCell].Cell))
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
            int x = _cells[cellId].X;
            int y = _cells[cellId].Y;
            for (int px = x - distanceMax; px <= x + distanceMax; px++)
            {
                short newCell = CellInfo.CellIdFromPos(px, y);
                if (newCell != CellInfo.CELL_ERROR)
                    if (!WalkableOnly || _cells[newCell].IsWalkable)
                        if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].Cell, _cells[newCell].Cell))
                            result.Add(newCell);
            }
            for (int py = y - distanceMax; py <= y + distanceMax; py++)
            {
                short newCell = CellInfo.CellIdFromPos(x, py);
                if (newCell != CellInfo.CELL_ERROR)
                    if (!WalkableOnly || _cells[newCell].IsWalkable)
                        if (!LOSNeeded || _map.CanBeSeen(_cells[cellId].Cell, _cells[newCell].Cell))
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

        #region Fast compute of neighbours  statics
        static public short[] GetNeighbours(short cellId, bool fighting)
        {
            if (fighting) return neighbours[cellId];
            return bothNeighbours[cellId];
        }

        static short[][] diagNeighbours = ComputeNeighbours(false);
        static short[][] neighbours = ComputeNeighbours(true);
        static short[][] bothNeighbours = ComputeNeighbours(null);

        private static short[][] ComputeNeighbours(bool? fighting)
        {
            short[][] result = new short[560][];
            for (short i = 0; i < 560; i++)
                result[i] = Neighbours(i, fighting).ToArray();
            return result;
        }

        // Return each valid square we can move to.
        static IEnumerable<short> Neighbours(short cellId, bool? fighting)
        {
            short x = CellInfo.XFromId(cellId);
            short y = CellInfo.YFromId(cellId);
            short id;
            if (fighting != false)
            {
                id = CellInfo.CellIdFromPos(x + 1, y);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x, y + 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x - 1, y);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x, y - 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
            }

            if (fighting != true)
            {
                id = CellInfo.CellIdFromPos(x + 1, y + 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x + 1, y - 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x - 1, y + 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
                id = CellInfo.CellIdFromPos(x - 1, y - 1);
                if (id != CellInfo.CELL_ERROR) yield return id;
            }
        }
        /// </summary>
        #endregion
    }
}
