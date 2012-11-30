using System.Collections.Generic;
using System.Linq;
using BiM.Behaviors.Game.Actors.Fighters;
using System;

namespace BiM.Behaviors.Game.World
{
    /// <summary>
    /// This allow to compure and store Line Of Sight information for a given target. 
    /// It is somewhat optimized : on updates when needed, on keep all LoS compute in cache for later use. 
    /// </summary>
    public class LOSMap : IDisposable
    {
        private bool?[] _losMap;
        private Fighter _fighter;
        private int _fighterCellId;

        public LOSMap()
        {
            _losMap = null;
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
        public bool UpdateTargetCell(Fighter target, bool throughEntities, bool delayComputed, bool forceUpdate)
        {
            if (target == null)
            {
                _losMap = null;            
                _fighter = null;
                return false;
            }
            if (target == _fighter && _fighterCellId == target.Cell.Id && !forceUpdate) return true;
            _fighterCellId = target.Cell.Id;
            _fighter = target;
            _losMap = null;
            if (target == null) return false;
            _losMap = new bool?[target.Map.Cells.Count()];
            if (target.Cell == null) return false;
            if (!delayComputed)
                foreach (Cell testCell in target.Map.Cells)
                    _losMap[testCell.Id] = target.Context.CanBeSeen(target.Cell, testCell);
            return true;
        }

        /// <summary>
        /// Says if id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool this[int cellId]
        {
            get { return _losMap[cellId] ?? (_losMap[cellId] = _fighter.Context.CanBeSeen(_fighter.Cell, _fighter.Map.Cells[cellId])).Value; }
        }

        public bool this[Cell cell]
        {
            get { return _losMap[cell.Id] ?? (_losMap[cell.Id] = _fighter.Context.CanBeSeen(_fighter.Cell, cell)).Value; }
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
                return _fighter.Map.Cells.Where(cell => this[cell]);
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
                return _fighter.Map.Cells.Where(cell => !this[cell]);
            return setOfCells.Where(cell => !this[cell]);
        }

        void IDisposable.Dispose()
        {
            if (_fighter != null)
                _fighter = null;
            if (_losMap != null)
                _losMap = null;

        }
    }
}
