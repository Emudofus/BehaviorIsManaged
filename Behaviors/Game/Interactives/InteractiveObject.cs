#region License GNU GPL
// InteractiveObject.cs
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
using System.Linq;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.World;
using BiM.Core.Collections;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Interactives
{
    public class InteractiveObject : WorldObject
    {
        public delegate void InteractiveUsedHandler(InteractiveObject interactive, RolePlayActor user, InteractiveSkill skill, int delay);
        public event InteractiveUsedHandler Used;

        private readonly ObservableCollectionMT<InteractiveSkill> m_disabledSkills;
        private readonly ObservableCollectionMT<InteractiveSkill> m_enabledSkills;

        private readonly ReadOnlyObservableCollectionMT<InteractiveSkill> m_readOnlyDisabledSkills;
        private readonly ReadOnlyObservableCollectionMT<InteractiveSkill> m_readOnlyEnabledSkills;
        private string m_name;

        public InteractiveObject(Map map, InteractiveElement interactive)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (interactive == null) throw new ArgumentNullException("interactive");
            Id = interactive.elementId;
            Type = interactive.elementTypeId > 0 ? ObjectDataManager.Instance.Get<Interactive>(interactive.elementTypeId) : null;

            Map = map;

            m_enabledSkills = new ObservableCollectionMT<InteractiveSkill>(interactive.enabledSkills.Select(x => new InteractiveSkill(this, x)));
            m_readOnlyEnabledSkills = new ReadOnlyObservableCollectionMT<InteractiveSkill>(m_enabledSkills);

            m_disabledSkills = new ObservableCollectionMT<InteractiveSkill>(interactive.disabledSkills.Select(x => new InteractiveSkill(this, x)));
            m_readOnlyDisabledSkills = new ReadOnlyObservableCollectionMT<InteractiveSkill>(m_disabledSkills);
        }

        public override int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Can be null
        /// </summary>
        public Interactive Type
        {
            get;
            private set;
        }

        public InteractiveAction Action
        {
            get { return Type != null ? (InteractiveAction)Type.actionId : InteractiveAction.None; }
        }

        public string Name
        {
            get
            {
                return Type != null ? m_name ?? (m_name = I18NDataManager.Instance.ReadText(Type.nameId)) : string.Empty;
            }
        }

        public InteractiveState State
        {
            get;
            private set;
        }

        public RolePlayActor UsedBy
        {
            get;
            private set;
        }

        public void DefinePosition(Cell cell)
        {
            Cell = cell;
        }

        public void ResetState()
        {
            State = InteractiveState.None;
        }

        public void NotifyInteractiveUsed(InteractiveUsedMessage message)
        {
            var actor = Map.GetActor(message.entityId);
            var skill = EnabledSkills.Concat(DisabledSkills).FirstOrDefault(x => x.JobSkill != null && x.JobSkill.id == message.skillId);

            var evnt = Used;
            if (evnt != null)
                evnt(this, actor, skill, message.duration);

            if (actor != null)
            {
                actor.NotifyUseInteractive(this, skill, message.duration);

                if (message.duration > 0)
                    UsedBy = actor;
            }
        }


        public void NotifyInteractiveUseEnded()
        {
            if (UsedBy != null)
            {
                UsedBy.NotifyInteractiveUseEnded();
                UsedBy = null;
            }
        }

        public ReadOnlyObservableCollectionMT<InteractiveSkill> EnabledSkills
        {
            get { return m_readOnlyEnabledSkills; }
        }

        public ReadOnlyObservableCollectionMT<InteractiveSkill> DisabledSkills
        {
            get { return m_readOnlyDisabledSkills; }
        }

        public IEnumerable<InteractiveSkill> AllSkills(int? jobId = null, int levelMin = 0, int levelMax = 100)
        {
            foreach (var skill in m_readOnlyEnabledSkills)
                if (jobId == null || skill.JobSkill.parentJobId == jobId)
                    if (skill.JobSkill.levelMin >= levelMin && skill.JobSkill.levelMin <= levelMax) yield return skill;
            foreach (var skill in m_readOnlyDisabledSkills)
                if (jobId == null || skill.JobSkill.parentJobId == jobId)
                    if (skill.JobSkill.levelMin >= levelMin && skill.JobSkill.levelMin <= levelMax) yield return skill;
        }

        public void Update(InteractiveElement interactive)
        {
            if (interactive == null) throw new ArgumentNullException("interactive");

            Type = interactive.elementTypeId > 0 ? ObjectDataManager.Instance.Get<Interactive>(interactive.elementTypeId) : null;
            m_enabledSkills.Clear();
            foreach (var skill in interactive.enabledSkills)
            {
                m_enabledSkills.Add(new InteractiveSkill(this, skill));
            }

            m_disabledSkills.Clear();
            foreach (var skill in interactive.disabledSkills)
            {
                m_disabledSkills.Add(new InteractiveSkill(this, skill));
            }
        }

        public void Update(StatedElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            State = (InteractiveState)element.elementState;
            
            DefinePosition(Map.Cells[element.elementCellId]);
        }

        public bool IsForJob(int jobId)
        {
            return m_disabledSkills.Any(skill => skill.JobSkill.parentJobId == jobId) || m_enabledSkills.Any(skill => skill.JobSkill.parentJobId == jobId);
        }

        // Returns if the ressource is a fish
        public bool IsFish()
        {
            return m_disabledSkills.Any(skill => skill.JobSkill.parentJobId == 36) || m_enabledSkills.Any(skill => skill.JobSkill.parentJobId == 36);
        }

        /// <summary>
        /// Get adjacent cells where Interactive should be usable from
        /// </summary>
        /// <returns></returns>
        public Cell[] GetAdjacentCells()
        {
            if (IsForJob(BiM.Behaviors.Game.Jobs.Job.FISHER))
            {
                return Cell.GetCellsInDirections(new DirectionsEnum[] { DirectionsEnum.DIRECTION_NORTH_EAST, DirectionsEnum.DIRECTION_NORTH_WEST, DirectionsEnum.DIRECTION_SOUTH_WEST, DirectionsEnum.DIRECTION_SOUTH_EAST }, 1, 3)
                            .Where(cell => Map.CanStopOnCell(cell) && Map.CanBeSeen(cell, Cell)).ToArray();
            }
            else
            {
                return Cell.GetAdjacentCells(x => Map.CanStopOnCell(x), true).ToArray();
            }
        }

        /// <summary>
        /// Says if the cell is close enough to use the Interactive
        /// </summary>
        /// <returns></returns>
        public bool IsAdjacentTo(Cell cell)
        {
            if (IsForJob(BiM.Behaviors.Game.Jobs.Job.FISHER))
            {
                return Map.CanStopOnCell(cell) && cell.ManhattanDistanceTo(Cell) < 4 && cell.X == Cell.X && cell.Y == Cell.Y && Map.CanStopOnCell(cell);
            }
            else
            {
                return cell.IsAdjacentTo(Cell, true);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} in {2} ", Name, State, Cell); 
        }
    }
}