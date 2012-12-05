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

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    /// <summary>
    /// This class provides optimized way to detect submaps (= connected regions) within a map.   
    /// </summary>
    public class SubMapProcessor
    {
        #region Properties
        // When in combat, then MapNeighbours are resticted to 4, and distances are manhattan distances
        private bool _isInFight;

        // Cells stores information about each square.
        private CellInfo[] _cells;

        private IMapContext _map;

        public byte SubMapsCount { get; private set; }
        #endregion Properties

        #region Constructor
        // Ctor : provides Map and mode (combat or not)
        public SubMapProcessor(IMapContext map, bool inFightMap)
        {
            _isInFight = inFightMap;
            _map = map;
            _cells = map.Cells.Select(cell => new CellInfo(cell)).ToArray();
            SubMapsCount = SubMapFiller(inFightMap);
        }
        #endregion Constructor

        #region Public data access
        /// <summary>
        /// Returns all cells for the given submap
        /// If subMapId == 0, then returns all non-walkable cells (not set to any subMap). 
        /// </summary>
        /// <param name="subMapId"></param>
        /// <returns></returns>
        public IEnumerable<Cell> GetCellsOfSubMap(byte subMapId)
        {
            if (subMapId > SubMapsCount) yield break;
            foreach (CellInfo cellinfo in _cells)
                if (cellinfo.SubMapId == subMapId)
                    yield return cellinfo.Cell;
        }

        /// <summary>
        /// Says if there is any transition cell in a given direction within a region 
        /// If subMapId == 0, then considers all non-walkable cells (not set to any subMap). 
        /// </summary>
        /// <param name="subMapId"></param>
        /// <returns></returns>
        public bool HasTransition(byte subMapId, MapNeighbour direction)
        {
            int transitionMask = World.Map.MapChangeDatas[direction];
            foreach (CellInfo cellinfo in _cells)
                if (cellinfo.SubMapId == subMapId && (cellinfo.MapLink & transitionMask) != 0)
                    return true;
            return false;
        }

        /// <summary>
        /// Retrieves the transition cells for in given direction within a region 
        /// If subMapId == 0, then considers all non-walkable cells (not set to any subMap). 
        /// </summary>
        /// <param name="subMapId"></param>
        /// <returns></returns>    
        public IEnumerable<Cell> GetTransitionCells(byte subMapId, MapNeighbour direction)
        {
            int transitionMask = World.Map.MapChangeDatas[direction];
            foreach (CellInfo cellinfo in _cells)
                if (cellinfo.SubMapId == subMapId && (cellinfo.MapLink & transitionMask) != 0)
                    yield return cellinfo.Cell;
        }

        #endregion

        #region SubArea filler
        // Identify each unlinked 'submaps' (sets of cells that are not linked together)  
        private byte SubMapFiller(bool inFightMap)
        {
            _isInFight = inFightMap;

            // Reset SubArea data - Mark each cell as from unset subarea
            foreach (var cell in _cells)
                cell.SubMapId = 0;

            byte SubAreaNo = 1;
            foreach (var cell in _cells)
                if (cell.SubMapId == 0 && (SubMapFiller(cell, SubAreaNo) > 0))
                    SubAreaNo++;
            return --SubAreaNo; // Return number of non-empty subregions found
        }

        /// <summary>
        /// PathFinding main method
        /// </summary>
        /// <param name="startingCells"></param>
        /// <param name="exitCells"></param>
        /// <param name="selectFartherCells"></param>
        /// <param name="firstStepOnly"></param>
        /// <returns></returns>
        private int SubMapFiller(CellInfo startingCell, byte RegionNb)
        {
            Debug.Assert(RegionNb > 0);

            // If a wrong or non-walkable cell or already with the given Region number, then return 0. 
            if ((startingCell == null) ||
                 !_map.IsCellWalkable(startingCell.Cell, true, null) || !(!_isInFight || startingCell.IsCombatWalkable) ||
                 (startingCell.SubMapId == RegionNb)
              )
                return 0;

            _isInFight = false;
            Random rnd = new Random();

            List<CellInfo> changed = new List<CellInfo>();
            List<CellInfo> changing;

            int cellCounter = 0;
            startingCell.SubMapId = RegionNb;
            changed.Add(startingCell);

            while (changed.Count > 0)
            {
                cellCounter += changed.Count;

                changing = new List<CellInfo>();
                // Look at each square on the board.
                foreach (CellInfo curCell in changed)
                {
                    //Debug.Assert((curCell != null && curCell.distanceSteps < CellInfo.DEFAULT_DISTANCE));
                    foreach (CellInfo newCell in ValidMoves(curCell, true))
                        if (newCell.SubMapId != RegionNb && _map.IsCellWalkable(newCell.Cell, true, null) && (!_isInFight || newCell.IsCombatWalkable))
                        {
                            newCell.SubMapId = RegionNb;
                            changing.Add(newCell);
                        }
                }

                changed = changing;
            }
            return cellCounter;
        }
        #endregion

        private CellInfo getNeighbourCell(CellInfo cell, int deltaX, int deltaY)
        {
            int NewCellId = cell.GetNeighbourCell(deltaX, deltaY);
            if (NewCellId == CellInfo.CELL_ERROR) return null;
            return _cells[NewCellId];
        }

        // Return each valid square we can move to.
        private IEnumerable<CellInfo> ValidMoves(CellInfo cell, bool fast)
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
    }
}
