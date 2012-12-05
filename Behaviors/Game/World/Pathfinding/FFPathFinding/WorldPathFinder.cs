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

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class WorldPathFinder
    {
        const uint DEFAULT_DISTANCE = 0xFFFFFFFF;

        //Int32 posRandom;

        public class WorldCellInfo
        {
            public uint distanceSteps = DEFAULT_DISTANCE;
            public bool isInPath = false;
        }
        // Cells stores information about each square.
        public WorldCellInfo[] cells { get; private set; }
        private Int16[][] links { get; set; }
        private uint[] mapPenalties { get; set; }
        Random rnd = new Random();

        // Ctor : provides cells array and mode (combat or not)
        public WorldPathFinder(Int16[][] MapLinks, uint[] Penalties)
        {
            this.cells = cells;
            links = MapLinks;
            mapPenalties = Penalties;
        }

        /// <summary>
        /// Reset old PathFinding path from the cells.
        /// </summary>
        public void ClearLogic()
        {
            // Reset some information about the cells.
            if (cells == null)
                cells = new WorldCellInfo[links.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == null)
                {
                    cells[i] = null;
                }

                cells[i].distanceSteps = DEFAULT_DISTANCE;
                cells[i].isInPath = false;
            }

            if (PathResult == null)
                PathResult = new List<int>();
            else
                PathResult.Clear();
        }

        public List<int> PathResult { get; private set; }

        //public Dictionary<int, CellInfo> startingCells;
        //public Dictionary<int, CellInfo> exitCells;
        public int StartingCell { get; private set; }
        public int ExitCell { get; private set; }

        #region FindPath algorithm itself

        /// <summary>
        /// Entry point for PathFinding algorithm, with one starting cell, and one exit
        /// </summary>
        public bool FindPath(int StartingSubMapId, int ExitSubMapId)
        {
            int[] ExitStartingSubMapIds = new int[] { ExitSubMapId };
            return FindPath(StartingSubMapId, ExitStartingSubMapIds);
        }

        /// <summary>
        /// Entry point for PathFinding algorithm, with the new SubMapId of the Destination (usually the leader' position)
        /// </summary>
        public bool ProcessNewDistancesFromDestination(int destinationSubMapId)
        {
            return FindPath(destinationSubMapId, null, true);
        }

        /// <summary>
        /// Find where the team member should move from its current position to reach the leader
        /// Return -1 if no path can be found
        /// If several possible, then get a random one
        /// </summary>
        public int GetNextSubMapIdToReachTheDestination(int MemberCurrentSubMapId, bool RandomPath = true)
        {
            int BestNextCell = -1;
            if (cells == null) throw new Exception("Cells are not initialized");
            uint BestChoiceDistance = DEFAULT_DISTANCE;
            foreach (int NextSubMap in links[MemberCurrentSubMapId])
                if (cells[NextSubMap].distanceSteps < BestChoiceDistance)
                {
                    BestNextCell = NextSubMap;
                    BestChoiceDistance = cells[NextSubMap].distanceSteps;
                }
                else
                    if ((cells[NextSubMap].distanceSteps == BestChoiceDistance) && (RandomPath) && (rnd.Next(2) == 0)) // If 2 possible cells have same value, choose randomly
                    {
                        BestNextCell = NextSubMap;
                    }
            return BestNextCell;
        }


        /// <summary>
        /// PathFinding main method
        /// </summary>
        /// <param name="startingCells"></param>
        /// <param name="exitCells"></param>
        /// <param name="selectFartherCells"></param>
        /// <param name="firstStepOnly"></param>
        /// <returns></returns>
        public bool FindPath(int StartingSubMapId, int[] ExitSubMapIds, bool FirstStepOnly = false)
        {
            Random rnd = new Random();
            ClearLogic();
            if (cells == null) throw new Exception("Cells are not initialized");
            if (StartingSubMapId < 0 || StartingSubMapId >= cells.Length) return false; // We need at least one starting cell
            if (!FirstStepOnly && (ExitSubMapIds == null || ExitSubMapIds.Length == 0)) return false; // We need at least one exit cell for step 2
            // PC starts at distance of 0. Set 0 to all possible starting cells
            cells[StartingSubMapId].distanceSteps = 0;

            //    cells[StartingCell].distanceSteps = 0;
            int NbMainLoop = 0;
            while (true)
            {
                NbMainLoop++;
                bool madeProgress = false;

                // Look at each square on the board.
                for (int CurrentSubMap = 0; CurrentSubMap < cells.Length; CurrentSubMap++)
                {
                    Int16[] LocalLinks = links[CurrentSubMap];
                    uint penalty = 0;

                    uint nexMapDistance = cells[CurrentSubMap].distanceSteps + 1;
                    foreach (Int16 Link in LocalLinks)
                    {
                        if (mapPenalties != null)
                            penalty = mapPenalties[Link];
                        if (cells[Link].distanceSteps > nexMapDistance + penalty)
                        {
                            cells[Link].distanceSteps = nexMapDistance + penalty;
                            madeProgress = true;
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
            // (or the highest distance one if selectFartherCells)
            ExitCell = ExitSubMapIds[0];
            uint MinDist = cells[ExitCell].distanceSteps;
            foreach (int cell in ExitSubMapIds)
                if (cells[cell].distanceSteps < MinDist)
                {
                    ExitCell = cell;
                    MinDist = cells[cell].distanceSteps;
                }
            int CurrentCell = ExitCell;
            PathResult.Add(ExitCell);
            cells[ExitCell].isInPath = true;
            // Look through each MapNeighbour and find the square
            // with the lowest number of steps marked.
            int lowestPoint;
            uint lowest;
            List<int> LowestPoints = new List<int>(10);
            while (true)
            {
                // Look through each MapNeighbour and find the square
                // with the lowest number of steps marked.
                lowestPoint = CellInfo.CELL_ERROR;
                lowest = DEFAULT_DISTANCE;

                foreach (int NewSubMapId in links[CurrentCell])
                {
                    uint count = cells[NewSubMapId].distanceSteps;
                    if (count < lowest)
                    {
                        LowestPoints.Clear();
                        lowest = count;
                        lowestPoint = NewSubMapId;
                    }
                    else
                        if (count == lowest) // If more than one point, then push it in the list, for random determination
                        {
                            if (LowestPoints.Count == 0)
                                LowestPoints.Add(lowestPoint);
                            LowestPoints.Add(NewSubMapId);
                        }
                }
                if (lowest == DEFAULT_DISTANCE) break; // Can't find a valid way :(
                if (LowestPoints.Count > 1) // Several points with same distance =>> randomly select one of them
                    lowestPoint = LowestPoints[rnd.Next(LowestPoints.Count)];

                // Mark the subMap as part of the path if it is the lowest
                // number. Set the current position as the subMap with
                // that number of steps.
                PathResult.Add(lowestPoint);
                cells[lowestPoint].isInPath = true;
                CurrentCell = lowestPoint;

                if (cells[CurrentCell].distanceSteps == 0) // Exit reached            
                {
                    StartingCell = CurrentCell;
                    // We went from closest Exit to a Starting position, so we're finished.
                    break;
                }
            }
            PathResult.Reverse(); // Reorder the path from starting position to target
            return CurrentCell == StartingCell;
        }
        #endregion FindPath algorithm itself


        /// <summary>
        /// Compute the exact length of the last path. 
        /// </summary>
        /// <returns></returns>
        public int GetLengthOfLastPath()
        {
            return PathResult.Count - 1;
        }

        /// <summary>
        /// Returns the last path. 
        /// </summary>
        /// <param name="MinDistance"></param>
        /// <returns></returns>
        public int[] GetLastPath()
        {
            return PathResult.ToArray();
        }

    }
}
