using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.World
{
    /// <summary>
    /// This allow to compure and store Line Of Sight information for a given target. 
    /// It is somewhat optimized : on updates when needed, on keep all LoS compute in cache for later use. 
    /// </summary>
    public class LOSMap : IDisposable
    {
        private bool?[] _losMap;
        private Cell _pivotCell;
        private MapContext<Fighter> _mapF;
        private MapContext<RolePlayActor> _mapRP;
        bool _throughEntities;

        private CellList cells
        {
            get
            {
                if (_mapF == null) return _mapRP.Cells;
                return _mapF.Cells;
            }
        }

        private bool CanBeSeen(Cell targetCell, bool throughEntities)
        {
            if (_mapF == null) return _mapRP.CanBeSeen(_pivotCell, targetCell, throughEntities);
            return _mapF.CanBeSeen(_pivotCell, targetCell, throughEntities);
        }

        public LOSMap(MapContext<Fighter> map, bool throughEntities = false)
        {
            _losMap = null;
            _mapF = map;
            _pivotCell = null;
            _throughEntities = throughEntities;
        }

        public LOSMap(MapContext<RolePlayActor> map, bool throughEntities = false)
        {
            _losMap = null;
            _mapRP = map;
            _pivotCell = null;
            _throughEntities = throughEntities;
        }

        /// <summary>
        /// Set a new target for the LoSMap.
        /// If the target and Cell Id are same as previous, and forceUpdate is not set, then nothing is done.
        /// Otherwise, reset the map for new compute of LOS. 
        /// </summary>
        /// <param name="target">The Fighter for which you want to check if he can be seen from any cell of the map</param>
        /// <param name="throughEntities">Set if actors block LoS</param>
        /// <param name="delayComputed">If set, force immediate compute of the full map. Otherwise, only compute needed cells on demand</param>
        /// <param name="forceUpdate">Should be used when obstacles (like actors) may have moved</param>
        /// <returns></returns>
        public bool UpdateTargetCell(Cell pivotCell, bool delayComputed, bool forceUpdate)
        {
            if (pivotCell == null)
            {
                _losMap = null;
                _pivotCell = null;
                return false;
            }

            if (_pivotCell != null && _pivotCell.Id == pivotCell.Id && !forceUpdate) return true;
            _pivotCell = pivotCell;
            _losMap = new bool?[cells.Count()];
            if (!delayComputed)
                foreach (Cell testCell in cells)
                    _losMap[testCell.Id] = CanBeSeen(testCell, _throughEntities);
            return true;
        }

        /// <summary>
        /// Says if id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool this[int cellId]
        {
            get { return _losMap[cellId] ?? (_losMap[cellId] = CanBeSeen(cells[cellId], _throughEntities)).Value; }
        }

        public bool this[Cell cell]
        {
            get { return _losMap[cell.Id] ?? (_losMap[cell.Id] = CanBeSeen(cell, _throughEntities)).Value; }
        }

        /// <summary>
        /// Get all cells that can see (or been seen, it's the same) by the target
        /// </summary>
        /// <param name="setOfCells">Optional set of cells. 
        /// If you provide such a set, then you will get cells from this set that can see the target </param>
        /// <returns></returns>
        public IEnumerable<Cell> GetCellsSeenByTarget(IEnumerable<Cell> setOfCells = null)
        {
            if (setOfCells == null)
                return cells.Where(cell => this[cell]);
            return setOfCells.Where(cell => this[cell]);
        }

        /// <summary>
        /// Get all cells that can NOT see (or NOT been seen, it's the same) by the target
        /// </summary>
        /// <param name="setOfCells">Optional set of cells. 
        /// If you provide such a set, then you will get cells from this set that can NOT see the target </param>
        /// <returns></returns>
        public IEnumerable<Cell> GetCellsNotSeenByTarget(IEnumerable<Cell> setOfCells = null)
        {
            if (setOfCells == null)
                return cells.Where(cell => !this[cell]);
            return setOfCells.Where(cell => !this[cell]);
        }

        void IDisposable.Dispose()
        {
            if (_pivotCell != null)
                _pivotCell = null;
            if (_mapRP != null)
                _mapRP = null;
            if (_mapF != null)
                _mapF = null;
            if (_losMap != null)
                _losMap = null;

        }
    }
}
