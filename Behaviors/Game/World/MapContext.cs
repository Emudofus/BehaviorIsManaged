#region License GNU GPL
// MapContext.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Core.Collections;
using NLog;

namespace BiM.Behaviors.Game.World
{
    public interface IMapContext
    {
        CellList Cells
        {
            get;
        }

        IEnumerable<ContextActor> Actors
        {
            get;
        }

        ContextActor GetActor(int id);
        ContextActor[] GetActors();
        ContextActor[] GetActors(Func<ContextActor, bool> predicate);
        ContextActor[] GetActorsOnCell(int cellId);
        ContextActor[] GetActorsOnCell(Cell cell);
        bool RemoveActor(int id);
        bool IsActor(int id);
        bool IsActorOnCell(int cellId);
        bool IsActorOnCell(Cell cell);
        void Tick(int dt);

        /// <summary>
        /// Says if Cell1 can see Cell2
        /// If not sure, then returns false. 
        /// </summary>
        /// <returns></returns>
        bool CanBeSeen(Cell cell1, Cell cell2, bool throughEntities = true);

        bool IsCellWalkable(Cell cell, bool throughEntities = true, Cell previousCell = null);

        void SetTrap(int NoTrap, int CellId, int radius);
        void UnsetTrap(int NoTrap);
        bool IsTrapped(short CellId);        
    }

    public abstract class MapContext<T> : IMapContext, INotifyPropertyChanged where T : ContextActor
    {
        public const int ElevationTolerance = 11;
        public const uint Width = 14;
        public const uint Height = 20;
        public const uint MapSize = Width * Height * 2;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected SortedList<int, SortedSet<short>> _trappedCells;

        public virtual bool IsTrapped(short CellId)
        {
            return false;
        }
        public virtual void SetTrap(int NoTrap, int CellId, int radius)
        {
        }

        public virtual void UnsetTrap(int NoTrap)
        {            
            if (_trappedCells.ContainsKey(NoTrap))
                _trappedCells.Remove(NoTrap);
        }

        private Dictionary<int, T> _actors = new Dictionary<int, T>();
        private readonly ObservableCollectionMT<T> _collectionActors;
        private readonly ReadOnlyObservableCollectionMT<T> _readOnlyActors;

        public MapContext()
        {
            _collectionActors = new ObservableCollectionMT<T>();
            _readOnlyActors = new ReadOnlyObservableCollectionMT<T>(_collectionActors);
            _trappedCells = new SortedList<int, SortedSet<short>>();
        }


        IEnumerable<ContextActor> IMapContext.Actors
        {
            get { return Actors; }
        }

        public ReadOnlyObservableCollection<T> Actors
        {
            get { return _readOnlyActors; }
        }


        public abstract CellList Cells
        {
            get;
        }

        /// <summary>
        /// Add an actor. Returns false is existing actor has been replaced
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public virtual bool AddActor(T actor)
        {
            if (_actors.ContainsKey(actor.Id))
            {
                RemoveActor(actor.Id);

                _actors.Add(actor.Id, actor);
                _collectionActors.Add(actor);
                return false;
            }

            _actors.Add(actor.Id, actor);
            _collectionActors.Add(actor);

            return true;
        }


        ContextActor[] IMapContext.GetActorsOnCell(Cell cell)
        {
            return GetActorsOnCell(cell);
        }

        public bool RemoveActor(int id)
        {
            T actor;
            if (_actors.TryGetValue(id, out actor))
                return RemoveActor(actor);

            return false;
        }

        public virtual bool RemoveActor(T actor)
        {
            return _actors.Remove(actor.Id) && _collectionActors.Remove(actor);
        }

        public virtual void ClearActors()
        {
            _actors.Clear();
            _collectionActors.Clear();
        }

        ContextActor IMapContext.GetActor(int id)
        {
            return GetActor(id);
        }

        ContextActor[] IMapContext.GetActors()
        {
            return GetActors();
        }

        public ContextActor[] GetActors(Func<ContextActor, bool> predicate)
        {
            return GetActors(predicate as Func<T, bool>);
        }

        ContextActor[] IMapContext.GetActorsOnCell(int cellId)
        {
            return GetActorsOnCell(cellId);
        }

        public T GetActor(int id)
        {
            return Actors.FirstOrDefault(x => x.Id == id);
        }

        public T[] GetActors()
        {
            return Actors.ToArray();
        }

        public TActor[] GetActors<TActor>()
        {
            return Actors.OfType<TActor>().ToArray();
        }

