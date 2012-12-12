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
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.World;
using BiM.Core.Collections;
using BiM.Protocol.Data;
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
            get { return Type != null ? (InteractiveAction) Type.actionId : InteractiveAction.None; }
        }

        public string Name
        {
            get
            {
                return Type != null ? m_name ?? ( m_name = I18NDataManager.Instance.ReadText(Type.nameId) ) : string.Empty;
            }
        }

        /// <summary>
        /// Can be null
        /// </summary>
        public override Cell Cell
        {
            get;
            protected set;
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
            State = (InteractiveState) element.elementState;
            DefinePosition(Map.Cells[element.elementCellId]);
        }
    }
}