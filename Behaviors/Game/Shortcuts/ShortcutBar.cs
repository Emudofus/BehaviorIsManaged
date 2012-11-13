#region License GNU GPL
// ShortcutBar.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class ShortcutBar<T>
        where T : Shortcut
    {
        private ObservableCollectionMT<T> m_shortcuts; 
        private ReadOnlyObservableCollectionMT<T> m_readOnlyShortcuts; 

        public ShortcutBar(PlayedCharacter character)
        {
            if (character == null) throw new ArgumentNullException("character");
            Character = character;
            m_shortcuts = new ObservableCollectionMT<T>();
            m_readOnlyShortcuts = new ReadOnlyObservableCollectionMT<T>(m_shortcuts);
        }

        public PlayedCharacter Character
        {
            get;
            protected set;
        }

        public abstract ShortcutBarEnum BarType
        {
            get;
        }

        public ReadOnlyObservableCollectionMT<T> Shortcuts
        {
            get { return m_readOnlyShortcuts; }
        }

        public T Get(int slot)
        {
            return Shortcuts.FirstOrDefault(x => x.Slot == slot);
        }

        public void Add(T shortcut)
        {
            m_shortcuts.Add(shortcut);
        }

        public bool Remove(T shortcut)
        {
            return m_shortcuts.Remove(shortcut);
        }

        public bool Remove(int slot)
        {
            var item = Get(slot);

            if (item == null)
                return false;

            return m_shortcuts.Remove(item);
        }

        public void Clear()
        {
            m_shortcuts.Clear();
        }

        public abstract void Update(ShortcutBarContentMessage content);
        public abstract void Update(ShortcutBarRefreshMessage message);
    }
}