        public TActor GetActor<TActor>(int id)
            where TActor : T
        {
            return Actors.FirstOrDefault(x => x.Id == id && x is TActor) as TActor;
        }

        public T[] GetActors(Func<T, bool> predicate)
        {
            return Actors.Where(predicate).ToArray();
        }

        public TActor GetActor<TActor>(Func<TActor, bool> predicate)
        {
            return Actors.OfType<TActor>().FirstOrDefault(predicate);
        }

        public TActor[] GetActors<TActor>(Func<TActor, bool> predicate)
        {
            return Actors.OfType<TActor>().Where(predicate).ToArray();
        }

        // Even dead one
        public T[] GetActorsOnCell(int cellId)
        {
            return _actors.Values.Where(actor => actor.Cell != null && actor.Cell.Id == cellId).ToArray();
        }

        public T[] GetActorsOnCell(Cell cell)
        {
            return GetActorsOnCell(cell.Id);
        }
        public bool IsActor(int id)
        {
            return _actors.ContainsKey(id);
        }

        // Only if alive
        public bool IsActorOnCell(int cellId)
        {
            return _actors.Values.Any(actor => actor.Cell != null && actor.Cell.Id == cellId && actor.IsAlive);
        }

        public bool IsActorOnCell(Cell cell)
        {
            return IsActorOnCell(cell.Id);
        }

        public abstract void Tick(int dt);

        #region LoS
        /// <summary>
        /// Check if distance from C to the segment [AB] is less or very close to sqrt(2)/2 "units" 
        /// and the projection of C on the line (AB) is inside the segment [AB].
        /// This should give a conservative way to compute LOS. In very close cases 
        /// (where the exact implementation of the LOS algorithm could make the difference), then 
        /// we consider that the LOS is blocked. The safe way. 
        /// </summary>
        private bool TooCloseFromSegment(int cx, int cy, int ax, int ay, int bx, int by)
        {
            const double MIN_DISTANCE_SQUARED = 0.5001;

            // Distance computing is inspired by Philip Nicoletti algorithm - http://forums.codeguru.com/printthread.php?t=194400&pp=15&page=2     
            int numerator = (cx - ax) * (bx - ax) + (cy - ay) * (by - ay);
            int denomenator = (bx - ax) * (bx - ax) + (by - ay) * (by - ay);

            if (numerator > denomenator || numerator < 0)
                return false; //The projection of the point on the line is outside the segment, so it doesn't block the LOS

            double Base = ((ay - cy) * (bx - ax) - (ax - cx) * (by - ay));
            double distanceLineSquared = Base * Base / denomenator;
            return (distanceLineSquared <= MIN_DISTANCE_SQUARED); // if distance to line is frankly over sqrt(2)/2, it won't block LOS. 
        }


        /// <summary>
        /// Says if Cell1 can see Cell2
        /// If not sure, then returns false. 
        /// Warning : cell1 and cell2 are ignored !
        /// </summary>
        /// <returns></returns>
        public bool CanBeSeen(Cell cell1, Cell cell2, bool throughEntities = false)
        {
            if (cell1 == null || cell2 == null) return false;
            if (cell1 == cell2) return true;

            foreach (Cell cell in cell1.GetAllCellsInRectangle(cell2, true, cell => cell != null && !cell.LineOfSight || (!throughEntities && IsActorOnCell(cell))))
                if (TooCloseFromSegment(cell.X, cell.Y, cell1.X, cell1.Y, cell2.X, cell2.Y)) return false;
            return true;
        }
        #endregion

        #region Pathfinding
        public bool IsCellWalkable(Cell cell, bool throughEntities = false, Cell previousCell = null)
        {
            if (!cell.Walkable)
                return false;

            if (cell.NonWalkableDuringRP)
                return false;

            // compare the floors
            if (UsingNewMovementSystem && previousCell != null)
            {
                int floorDiff = Math.Abs(cell.Floor) - Math.Abs(previousCell.Floor);

                if (cell.MoveZone != previousCell.MoveZone ||
                    cell.MoveZone == previousCell.MoveZone && cell.MoveZone == 0 && floorDiff > Map.ElevationTolerance)
                    return false;
            }

            if (!throughEntities && IsActorOnCell(cell))
                return false;

            // todo : LoS => Sure ? LoS may stop a walk ?!

            return true;
        }

        public abstract bool UsingNewMovementSystem
        {
            get;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}