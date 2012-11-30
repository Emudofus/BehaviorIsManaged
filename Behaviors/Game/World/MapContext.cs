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
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using NLog;

namespace BiM.Behaviors.Game.World
{
    public interface IMapContext
    {
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
    }

    public abstract class MapContext<T> : IMapContext, INotifyPropertyChanged where T : ContextActor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, T> m_actors = new Dictionary<int, T>();
        private readonly ObservableCollectionMT<T> m_collectionActors;
        private readonly ReadOnlyObservableCollectionMT<T> m_readOnlyActors;

        public MapContext()
        {
            m_collectionActors = new ObservableCollectionMT<T>();
            m_readOnlyActors = new ReadOnlyObservableCollectionMT<T>(m_collectionActors);
        }


        IEnumerable<ContextActor> IMapContext.Actors
        {
            get { return Actors; }
        }

        public ReadOnlyObservableCollection<T> Actors
        {
            get { return m_readOnlyActors; }
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
            if (m_actors.ContainsKey(actor.Id))
            {
                RemoveActor(actor.Id);

                m_actors.Add(actor.Id, actor);
                m_collectionActors.Add(actor);
                return false;
            }

            m_actors.Add(actor.Id, actor);
            m_collectionActors.Add(actor);

            return true;
        }


        ContextActor[] IMapContext.GetActorsOnCell(Cell cell)
        {
            return GetActorsOnCell(cell);
        }

        public bool RemoveActor(int id)
        {
            T actor;
            if (m_actors.TryGetValue(id, out actor))
                return RemoveActor(actor);

            return false;
        }

        public virtual bool RemoveActor(T actor)
        {
            return m_actors.Remove(actor.Id) && m_collectionActors.Remove(actor);
        }

        public virtual void ClearActors()
        {
            m_actors.Clear();
            m_collectionActors.Clear();
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

        public T[] GetActorsOnCell(int cellId)
        {
            return Actors.Where(actor => actor.Cell != null && actor.Cell.Id == cellId).ToArray();
        }

        public T[] GetActorsOnCell(Cell cell)
        {
            return GetActorsOnCell(cell.Id);
        }
        public bool IsActor(int id)
        {
            return m_actors.ContainsKey(id);
        }

        public bool IsActorOnCell(int cellId)
        {
            return m_collectionActors.Any(actor => actor.Cell != null && actor.Cell.Id == cellId);
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
            const double MIN_DISTANCE_SQUARED = 0.500;

            // Distance computing is inspired by Philip Nicoletti algorithm - http://forums.codeguru.com/printthread.php?t=194400&pp=15&page=2     
            int numerator = ( cx - ax ) * ( bx - ax ) + ( cy - ay ) * ( by - ay );
            int denomenator = ( bx - ax ) * ( bx - ax ) + ( by - ay ) * ( by - ay );

            if (numerator > denomenator || numerator < 0)
                return false; //The point is outside the segment, so it doesn't block the LOS

            double Base = ( ( ay - cy ) * ( bx - ax ) - ( ax - cx ) * ( by - ay ) );
            double distanceLineSquared = Base * Base / denomenator;
            return ( distanceLineSquared <= MIN_DISTANCE_SQUARED ); // if distance to line is frankly over sqrt(2)/2, it won't block LOS. 
        }

        /// <summary>
        /// Says if Cell1 can see Cell2
        /// If not sure, then returns false. 
        /// </summary>
        /// <returns></returns>
        public bool CanBeSeen(Cell cell1, Cell cell2, bool throughEntities = true)
        {
            if (cell1 == null || cell2 == null) return false;
            if (cell1 == cell2) return true;

            int x1, x2;
            if (cell1.X < cell2.X)
            {
                x1 = cell1.X;
                x2 = cell2.X;
            }
            else
            {
                x1 = cell2.X;
                x2 = cell1.X;
            }

            int y1, y2;
            if (cell1.Y < cell2.Y)
            {
                y1 = cell1.Y;
                y2 = cell2.Y;
            }
            else
            {
                y1 = cell2.Y;
                y2 = cell1.Y;
            }

            int cellIdStart, cellIdEnd;
            if (cell1.Id < cell2.Id)
            {
                cellIdStart = cell1.Id;
                cellIdEnd = cell2.Id;
            }
            else
            {
                cellIdStart = cell2.Id;
                cellIdEnd = cell1.Id;
            }

            Cell info;
            for (int cellId = cellIdStart + 1; cellId < cellIdEnd; cellId++)
            {
                info = Cells[cellId];

                if (info == null)
                    return false;

                bool blocker = !info.LineOfSight || ( !throughEntities && IsActorOnCell(info) );

                if (blocker && info.X >= x1 && info.X <= x2 && info.Y >= y1 && info.Y <= y2)
                    // If one obstacle on the LOS, returns false
                    if (TooCloseFromSegment(info.X, info.Y, cell1.X, cell1.Y, cell2.X, cell2.Y))
                        return false;
            }
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